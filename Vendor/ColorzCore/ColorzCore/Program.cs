using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ColorzCore.IO;
using ColorzCore.Lexer;
using ColorzCore.Parser;

namespace ColorzCore
{
    public static class Program
    {
        private const int ExitSuccess = 0;
        private const int ExitFailure = 1;
        public static bool Debug = false;
        public static Stream CustomOutputStream { get; set; }

        private static string[] _helpstringarr =
        {
            "EA Colorz Core. Usage:",
            "./ColorzCore <A|D> <game> [-opts]",
            "",
            "Only A is allowed as assembly mode currently.",
            "Game may be any string; the respective _game_ variable gets defined in scripts.",
            "Available options:",
            "-raws:<dir>",
            "   Sets the raws directory to the one provided (relative to ColorzCore). Defaults to \"Language Raws\".",
            "-rawsExt:<ext>",
            "   Sets the extension of files used for raws to the one provided. Defaults to .txt.",
            "-output:<filename>",
            "   Set the file to write assembly to.",
            "-input:<filename>",
            "   Set the file to take input script from. Defaults to stdin.",
            "-error:<filename>",
            "   Set a file to redirect messages, warnings, and errors to. Defaults to stderr.",
            "-werr",
            "   Treat all warnings as errors and prevent assembly.",
            "--no-mess",
            "   Suppress output of messages.",
            "--no-warn",
            "   Suppress output of warnings.",
            "--quiet",
            "   Equivalent to --no-mess --no-warn.",
            "-h|--help",
            "   Display helpstring and exit.",
            "-debug",
            "   Enable debug mode. Not recommended for end users."
        };

        private static string _helpstring =
            Enumerable.Aggregate(_helpstringarr, (String a, String b) => { return a + '\n' + b; }) + '\n';

        public static int Main(string[] args)
        {
            var options = new EaOptions();

            var rawSearcher = new IncludeFileSearcher();
            rawSearcher.IncludeDirectories.Add(AppDomain.CurrentDomain.BaseDirectory);

            var inStream = Console.OpenStandardInput();
            var inFileName = "stdin";

            Stream outStream = null;
            var outFileName = "none";

            var errorStream = Console.Error;

            var rawsFolder = rawSearcher.FindDirectory("Language Raws");
            var rawsExtension = ".txt";

            for (var i = 2; i < args.Length; i++)
            {
                if (args[i][0] != '-')
                {
                    Console.Error.WriteLine("Unrecognized paramter: " + args[i]);
                }
                else
                {
                    var flag = args[i].Substring(1).Split(new char[] { ':' }, 2);

                    try
                    {
                        switch (flag[0])
                        {
                            case "raws":
                                rawsFolder = rawSearcher.FindDirectory(flag[1]);
                                break;

                            case "rawsExt":
                                rawsExtension = flag[1];
                                break;

                            case "output":
                                outFileName = flag[1];
                                outStream = CustomOutputStream ?? File.Open(outFileName, FileMode.Open,
                                    FileAccess.ReadWrite); //TODO: Handle file not found exceptions
                                break;

                            case "input":
                                inFileName = flag[1];
                                inStream = File.OpenRead(flag[1]);
                                break;

                            case "error":
                                errorStream = new StreamWriter(File.OpenWrite(flag[1]));
                                options.noColoredLog = true;
                                break;

                            case "debug":
                                Debug = true;
                                break;

                            case "werr":
                                options.werr = true;
                                break;

                            case "-no-mess":
                                options.nomess = true;
                                break;

                            case "-no-warn":
                                options.nowarn = true;
                                break;

                            case "-no-colored-log":
                                options.noColoredLog = true;
                                break;

                            case "quiet":
                                options.nomess = true;
                                options.nowarn = true;
                                break;

                            case "-nocash-sym":
                                options.nocashSym = true;
                                break;

                            case "I":
                            case "-include":
                                options.includePaths.Add(flag[1]);
                                break;

                            case "T":
                            case "-tools":
                                options.toolsPaths.Add(flag[1]);
                                break;

                            case "IT":
                            case "TI":
                                options.includePaths.Add(flag[1]);
                                options.toolsPaths.Add(flag[1]);
                                break;

                            case "h":
                            case "-help":
                                Console.Out.WriteLine(_helpstring);
                                return ExitSuccess;

                            default:
                                Console.Error.WriteLine("Unrecognized flag: " + flag[0]);
                                return ExitFailure;
                        }
                    }
                    catch (IOException e)
                    {
                        Console.Error.WriteLine("Exception: " + e.Message);
                        return ExitFailure;
                    }
                }
            }

            if (args.Length < 2)
            {
                Console.WriteLine("Required parameters missing.");
                return ExitFailure;
            }

            if (args[0] != "A")
            {
                Console.WriteLine("Only assembly is supported currently.");
                return ExitFailure;
            }

            if (outStream == null)
            {
                Console.Error.WriteLine("No output specified for assembly.");
                return ExitFailure;
            }

            if (rawsFolder.IsNothing)
            {
                Console.Error.WriteLine("Couldn't find raws folder");
                return ExitFailure;
            }

            var game = args[1];

            //FirstPass(Tokenizer.Tokenize(inputStream));

            var log = new Log
            {
                Output = errorStream,
                WarningsAreErrors = options.werr,
                NoColoredTags = options.noColoredLog
            };

            if (options.nowarn)
                log.IgnoredKinds.Add(Log.MsgKind.WARNING);

            if (options.nomess)
                log.IgnoredKinds.Add(Log.MsgKind.MESSAGE);

            var myInterpreter = new EaInterpreter(game, rawsFolder.FromJust, rawsExtension, inStream, inFileName,
                outStream, log, options);

            var success = myInterpreter.Interpret();

            if (success && options.nocashSym)
            {
                using (var output = File.CreateText(Path.ChangeExtension(outFileName, "sym")))
                {
                    if (!(success = myInterpreter.WriteNocashSymbols(output)))
                    {
                        log.Message(Log.MsgKind.ERROR, "Error trying to write no$gba symbol file.");
                    }
                }
            }

            inStream.Close();
            outStream.Close();
            errorStream.Close();

            return success ? ExitSuccess : ExitFailure;
        }

        public static bool EaParse(string game, string rawsFolder, string rawsExtension, Stream inStream,
            string inFileName, Stream outStream, Log log, Dictionary<string, Definition> initialDefinitions)
        {
            var options = new EaOptions();

            var myInterpreter = new EaInterpreter(game, rawsFolder, rawsExtension, inStream, inFileName, outStream, log,
                options);

            myInterpreter.AddDefinitions(initialDefinitions);

            return myInterpreter.Interpret();
        }

        public static Definition CreateDefinition(string value)
        {
            var t = new Tokenizer();
            return new Definition(t.TokenizePhrase(value, "TMCR Rom", 1, 0, value.Length).ToList());
        }
    }
}
