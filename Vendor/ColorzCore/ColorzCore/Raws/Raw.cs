using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColorzCore.DataTypes;
using ColorzCore.Parser.AST;

namespace ColorzCore.Raws
{
    internal class Raw
    {
        private readonly IList<Tuple<int, int, int>> _fixedParams; //position, length, value
        private readonly int _length;
        private readonly IList<IRawParam> _myParams;
        private readonly bool _repeatable;
        private readonly IMaybe<int> _terminatingList;

        /***
         * Flags: 
         priority
          Affects where disassembly uses the code. Existing priorities are:
          main, low, pointer, unit, moveManual, shopList, ballista, ASM,
          battleData, reinforcementData and unknown.

         repeatable
          Means that the last parameter can be repeated and for every 
          repetition a new code is made. Currently requires code to have
          only one parameter.

         unsafe
          EA normally checks for things like parameter collisions and
          other index errors. With this flag, you can bypass them.
          Do not use unless you know what you are doing.

         end
          Means that the code ends disassembly of particular branch in 
          chapter-wide disassembly or in disassembly to end. 

         indexMode
          Affect how many bits lengths and positions mean. 8 means lengths 
          and positions are in bytes. Default is 1.

         terminatingList
          Means that the code is a variable length array of parameters which
          ends in specified value. Requires for code to have only one parameter.

         offsetMod
          The modulus in which the beginning offset of the code has to be 0.
          Default is 4. 

         noAssembly
          Forbids code from participating in assembly.

         noDisassembly
          Forbids code from participating in disassembly.
  */

        public Raw(string name, int length, short code, int offsetMod, HashSet<string> game, IList<IRawParam> varParams,
            IList<Tuple<int, int, int>> fixedParams, IMaybe<int> terminatingList, bool repeatable)
        {
            Name = name;
            this._length = length;
            Code = code;
            Game = game;
            OffsetMod = offsetMod;
            _myParams = varParams;
            this._fixedParams = fixedParams;
            this._terminatingList = terminatingList;
            this._repeatable = repeatable;
        }

        public string Name { get; }
        public short Code { get; }
        public int OffsetMod { get; }
        public HashSet<string> Game { get; }

        public static Raw CopyWithNewName(Raw baseRaw, string newName)
        {
            return new Raw(newName, baseRaw._length, baseRaw.Code, baseRaw.OffsetMod, baseRaw.Game, baseRaw._myParams,
                baseRaw._fixedParams, baseRaw._terminatingList, baseRaw._repeatable);
        }

        public static IList<Raw> ParseAllRaws(FileStream fs)
        {
            var r = new StreamReader(fs);
            RawParseException.filename = fs.Name;
            IList<Raw> myRaws = new List<Raw>();
            while (!r.EndOfStream)
                try
                {
                    var temp = ParseRaw(r);
                    if (temp != null)
                    {
                        myRaws.Add(temp);
                        if (temp.Name[0] != '_')
                            //TODO: Make implicit inclusion of _0xDEAD codes etc
                            //Raw temp2 = Raw.CopyWithNewName(temp, '_0x')
                            ;
                    }
                }
                catch (EndOfStreamException)
                {
                }

            return myRaws;
        }

        public static Raw ParseRaw(StreamReader r)
        {
            //Since the writer of the raws is expected to know what they're doing, I'm going to be a lot more lax with error messages and graceful failure.
            string rawLine;
            do
            {
                rawLine = r.ReadLine();
                if (rawLine != null && (rawLine.Trim().Length == 0 || rawLine[0] == '#'))
                    continue;
                break;
            } while (rawLine != null);

            if (rawLine == null)
                return null; //End of stream reached
            if (char.IsWhiteSpace(rawLine[0]))
                throw new RawParseException("Raw not at start of line.", rawLine);
            var parts = rawLine.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var
                name = parts[0].Trim()
                    .ToUpperInvariant(); //Note all raws implicitly have all uppercase names -- this is to allow for case-insensitive comparison down the line. TODO: Make case sensitivity a requirement?
            var code = parts[1].Trim();
            var length = parts[2].Trim();
            var flags = parts.Length == 4 ? parts[3].Trim() : "";
            Dictionary<string, Flag> flagDict;
            try
            {
                flagDict = ParseFlags(flags);
            }
            catch (Exception e)
            {
                throw new RawParseException(e.Message, rawLine);
            }

            var indexMode = flagDict.ContainsKey("indexMode") ? flagDict["indexMode"].Values.GetLeft[0].ToInt() : 1;
            var lengthVal = indexMode * length.ToInt();
            IList<IRawParam> parameters = new List<IRawParam>();
            IList<Tuple<int, int, int>> fixedParams = new List<Tuple<int, int, int>>();
            while (!r.EndOfStream)
            {
                var nextChar = r.Peek();
                if (nextChar == '#')
                {
                    r.ReadLine();
                    continue;
                }

                if (char.IsWhiteSpace((char)nextChar) && (rawLine = r.ReadLine()).Trim().Length > 0)
                {
                    var possiblyParam = ParseParam(rawLine, indexMode);
                    if (possiblyParam.IsLeft)
                        parameters.Add(possiblyParam.GetLeft);
                    else
                        fixedParams.Add(possiblyParam.GetRight);
                }
                else
                {
                    break;
                }
            }

            if (!flagDict.ContainsKey("unsafe"))
            {
                //TODO: Check for parameter offset collisions
            }

            var game = flagDict.ContainsKey("game") ? new HashSet<string>(flagDict["game"].Values.GetLeft) :
                flagDict.ContainsKey("language") ? new HashSet<string>(flagDict["language"].Values.GetLeft) :
                new HashSet<string>();
            var offsetMod = flagDict.ContainsKey("offsetMod") ? flagDict["offsetMod"].Values.GetLeft[0].ToInt() : 4;
            var terminatingList = flagDict.ContainsKey("terminatingList")
                ? new Just<int>(flagDict["terminatingList"].Values.GetLeft[0].ToInt())
                : (IMaybe<int>)new Nothing<int>();
            if (!terminatingList.IsNothing && code.ToInt() != 0)
                throw new RawParseException("TerminatingList with code nonzero.", rawLine);
            var repeatable = flagDict.ContainsKey("repeatable");
            if ((repeatable || !terminatingList.IsNothing) && parameters.Count > 1 && fixedParams.Count > 0)
                throw new RawParseException(
                    "Repeatable or terminatingList code with multiple parameters or fixed parameters.", rawLine);
            return new Raw(name, lengthVal, (short)code.ToInt(), offsetMod, game, parameters, fixedParams,
                terminatingList, repeatable);
        }

        public static IEither<IRawParam, Tuple<int, int, int>> ParseParam(string paramLine, int indexMode)
        {
            if (!char.IsWhiteSpace(paramLine[0]))
                throw new RawParseException("Raw param does not start with whitespace.", paramLine);
            var parts = paramLine.Trim().Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var name = parts[0];
            var position = parts[1];
            var length = parts[2];
            var flags = parts.Length == 4 ? parts[3].Trim() : "";
            Dictionary<string, Flag> flagDict;
            try
            {
                flagDict = ParseFlags(flags);
            }
            catch (Exception e)
            {
                throw new RawParseException(e.Message, paramLine);
            }

            var positionBits = position.ToInt() * indexMode;
            var lengthBits = length.ToInt() * indexMode;

            if (flagDict.ContainsKey("fixed"))
                return new Right<IRawParam, Tuple<int, int, int>>(new Tuple<int, int, int>(positionBits, lengthBits,
                    name.ToInt()));
            if (flagDict.ContainsKey("coordinate") || flagDict.ContainsKey("coordinates"))
            {
                var coordNum = (flagDict.ContainsKey("coordinate") ? flagDict["coordinate"] : flagDict["coordinates"])
                    .Values;
                var nCoords = coordNum.IsLeft
                    ? coordNum.GetLeft.Max(s => s.ToInt())
                    : Math.Max(coordNum.GetRight.Item1, coordNum.GetRight.Item2);
                return new Left<IRawParam, Tuple<int, int, int>>(new ListParam(name, positionBits, lengthBits,
                    nCoords));
            }

            var pointer = flagDict.ContainsKey("pointer");
            return new Left<IRawParam, Tuple<int, int, int>>(new AtomicParam(name, positionBits, lengthBits, pointer));
        }

        private static Dictionary<string, Flag> ParseFlags(string flags)
        {
            var temp = new Dictionary<string, Flag>();
            if (flags.Length > 0)
            {
                var parts = flags.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var flag in parts)
                {
                    if (flag[0] != '-')
                        throw new Exception("Flag does not start with '-'");
                    var withoutDash = flag.Substring(1);
                    if (withoutDash.Contains(':'))
                    {
                        var parts2 = withoutDash.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        var flagName = parts2[0];
                        if (parts2.Length == 2 && withoutDash.Contains('-'))
                        {
                            var parts3 = parts2[1].Split(new char[1] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                            temp[flagName] = new Flag(parts3[0].ToInt(), parts3[1].ToInt());
                        }
                        else
                        {
                            temp[flagName] = new Flag(new List<string>(parts2.Skip(1)));
                        }
                    }
                    else
                    {
                        temp[withoutDash] = new Flag();
                    }
                }
            }

            return temp;
        }

        public int LengthBits(int paramCount)
        {
            if (_repeatable)
                return _length * paramCount;
            if (!_terminatingList.IsNothing)
                return _myParams[0].Length * (paramCount + 1);
            return _length;
        }

        public int LengthBytes(int paramCount)
        {
            return (LengthBits(paramCount) + 7) / 8;
        }

        public bool Fits(IList<IParamNode> parameters)
        {
            if (parameters.Count == _myParams.Count)
            {
                for (var i = 0; i < parameters.Count; i++)
                    if (!_myParams[i].Fits(parameters[i]))
                        return false;
                return true;
            }

            if (_repeatable || !_terminatingList.IsNothing)
            {
                foreach (var p in parameters)
                    if (!_myParams[0].Fits(p))
                        return false;
                return true;
            }

            return false;
        }

        /* Precondition: params fits the shape of this raw's params. */
        public byte[] GetBytes(IList<IParamNode> parameters)
        {
            var data = new BitArray(0);
            //Represent a code's bytes as a list/array of its length.
            if (!_repeatable && _terminatingList.IsNothing)
            {
                data.Length = _length;
                if (Code != 0)
                {
                    int temp = Code;
                    for (var i = 0; i < 0x10; i++, temp >>= 1) data[i] = (temp & 1) == 1;
                }

                for (var i = 0; i < _myParams.Count; i++) _myParams[i].Set(data, parameters[i]);
                foreach (var fp in _fixedParams)
                {
                    var val = fp.Item3;
                    for (var i = fp.Item1; i < fp.Item1 + fp.Item2; i++, val >>= 1) data[i] = (val & 1) == 1;
                }
            }
            else if (_repeatable)
            {
                foreach (var p in parameters)
                {
                    var localData = new BitArray(_length);
                    if (Code != 0)
                    {
                        int temp = Code;
                        for (var i = 0; i < 0x10; i++, temp >>= 1) localData[i] = (temp & 1) == 1;
                    }

                    _myParams[0].Set(localData, p);
                    data.Append(localData);
                }
            }
            else
            {
                //Is a terminatingList.
                var terminator = _terminatingList.FromJust;
                for (var i = 0; i < parameters.Count; i++)
                {
                    var localData = new BitArray(_myParams[0].Length);
                    _myParams[0].Set(localData, parameters[i]);
                    data.Append(localData);
                }

                var term = new BitArray(_myParams[0].Length);
                ((AtomicParam)_myParams[0]).Set(term, terminator);
                data.Append(term);
            }

            var myBytes = new byte[(data.Length + 7) / 8];
            data.CopyTo(myBytes, 0);
            return myBytes;
        }

        public class RawParseException : Exception
        {
            public static string filename;
            public string rawline;

            public RawParseException(string s, string rawline) : base(s)
            {
                this.rawline = rawline;
            }
        }
    }
}
