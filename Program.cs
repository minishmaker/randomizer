using MinishRandomizer.Randomizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinishRandomizer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            

            bool showGui = true;
            bool randomize = false;
            bool outputRom = false;
            bool outputSpoiler = false;
            bool webOutput = false;
            bool patchOutput = false;
            string logicPath = null;
            string patchPath = null;
            string settingsString = null;
            string gimmicksString = null;
            int? seed = null;

            Console.WriteLine(args.Length);

            foreach (string argument in args)
            {
                string[] argParts = argument.Split(new char[] { ':' }, 1);

                switch (argParts[0])
                {
                    case "help":
                    case "--help":
                    case "-h":
                        
                        break;

                    case "--nogui":
                    case "-g":
                        showGui = false;
                        break;

                    case "--randomize":
                    case "-r":
                        randomize = true;

                        if (argParts.Length < 2)
                        {
                            outputRom = true;
                            outputSpoiler = false;
                        }
                        else
                        {
                            switch (argParts[1])
                            {
                                case "rom":
                                case "r":
                                    outputRom = true;
                                    break;

                                case "web":
                                case "w":
                                    webOutput = true;
                                    break;

                                case "patch":
                                case "p":
                                    patchOutput = true;
                                    break;

                                case "none":
                                case "n":
                                    break;

                                default:

                                    break;
                            }

                            if (argParts.Length > 2)
                            {
                                if (argParts[2] == "spoiler")
                                {
                                    outputSpoiler = true;
                                }
                            }
                        }
                        break;
                    case "--logic":
                    case "-l":
                        if (argParts.Length == 2)
                        {
                            logicPath = argParts[1];
                        }
                        break;

                    case "--patch":
                    case "-p":
                        if (argParts.Length == 2)
                        {
                            patchPath = argParts[1];
                        }
                        break;

                    case "--settings":
                    case "-S":
                        if (argParts.Length == 2)
                        {
                            settingsString = argParts[1];
                        }
                        break;

                    case "--gimmicks":
                    case "-k":
                        if (argParts.Length == 2)
                        {
                            gimmicksString = argParts[1];
                        }
                        break;

                    case "--seed":
                    case "-d":
                        if (argParts.Length >= 2)
                        {
                            if (int.TryParse(argParts[1], out int tempSeed))
                            {
                                seed = tempSeed;
                            }
                            else
                            {
                                Console.WriteLine($"{argParts[1]} is not a valid integer value!");
                                return;
                            }
                        }
                        break;
                }

                
            }

            if (webOutput || outputRom || outputSpoiler)
            {
                Shuffler shuffler = new Shuffler();

                if (seed != null)
                {
                    shuffler.SetSeed(seed ?? 0);
                }
                
                if (logicPath != null)
                {
                    shuffler.LoadOptions(logicPath);
                }
                else
                {
                    shuffler.LoadOptions();
                }

                if (settingsString != null)
                {
                    shuffler.LoadSettingsString(settingsString);
                }

                if (gimmicksString != null)
                {
                    shuffler.LoadGimmicksString(gimmicksString);
                }

                if (logicPath != null)
                {
                    shuffler.LoadLocations(logicPath);
                }
                else
                {
                    shuffler.LoadLocations();
                }

                shuffler.RandomizeLocations();
            }

            if (showGui)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
        }
    }
}
