using System.Text;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerCLI;

public static class CommandFileParser
{
    internal static void ParseCommandsFile(string filename)
    {
        var file = new StreamReader(new FileStream(filename, FileMode.Open));
        var exited = false;
        var logBuilder = new StringBuilder();
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
                case "ClearYAML":
                    GenericCommands.ClearYAMLConfig("Y");
                    break;
                case "UseYAML":
                    GenericCommands.UseYAML(inputs.Length > 1 ? inputs[1] : null, inputs.Length > 2 ? inputs[2] : null, inputs.Length > 3 ? inputs[3] : null);
                    break;
                case "LoadYAML":
                    GenericCommands.LoadYAML(inputs.Length > 1 ? inputs[1] : null, inputs.Length > 2 ? inputs[2] : null);
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
                    GenericCommands.GetSelectedSettingString();
                    break;
                case "GetFinalSettingString":
                    GenericCommands.GetFinalSettingString();
                    break;
                case "Rem":
                    Console.WriteLine($"Commented out line {input}");
                    break;
                case "BulkGenerateSeeds":
                    if (!int.TryParse(inputs[1], out var numberOfSeedToGen))
                        throw new Exception("Provided value of bulk generated seeds is not a number!");
                    
                    if (!bool.TryParse(inputs[2], out var shuffleSettingsEachAttempt))
                        throw new Exception("2nd parameter must be true or false!");

                    var successes = 0;
                    var failures = 0;
                    var totalSeeds = 0;
                    var lastRunFailure = false;
                    var consecutiveFailures = 0;
                    var startTime = DateTime.Now;
                    while (successes++ < numberOfSeedToGen)
                    {
                        totalSeeds++;

                        GenericCommands.Seed("R");
                        
                        if (shuffleSettingsEachAttempt && !lastRunFailure) ShuffleAllOptions();

                        var time = DateTime.Now;
                        var result = GenericCommands.Randomize("1");
                        if (!result)
                        {
                            --successes;
                            ++failures;
                            lastRunFailure = true;
                            if (++consecutiveFailures < 10) continue;

                            logBuilder.AppendLine($"Failed to generate with settings {GenericCommands.ShufflerController.GetSelectedSettingsString()}");
                            lastRunFailure = false;
                            consecutiveFailures = 0;
                        }
                        else
                        {
                            var res = GenericCommands.PatchRomWithoutSaving();
                            if (!res)
                            {
                                logBuilder.AppendLine($"ROM with settings {GenericCommands.ShufflerController.GetSelectedSettingsString()} failed to patch! {res.ErrorMessage}");
                                lastRunFailure = false;
                                consecutiveFailures = 0;
                            }
                            else
                            {
                                var diff = DateTime.Now - time;
                                logBuilder.AppendLine($"Generated seed {successes} taking {diff.Seconds}.{diff.Milliseconds} seconds");
                                consecutiveFailures = 0;
                                lastRunFailure = false;
                            }
                        }
                    }
                    logBuilder.AppendLine($"Total Time Taken: {(DateTime.Now - startTime).Hours.ToString("00")}:{(DateTime.Now - startTime).Minutes.ToString("00")}:{(DateTime.Now - startTime).Seconds.ToString("00")}.{(DateTime.Now - startTime).Milliseconds}");
                    logBuilder.AppendLine($"Average time to gen a seed: {((DateTime.Now - startTime).TotalSeconds / (double)totalSeeds).ToString("0.00")} seconds");
                    logBuilder.AppendLine($"Total Success Rate: {(--successes / (double)totalSeeds) * 100}%");
                    break;
                case "Exit":
                    exited = true;
                    break;
                default:
                    Console.WriteLine($"Warning: Invalid command or whitespace line {input} detected! Parsing will continue.");
                    break;
            }
            File.WriteAllText($"{Directory.GetCurrentDirectory()}/CLIBulkGenOutput.txt", logBuilder.ToString());
        }

        if (!exited)
        {
            Console.WriteLine("Warning: End of command file reached but no call to Exit was read! This will exit as expected, but all command files should include an exit call at the end in the event this changes going forward.");
        }
    }

    private static void ShuffleAllOptions()
    {
        var options = GenericCommands.ShufflerController.GetSelectedOptions();

        var rand = new Random();

        foreach (var option in options)
        {
            switch (option)
            {
                case LogicFlag lf:
                    lf.Active = rand.Next() % 2 == 0;
                    break;
                case LogicDropdown ld:
                    ld.Selection = ld.Selections.Keys.ToList()[rand.Next() % ld.Selections.Keys.Count];
                    break;
                // case LogicNumberBox lnb:
                //     lnb.Value = $"{rand.Next(lnb.MinValue, lnb.MaxValue + 1)}";
                //     break;
                default:
                    break;
            }
        }
    }
}
