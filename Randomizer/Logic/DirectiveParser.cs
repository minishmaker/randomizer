using MinishRandomizer.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinishRandomizer.Randomizer.Logic
{

    public class DirectiveParser
    {
        public uint? RomCrc;
        public string LogicName;
        public string LogicVersion;
        public List<LogicOption> Options;
        private List<LogicDefine> Defines;
        public List<EventDefine> EventDefines;
        private int IfCounter;

        public DirectiveParser()
        {
            Options = new List<LogicOption>();
            Defines = new List<LogicDefine>();
            EventDefines = new List<EventDefine>();
            IfCounter = 0;
        }

        private string[] SplitDirective(string line)
        {
            // Split define into parameters
            string[] untrimmed = line.Split('-');

            // Trim each parameter down
            string[] trimmed = new string[untrimmed.Length];
            for (int i = 0; i < untrimmed.Length; i++)
            {
                trimmed[i] = untrimmed[i].Trim();
            }

            return trimmed;
        }

        /// <summary>
        /// Check if a directive should be parsed on load or on randomization
        /// </summary>
        /// <param name="line">The line that contains the given directive</param>
        /// <returns>True if the directive should be parsed on load, throws an error if the directive is invalid.</returns>
        public bool ParseOnLoad(string line)
        {
            string directive = SplitDirective(line)[0];
            switch (directive)
            {
                case "!flag":
                case "!color":
                case "!name":
                case "!version":
                case "!crc":
                    return true;
                case "!define":
                case "!eventdefine":
                case "!ifdef":
                case "!ifndef":
                case "!else":
                case "!elseifdef":
                case "!elseifndef":
                case "!endif":
                    return false;
                default:
                    throw new ParserException($"\"{directive}\" is not a valid directive! The logic file may be bad.");
            }
        }

        public void ParseDirective(string line)
        {
            string[] mainDirectiveParts = SplitDirective(line);

            switch (mainDirectiveParts[0])
            {
                case "!ifdef":
                case "!ifndef":
                case "!else":
                case "!elseifdef":
                case "!elseifndef":
                case "!endif":
                    ParseConditionalDirective(mainDirectiveParts);
                    return;
            }

            if (!ShouldIgnoreLines())
            {
                switch (mainDirectiveParts[0])
                {
                    case "!crc":
                        RomCrc = ParseCrcDirective(mainDirectiveParts);
                        break;
                    case "!flag":
                        Options.Add(ParseFlagDirective(mainDirectiveParts));
                        break;
                    case "!color":
                        Options.Add(ParseColorDirective(mainDirectiveParts));
                        break;
                    case "!define":
                        Defines.Add(ParseDefineDirective(mainDirectiveParts));
                        break;
                    case "!eventdefine":
                        EventDefines.Add(ParseEventDefine(mainDirectiveParts));
                        break;
                    case "!name":
                        LogicName = mainDirectiveParts[1];
                        break;
                    case "!version":
                        LogicVersion = mainDirectiveParts[1];
                        break;
                    default:
                        throw new ParserException($"\"{mainDirectiveParts[0]}\" is not a valid directive!");
                }
            }
        }

        private void ParseConditionalDirective(string[] directiveParts)
        {
            switch (directiveParts[0])
            {
                case "!ifdef":
                    if (ShouldIgnoreLines() || !IsDefined(directiveParts[1]))
                    {
                        IfCounter++;
                    }
                    return;
                case "!ifndef":
                    if (ShouldIgnoreLines() || IsDefined(directiveParts[1]))
                    {
                        IfCounter++;
                    }
                    return;
                case "!else":
                    if (ShouldIgnoreLines())
                    {
                        if (IfCounter <= 1)
                        {
                            IfCounter--;
                        }
                    }
                    else
                    {
                        IfCounter++;
                    }
                    return;
                /*Currently removed cause it causes problems
                 * case "!elseifdef":
                    if (ShouldIgnoreLines())
                    {
                        if (IfCounter <= 1 && IsDefined(directiveParts[1]))
                        {
                            IfCounter--;
                        }
                    }
                    else
                    {
                        IfCounter++;
                    }
                    return;
                case "!elseifndef":
                    if (ShouldIgnoreLines())
                    {
                        if (IfCounter <= 1 && IsDefined(directiveParts[1]))
                        {
                            IfCounter--;
                        }
                    }
                    else
                    {
                        IfCounter++;
                    }
                    return;*/
                case "!endif":
                    if (IfCounter > 0)
                    {
                        IfCounter--;
                    }
                    return;
            }
        }

        private bool IsDefined(string defineText)
        {
            foreach (LogicDefine define in Defines)
            {
                if (define.Name == defineText)
                {
                    return true;
                }
            }

            return false;
        }

        public void ClearDefines()
        {
            Defines.Clear();
            EventDefines.Clear();
        }

        public void ClearOptions()
        {
            Options.Clear();
        }

        public void AddOptions()
        {
            foreach (LogicOption option in Options)
            {
                Defines.AddRange(option.GetLogicDefines());
            }
        }

        public string ReplaceDefines(string locationString)
        {
            string outString = locationString;
            foreach (LogicDefine define in Defines)
            {
                if (define.CanReplace(outString))
                {
                    outString = define.Replace(outString);
                }

                if (outString.IndexOf("`") == -1)
                {
                    return outString;
                }
            }

            throw new ParserException($"{locationString} has an invalid/undefined define!");
        }

        public bool ShouldIgnoreLines()
        {
            Console.WriteLine(IfCounter);
            return IfCounter != 0;
        }

        private LogicFlag ParseFlagDirective(string[] directiveParts)
        {
            if (directiveParts.Length < 4)
            {
                throw new ParserException("A flag somewhere has an incorrect number of parameters!");
            }

            LogicOptionType optionType = GetOptionType(directiveParts[1]);

            if (optionType == LogicOptionType.Untyped)
            {
                throw new ParserException($"A flag somewhere has an invalid type! ({directiveParts[1]})");
            }

            bool defaultActive;
            if (directiveParts.Length >= 5)
            {
                if (!bool.TryParse(directiveParts[4], out defaultActive))
                {
                    throw new ParserException($"{directiveParts[4]} is not a valid boolean value");
                }
            }
            else
            {
                defaultActive = false;
            }

            return new LogicFlag(directiveParts[2], directiveParts[3], defaultActive, optionType);
        }

        private LogicDefine ParseDefineDirective(string[] directiveParts)
        {
            if (directiveParts.Length == 2)
            {
                return new LogicDefine(directiveParts[1]);
            }
            else if (directiveParts.Length == 3)
            {
                return new LogicDefine(directiveParts[1], directiveParts[2]);
            }
            else
            {
                throw new ParserException($"A define somewhere has an incorrect number of parameters! ({directiveParts.Length})");
            }
        }

        private LogicColorPicker ParseColorDirective(string[] directiveParts)
        {
            LogicOptionType optionType = GetOptionType(directiveParts[1]);

            if (optionType == LogicOptionType.Untyped)
            {
                throw new ParserException($"A color somewhere has an invalid type! ({directiveParts[1]})");
            }

            if (directiveParts.Length == 4)
            {
                return new LogicColorPicker(directiveParts[2], directiveParts[3], optionType, Color.White);
            }
            else if (directiveParts.Length % 3 == 1)
            {

                List<Color> colors = new List<Color>();
                for (int i = 4; i < directiveParts.Length; i += 3)
                {
                    // This parses the color components out, in groups of three.
                    if (int.TryParse(directiveParts[i], NumberStyles.HexNumber, null, out int rComponent))
                    {
                        if (int.TryParse(directiveParts[i + 1], NumberStyles.HexNumber, null, out int gComponent))
                        {
                            if (int.TryParse(directiveParts[i + 2], NumberStyles.HexNumber, null, out int bComponent))
                            {
                                colors.Add(Color.FromArgb(rComponent, gComponent, bComponent));
                            }
                        }
                    }
                }

                return new LogicColorPicker(directiveParts[2], directiveParts[3], optionType, colors);
            }
            else
            {
                throw new ParserException($"A color somewhere has an incorrect number of parameters! ({directiveParts.Length})");
            }
        }

        private EventDefine ParseEventDefine(string[] directiveParts)
        {
            if (directiveParts.Length == 2)
            {
                return new EventDefine(directiveParts[1]);
            }
            else if (directiveParts.Length == 3) {
                return new EventDefine(directiveParts[1], directiveParts[2]);
            }
            else
            {
                throw new ParserException($"An event define somewhere has an incorrect number of parameters! ({directiveParts.Length})");
            }
        }

        private LogicOptionType GetOptionType(string typeString)
        {
            if (Enum.TryParse(typeString, out LogicOptionType result))
            {
                return result;
            }
            else
            {
                return LogicOptionType.Untyped;
            }
        }

        private uint ParseCrcDirective(string[] directiveParts)
        {
            if (uint.TryParse(directiveParts[1], NumberStyles.HexNumber, null, out uint outCrc))
            {
                return outCrc;
            }
            else
            {
                throw new ParserException($"\"{directiveParts[1]}\" is not a valid crc!");
            }
        }
    }
}
