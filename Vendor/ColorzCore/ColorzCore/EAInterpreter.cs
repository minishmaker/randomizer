using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColorzCore.DataTypes;
using ColorzCore.IO;
using ColorzCore.Lexer;
using ColorzCore.Parser;
using ColorzCore.Parser.AST;
using ColorzCore.Preprocessor;
using ColorzCore.Raws;

namespace ColorzCore
{
    //Class to excapsulate all steps in EA script interpretation.
    internal class EaInterpreter
    {
        private readonly Dictionary<string, IList<Raw>> _allRaws;
        private readonly Stream _fout;
        private string _game;
        private readonly string _iFile;
        private readonly Log _log;
        private readonly EaParser _myParser;
        private EaOptions _opts;
        private readonly Stream _sin;

        public EaInterpreter(string game, string rawsFolder, string rawsExtension, Stream sin, string inFileName,
            Stream fout, Log log, EaOptions opts)
        {
            this._game = game;

            try
            {
                _allRaws = ProcessRaws(game, LoadAllRaws(rawsFolder, rawsExtension));
            }
            catch (Raw.RawParseException e)
            {
                var loc = new Location
                {
                    file = Raw.RawParseException
                        .filename, // I get that this looks bad, but this exception happens at most once per execution... TODO: Make this less bad.
                    lineNum = e.rawline.ToInt(),
                    colNum = 1
                };

                log.Message(Log.MsgKind.ERROR, loc, "An error occured while parsing raws");
                log.Message(Log.MsgKind.ERROR, loc, e.Message);

                Environment.Exit(-1);
            }

            this._sin = sin;
            this._fout = fout;
            this._log = log;
            _iFile = inFileName;
            this._opts = opts;

            var includeSearcher = new IncludeFileSearcher();
            includeSearcher.IncludeDirectories.Add(AppDomain.CurrentDomain.BaseDirectory);

            foreach (var path in opts.includePaths)
                includeSearcher.IncludeDirectories.Add(path);

            var toolSearcher = new IncludeFileSearcher { AllowRelativeInclude = false };
            toolSearcher.IncludeDirectories.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tools"));

            foreach (var path in opts.toolsPaths)
                includeSearcher.IncludeDirectories.Add(path);

            _myParser = new EaParser(_allRaws, log, new DirectiveHandler(includeSearcher, toolSearcher));

            _myParser.Definitions['_' + game + '_'] = new Definition();
            _myParser.Definitions["__COLORZ_CORE__"] = new Definition();
        }

        public bool Interpret()
        {
            var t = new Tokenizer();
            var myRom = new Rom(_fout);

            IList<ILineNode> lines = new List<ILineNode>(_myParser.ParseAll(t.Tokenize(_sin, _iFile)));

            /* First pass on AST: Identifier resolution.
             * 
             * Suppose we had the code
             * 
             * POIN myLabel
             * myLabel:
             * 
             * At parse time, myLabel did not exist for the POIN. 
             * It is at this point we want to make sure all references to identifiers are valid, before assembling.
             */
            var undefinedIds = new List<Token>();
            foreach (var line in lines)
                try
                {
                    line.EvaluateExpressions(undefinedIds);
                }
                catch (MacroInvocationNode.MacroException e)
                {
                    _myParser.Error(e.CausedError.MyLocation, "Unexpanded macro.");
                }

            foreach (var errCause in undefinedIds)
                if (errCause.Content.StartsWith(Pool.PooledLabelPrefix, StringComparison.Ordinal))
                    _myParser.Error(errCause.Location, "Unpooled data (forgot #pool?)");
                else
                    _myParser.Error(errCause.Location, "Undefined identifier: " + errCause.Content);

            /* Last step: assembly */

            if (!_log.HasErrored)
            {
                foreach (var line in lines)
                {
                    if (Program.Debug) _log.Message(Log.MsgKind.DEBUG, line.PrettyPrint(0));

                    line.WriteData(myRom);
                }

                myRom.WriteRom();

                _log.Output.WriteLine("No errors. Please continue being awesome.");
                return true;
            }

            _log.Output.WriteLine("Errors occurred; no changes written.");
            return false;
        }

        public bool WriteNocashSymbols(TextWriter output)
        {
            foreach (var label in _myParser.GlobalScope.Head.LocalLabels())
                // TODO: more elegant offset to address mapping
                output.WriteLine("{0:X8} {1}", label.Value + 0x8000000, label.Key);

            return true;
        }

        public void AddDefinitions(Dictionary<string, Definition> definitions)
        {
            // https://stackoverflow.com/questions/294138/merging-dictionaries-in-c-sharp cause I don't know a better way. Let me know if there is one!
            definitions.ToList().ForEach(x => _myParser.Definitions.Add(x.Key, x.Value));
        }

        private static IList<Raw> LoadAllRaws(string rawsFolder, string rawsExtension)
        {
            string folder;
            var directoryInfo = new DirectoryInfo(rawsFolder);
            folder = Path.GetFullPath(rawsFolder);
            var files = directoryInfo.GetFiles("*" + rawsExtension, SearchOption.AllDirectories);
            IEnumerable<Raw> allRaws = new List<Raw>();
            foreach (var fileInfo in files)
            {
                var fs = new FileStream(fileInfo.FullName, FileMode.Open);
                allRaws = allRaws.Concat(Raw.ParseAllRaws(fs));
                fs.Close();
            }

            return new List<Raw>(allRaws);
        }

        private static Dictionary<string, IList<Raw>> ProcessRaws(string game, IList<Raw> allRaws)
        {
            var retVal = new Dictionary<string, IList<Raw>>();
            foreach (var r in allRaws)
                if (r.Game.Contains(game))
                    retVal.AddTo(r.Name, r);
            return retVal;
        }
    }
}
