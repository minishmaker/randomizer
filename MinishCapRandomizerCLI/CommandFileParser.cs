namespace MinishCapRandomizerCLI;

public static class CommandFileParser
{
    internal static void ParseCommandsFile(string filename)
    {
        var file = new StreamReader(new FileStream(filename, FileMode.Open));
        var exited = false;
        while (!file.EndOfStream && !exited)
        {
            var input = file.ReadLine();

            var inputs = input.Split(' ');

            switch (inputs[0])
            {
                case "LoadRom":
                    GenericCommands.LoadRom(inputs[1]);
                    break;
                case "ChangeSeed":
                    GenericCommands.Seed(inputs[1], inputs.Length > 2 ? inputs[2] : null);
                    break;
                case "LoadLogic":
                    GenericCommands.LoadLogic(inputs.Length > 1 ? inputs[1] : "");
                    break;
                case "LoadPatch":
                    GenericCommands.LoadPatch(inputs.Length > 1 ? inputs[1] : "");
                    break;
                case "LoadSettings":
                    GenericCommands.LoadSettings(inputs[1]);
                    break;
                case "Logging":
                    GenericCommands.Logging(inputs[1], inputs.Length > 2 ? inputs[2] : null, inputs.Length > 2 ? inputs[2] : null);
                    break;
                case "Randomize":
                    GenericCommands.Randomize(inputs[1]);
                    break;
                case "SaveRom":
                    GenericCommands.SaveRom(inputs[1]);
                    break;
                case "SaveSpoiler":
                    GenericCommands.SaveSpoiler(inputs[1]);
                    break;
                case "SavePatch":
                    GenericCommands.SavePatch(inputs[1]);
                    break;
                case "GetSettingString":
                    GenericCommands.GetSettingString();
                    break;
                case "Rem":
                    Console.WriteLine($"Commented out line {input}");
                    break;
                case "BulkGenerateSeeds":
                    if (!int.TryParse(inputs[1], out var numberOfSeedToGen))
                        throw new Exception("Provided value of bulk generated seeds is not a number!");

                    var num = 0;
                    while (num++ < numberOfSeedToGen)
                    {
                        GenericCommands.Seed("R");
                        var result = GenericCommands.Randomize("1");
                        if (!result) --num;
                        else
                            Console.WriteLine($"Generated seed {num}");
                    }
                    break;
                case "Exit":
                    exited = true;
                    break;
                default:
                    Console.WriteLine($"Warning: Invalid command or whitespace line {input} detected! Parsing will continue.");
                    break;
            }
        
        }

        if (!exited)
        {
            Console.WriteLine("Warning: End of command file reached but no call to Exit was read! This will exit as expected, but all command files should include an exit call at the end in the event this changes going forward.");
        }
    }
}
