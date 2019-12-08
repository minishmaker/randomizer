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
            bool generateRom = false;
            bool outputSpoiler = false;
            bool webOutput = false;
            string logicPath;
            string patchPath;
            string settingsString;
            string gimmicksString;
            int? seed = null;

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

                    case "--web":
                    case "-w":
                        webOutput = true;
                        break;

                    case "--romgenerate":
                    case "-r":
                        generateRom = true;
                        break;

                    case "--spoiler":
                    case "-s":
                        outputSpoiler = true;
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
                        if (argParts.Length == 2)
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

            if (webOutput || generateRom || outputSpoiler)
            {
                Shuffler shuffler = new Shuffler();
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
