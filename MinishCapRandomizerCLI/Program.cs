using System.Drawing;
using RandomizerCore.Controllers;
using RandomizerCore.Controllers.Models;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerCLI;

internal class Program
{
    //Not Null
    private static Dictionary<string, Action> _commands;
    //Not Null
    private static ShufflerController _shufflerController;
    private static string? _cachedLogicPath;
    private static string? _cachedPatchPath;
    private static bool _exiting = false;

    private static void Main(string[] args)
    {
        _shufflerController = new ShufflerController();
        Console.Write("Loading default logic file...");
        _shufflerController.LoadLogicFile();
        Console.WriteLine("done");
        LoadCommands();
        InputLoop();
    }

    private static void InputLoop()
    {
        PrintCommands();
        while (!_exiting)
        {
            Console.Write("Waiting for command, type \"Help\" to see the list of available commands: ");
            var command = Console.ReadLine();
            if (string.IsNullOrEmpty(command) || !_commands.ContainsKey(command))
            {
                Console.WriteLine($"Invalid command entered! {command} is not a valid command!");
                continue;
            }
            
            _commands[command].Invoke();
        }
    }

    private static void LoadCommands()
    {
        _commands = new Dictionary<string, Action>
        {
            {"Help", PrintCommands},
            {"LoadRom", LoadRom},
            {"ChangeSeed", Seed},
            {"LoadLogic", LoadLogic},
            {"LoadPatch", LoadPatch},
            {"LoadSettings", LoadSettings},
            {"Options", Options},
            {"Logging", Logging},
            {"Randomize", Randomize},
            {"SaveRom", SaveRom},
            {"SaveSpoiler", SaveSpoiler},
            {"SavePatch", SavePatch},
            {"GetSettingString", GetSettingString},
            {"PatchRom", PatchRom},
            {"CreatePatch", CreatePatch},
            {"Exit", Exit},
        };
    }

    private static void PrintCommands()
    {
        Console.WriteLine(@"Available Commands, note that commands are case-sensitive!
LoadRom             Load a European Minish Cap ROM
ChangeSeed          Change randomization seed
LoadLogic           Load custom logic file
LoadPatch           Load custom patch file
LoadSettings        Load a setting string
Options             Display options, allows editing of option values
Logging             Allows you to change logger settings
Randomize           Generates a randomized ROM
SaveRom             Saves and patches the ROM, requires Randomize to have been called
SaveSpoiler         Saves the spoiler log, requires Randomize to have been called
SavePatch           Saves a BPS patch for the randomized ROM, requires Randomize to have been called
GetSettingString    Gets the setting string for your currently selected settings
PatchRom            Patches a European Minish Cap ROM with a BPS patch
CreatePatch         Creates a patch from a patched ROM and an unpatched European Minish Cap ROM
Exit                Exits the program
");
    }

    private static void LoadRom()
    {
        Console.Write("Please enter the path to your Minish Cap Rom: ");
        try
        {
            var input = Console.ReadLine();
            if (input != null) _shufflerController.LoadRom(input);
            Console.WriteLine("ROM Loaded Successfully!");
        }
        catch
        {
            Console.WriteLine("Failed to load ROM! Please check your file path and make sure you have read access.");
        }
    }

    private static void Seed()
    {
        Console.Write("Would you like a random seed or a set seed? (R or S): ");
        var input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
            switch (input.ToLowerInvariant())
            {
                case "r":
                    var rand = new Random();
                    _shufflerController.SetRandomizationSeed(rand.Next());
                    Console.WriteLine("Seed set successfully!");
                    break;
                case "s":
                    Console.Write("Please enter the seed you want to use: ");
                    var seedString = Console.ReadLine();
                    
                    if (string.IsNullOrEmpty(seedString) || !int.TryParse(seedString, out var seed))
                    {
                        Console.WriteLine("Invalid seed entered!");
                        break;
                    }
                    
                    _shufflerController.SetRandomizationSeed(seed);
                    Console.WriteLine("Seed set successfully!");
                    break;
                default:
                    if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Invalid Input!");
                    }
                    break;
            }
        }
        else Console.WriteLine("Invalid Input!");
    }

    private static void LoadLogic()
    {
        Console.Write("Please enter the path to the Logic File you want to use (leave empty to use default logic): ");
        try
        {
            _cachedLogicPath = Console.ReadLine();
            _shufflerController.LoadLogicFile(_cachedLogicPath);
            Console.WriteLine("Logic file loaded successfully!");
        }
        catch
        {
            Console.WriteLine("Failed to load Logic File! Please check your file path and make sure you have read access.");
        }
    }

    private static void LoadPatch()
    {        
        Console.Write("Please enter the path to the Patch File you want to use (leave empty to use default patch): ");
        _cachedPatchPath = Console.ReadLine();
        Console.WriteLine("Patch file loaded successfully!");
    }

    private static void LoadSettings()
    {
        Console.Write("Please enter the setting string to load: ");
        var input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input)) _shufflerController.LoadSettingsFromSettingString(input);
        Console.WriteLine("Settings loaded successfully!");
    }

    private static void Options()
    {
        Console.WriteLine("Options for current logic file:");
        var options = _shufflerController.GetLogicOptions();
        for (var i = 0; i < options.Count; )
        {
            var option = options[i];
            Console.WriteLine($"{++i}) Type: {option.GetOptionUIType()}, Option Name: {option.NiceName}, Setting Type: {option.Type}, Value: {GetOptionValue(option)}");
        }

        Console.Write("Please enter the number of the setting you would like to change, enter \"Exit\" to stop editing, or enter \"List\" to list all of the options again: ");
        var input = Console.ReadLine();
        
        while (string.IsNullOrEmpty(input) || !input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Equals("list", StringComparison.OrdinalIgnoreCase))
                {
                    for (var i = 0; i < options.Count; )
                    {
                        var option = options[i];
                        Console.WriteLine($"{++i}) Type: {option.GetOptionUIType()}, Option Name: {option.NiceName}, Setting Type: {option.Type}, Value: {GetOptionValue(option)}");
                    }
                }
                else if (int.TryParse(input, out var num))
                {
                    if (--num >= 0 && num < options.Count)
                    {
                        EditOption(options[num]);
                    }
                    else
                    {
                        Console.WriteLine("Number ouf of range!");
                    }
                }
                else
                {
                    if (options.Exists(option => String.Equals(option.Name, input)))
                    {
                        var option = options.Find(option => String.Equals(option.Name, input));
                        if (option != null)
                        {
                            EditOption(option);
                        }
                        else
                        {
                            Console.WriteLine($"Unknown Option {input}!");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Unknown Option {input}!");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Input!");
            }
            Console.Write("Please enter the number of the setting you would like to change, enter \"Exit\" to stop editing, or enter \"List\" to list all of the options again: ");
            input = Console.ReadLine();
        }
    }

    private static void Logging()
    {
        Console.WriteLine("1) Logger verbosity");
        Console.WriteLine("2) Logger output file");
        Console.WriteLine("3) Force publish log transactions using current verbosity and output file");
        Console.Write("Enter the number of the logger option you want to change: ");
        var input = Console.ReadLine();
        if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var i))
        {
            Console.WriteLine("Invalid Input!");
            return;
        }

        switch (i)
        {
            case 1:
            {
                Console.WriteLine("Note: One logger transaction has many logs: info logs, warning logs, error logs, and exception logs");
                Console.WriteLine("1) Publish all logger transactions");
                Console.WriteLine("2) Publish only transactions that contain errors or warnings");
                Console.Write("Enter the number for your desired verbosity: ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out i) || (i != 1 && i != 2))
                {
                    Console.WriteLine("Invalid Input!");
                    return;
                }
                _shufflerController.SetLoggerVerbosity(i == 1);
                Console.WriteLine("Logger updated successfully!");
                break;
            }
            case 2:
            {
                Console.Write("Please enter the path to save logs after randomization: ");
                input = Console.ReadLine();
                if (input != null) _shufflerController.SetLogOutputPath(input);
                Console.WriteLine("Logger updated successfully!");
                break;
            }
            case 3:
                Console.WriteLine(_shufflerController.PublishLogs());
                break;
            default:
                Console.WriteLine("Invalid Input!");
                return;
        }
    }

    private static void Randomize()
    {
        Console.Write("How many times would you like the randomizer to attempt to generate a new seed if randomization fails? ");
        var attemptsStr = Console.ReadLine();
        if (!string.IsNullOrEmpty(attemptsStr) || !int.TryParse(attemptsStr, out var attempts) || attempts <= 0) attempts = 1;

        _shufflerController.LoadLocations(_cachedLogicPath);
        ShufflerControllerResult result = _shufflerController.Randomize(attempts);
        Console.WriteLine(result ? "Randomization successful!" : "Randomization failed! "+result.ErrorMessage);
    }

    private static void SaveRom()
    {        
        Console.Write("Please enter the path to save the ROM (blank for default): ");
        try
        {
            var input = Console.ReadLine();
            _shufflerController.SaveAndPatchRom(string.IsNullOrEmpty(input) ? $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MinishRandomizer-ROM.gba" : input);
            Console.WriteLine("Rom saved successfully!");
        }
        catch
        {
            Console.WriteLine("Failed to save ROM! Please check your file path and make sure you have write access.");
        }
    }

    private static void SaveSpoiler()
    {        
        Console.Write("Please enter the path to save the spoiler (blank for default): ");
        try
        {
            var input = Console.ReadLine();
            _shufflerController.SaveSpoiler(string.IsNullOrEmpty(input) ? $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MinishRandomizer-Spoiler.txt" : input);
            Console.WriteLine("Spoiler saved successfully!");
        }
        catch
        {
            Console.WriteLine("Failed to save spoiler! Please check your file path and make sure you have write access.");
        }
    }

    private static void SavePatch()
    {
        Console.Write("Please enter the path to save the patch (blank for default): ");
        try
        {
            var input = Console.ReadLine();
            _shufflerController.CreatePatch(string.IsNullOrEmpty(input) ? $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MinishRandomizer-Patch.bps" : input, _cachedPatchPath);
            Console.WriteLine("Patch saved successfully!");
        }
        catch
        {
            Console.WriteLine("Failed to save patch! Please check your file path and make sure you have write access.");
        }
    }

    private static void GetSettingString()
    {
        Console.WriteLine("Setting String:");
        Console.WriteLine(_shufflerController.GetSettingsString());
    }

    private static void PatchRom()
    {

        Console.Write("Please enter the path of the rom patch: ");
        var patch = Console.ReadLine();
        
        Console.Write("Please enter the path to save the ROM to: ");
        var rom = Console.ReadLine();

        if (rom == null || patch == null)
        {
            Console.WriteLine("Failed to save patch! Please check your file paths and make sure you have read/write access.");
        }
        else
        {
            var result = _shufflerController.SaveRomPatch(patch, rom);

            Console.WriteLine(!result
                ? "Failed to patch ROM! "+result.ErrorMessage
                : "ROM patched successfully!");
        }
    }

    private static void CreatePatch()
    {
        Console.Write("Please enter the path of the patched rom: ");
        var rom = Console.ReadLine();
            
        Console.Write("Please enter the path to save the patch to: ");
        var patch = Console.ReadLine();

        if (rom == null || patch == null)
        {
            Console.WriteLine("Failed to save patch! Please check your file paths and make sure you have read/write access.");
        }
        else
        {
            var result = _shufflerController.SaveRomPatch(patch, rom);

            Console.WriteLine(!result
                ? "Failed to save patch! "+result.ErrorMessage
                : "Patch saved successfully!");
        }
    }

    private static void Exit()
    {
        _exiting = true;
    }

    private static string GetOptionValue(LogicOptionBase option)
    {
        switch (option)
        {
            case LogicFlag:
            {
                return option.Active.ToString();
            }
            case LogicDropdown dropdown:
            {
                return dropdown.Selection.ToString();
            }
            case LogicColorPicker colorPicker:
            {
                return colorPicker.Active ? colorPicker.DefinedColor.ToString() : "Vanilla";
            }
            case LogicNumberBox box:
            {
                return box.Value;
            }
        }
        return "";
    }

    private static void EditOption(LogicOptionBase option)
    {
        switch (option)
        {
            case LogicFlag:
            {
                Console.WriteLine("1) Enabled");
                Console.WriteLine("2) Disabled");
                Console.Write("Enter the number of the option to set the flag to: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var i) || (i != 0 && i != 1 && i != 2))
                {
                    if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Invalid Input!");
                    }
                    break;
                }

                option.Active = i == 1;
                Console.WriteLine("Flag set successfully!");
                break;
            }
            case LogicDropdown dropdown:
            {
                var keys = dropdown.Selections.Keys.ToList();
                for (var i = 0; i < keys.Count; )
                {
                    var selection = keys[i];
                    Console.WriteLine($"{++i}) {selection}");
                }
                
                Console.Write("Enter the number of the option you want for the dropdown: ");
                var input = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var o) || o < 1 || o > keys.Count)
                {
                    if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Invalid Input!");
                    }
                    break;
                }

                dropdown.Selection = keys[o - 1];
                Console.WriteLine("Dropdown option set successfully!");
                break;
            }
            case LogicColorPicker colorPicker:
            {
                Console.WriteLine("1) Use Vanilla Color");
                Console.WriteLine("2) Use Default Color");
                Console.WriteLine("3) Use Random Color");
                Console.WriteLine("4) Enter ARGB Color Code");
                Console.Write("Enter the number of the option you want for the color picker: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var i) || i is < 1 or > 4)
                {
                    if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Invalid Input!");
                    }
                    break;
                }

                switch (i)
                {
                    case 1:
                        colorPicker.Active = false;
                        Console.WriteLine("Color set successfully!");
                        break;
                    case 2:
                        colorPicker.Active = true;
                        colorPicker.DefinedColor = colorPicker.BaseColor;
                        Console.WriteLine("Color set successfully!");
                        break;
                    case 3:
                        colorPicker.Active = true;
                        colorPicker.PickRandomColor();
                        Console.WriteLine("Color set successfully!");
                        break;
                    case 4:
                        Console.Write("Please enter the ARGB color string you wish to use: ");
                        var argb = Console.ReadLine();
                        if (string.IsNullOrEmpty(argb) || !int.TryParse(argb, out var color))
                        {
                            if (!argb.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Invalid Input!");
                            }
                            break;
                        }

                        colorPicker.Active = true;
                        colorPicker.DefinedColor = Color.FromArgb(color);
                        Console.WriteLine("Color set successfully!");
                        break;
                }
                break;
            }
            case LogicNumberBox box:
            {
                Console.Write($"Please enter a number from 0 to 255 for {box.NiceName}: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var i) || i is < 0 or > 255)
                {
                    if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Invalid Input!");
                    }
                    break;
                }

                box.Value = input;
                Console.WriteLine("Number box value set successfully!");
                break;
            }
        }
    }
}
