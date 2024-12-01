using System.Drawing;
using System.Globalization;
using RandomizerCore.Controllers;
using RandomizerCore.Controllers.Models;
using RandomizerCore.Random;
using RandomizerCore.Randomizer.Logic.Options;
using SkiaSharp;

namespace MinishCapRandomizerCLI;

internal static class GenericCommands
{
    //Not Null
#pragma warning disable CS8618
    internal static ShufflerController ShufflerController;
    internal static YamlController YamlController;
    internal static ControllerBase? PreviouslyUsedController;
#pragma warning restore CS8618
    private static string? _cachedLogicPath;
    private static string? _cachedPatchPath;
    private static string? _cachedYAMLPathLogic;
    private static string? _cachedYAMLPathCosmetics;
    private static bool _cachedUseGlobalYAML = false;
    private static bool _strict = false;
    
    internal static void LoadRom(string? path = null)
    {
        Console.Write("Please enter the path to your Minish Cap Rom: ");
        try
        {
            var input = path ?? Console.ReadLine();
            if (input != null) ShufflerController.LoadRom(input);
            Console.WriteLine("ROM Loaded Successfully!");
        }
        catch
        {
            PrintError("Failed to load ROM! Please check your file path and make sure you have read access.");
        }
    }

    internal static void Seed(string? option = null, string? seedStr = null)
    {
        Console.Write("Would you like a random seed or a set seed? (R or S): ");
        var input = option ?? Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
            switch (input.ToLowerInvariant())
            {
                case "r":
                    var rand = new SquaresRandomNumberGenerator();
                    var rSeed = rand.Next();
                    ShufflerController.SetRandomizationSeed(rSeed);
                    YamlController.SetRandomizationSeed(rSeed);
                    Console.WriteLine($"Seed {rSeed} set successfully!");
                    break;
                case "s":
                    Console.Write("Please enter the seed you want to use: ");
                    var seedString = seedStr ?? Console.ReadLine();
                    
                    if (string.IsNullOrEmpty(seedString) || !ulong.TryParse(seedString, NumberStyles.HexNumber, default, out var seed))
                    {
                        PrintError("Invalid seed entered!");
                        break;
                    }
                    
                    ShufflerController.SetRandomizationSeed(seed);
                    YamlController.SetRandomizationSeed(seed);
                    Console.WriteLine("Seed set successfully!");
                    break;
                default:
                    if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        PrintError("Invalid Input!");
                    }
                    break;
            }
        }
        else PrintError("Invalid Input!");
    }

    internal static void LoadLogic(string? logicFile = null)
    {
        Console.Write("Please enter the path to the Logic File you want to use (leave empty to use default logic): ");
        try
        {
            _cachedLogicPath = logicFile ?? Console.ReadLine();
            ShufflerController.LoadLogicFile(_cachedLogicPath);
            YamlController.LoadLogicFile(_cachedLogicPath);
            Console.WriteLine("Logic file loaded successfully!");
        }
        catch
        {
            PrintError("Failed to load Logic File! Please check your file path and make sure you have read access.");
        }
    }

    internal static void LoadPatch(string? patchFile = null)
    {
        Console.Write("Please enter the path to the Patch File you want to use (leave empty to use default patch): ");
        _cachedPatchPath = patchFile ?? Console.ReadLine();
        Console.WriteLine("Patch file loaded successfully!");
    }

    internal static void ClearYAMLConfig(string? option = null)
    {
        Console.Write("Are you sure you want to clear YAML config? This action cannot be undone! (Y or N): ");
        var input = option ?? Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
            switch (input.ToLowerInvariant())
            {
                case "n": 
                    return;
                case "y":
                    _cachedUseGlobalYAML = false;
                    _cachedYAMLPathCosmetics = null;
                    _cachedYAMLPathLogic = null;
                    ShufflerController.LoadLogicFile(_cachedLogicPath ?? "");
                    Console.WriteLine("YAML config cleared successfully!");
                    return;
            }
        }
        else PrintError("Invalid Input!");
    }

    internal static void UseYAML(string? globalYAMLMode = null, string? yamlFileLogic = null, string? yamlFileCosmetics = null)
    {
        Console.WriteLine("Do you want to use a single YAML file (if any) for all options or should Logic and Cosmetic options be handled separately?");
        Console.WriteLine("1) All settings handled the same way");
        Console.WriteLine("2) Separate Logic and Cosmetic settings");
        Console.Write("Enter the number corresponding to the mode you want to use: ");
        var input = globalYAMLMode ?? Console.ReadLine();
        if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var number) || number < 1 || number > 2)
        {
            Console.WriteLine("Invalid Input!");
            return;
        }
        _cachedUseGlobalYAML = number == 1;
        if (_cachedUseGlobalYAML)
        {
            Console.Write("Please enter the path to the YAML File you want to use (leave empty to use selected settings instead of YAML): ");
            _cachedYAMLPathLogic = yamlFileLogic ?? Console.ReadLine();
            _cachedYAMLPathCosmetics = _cachedYAMLPathLogic;
            Console.WriteLine(string.IsNullOrEmpty(_cachedYAMLPathLogic) ? "Using selected settings instead of YAML!" : "YAML file loaded successfully!");
        }
        else
        {
            Console.Write("Please enter the path to the YAML File you want to use for Logic Options (leave empty to use selected settings instead of YAML): ");
            _cachedYAMLPathLogic = yamlFileLogic ?? Console.ReadLine();
            Console.WriteLine(string.IsNullOrEmpty(_cachedYAMLPathLogic) ? "Using selected settings instead of YAML!" : "YAML file loaded successfully!");
            Console.Write("Please enter the path to the YAML File you want to use for Cosmetic Options (leave empty to use selected settings instead of YAML): ");
            _cachedYAMLPathCosmetics = yamlFileCosmetics ?? Console.ReadLine();
            Console.WriteLine(string.IsNullOrEmpty(_cachedYAMLPathCosmetics) ? "Using selected settings instead of YAML!" : "YAML file loaded successfully!");
        }
    }

    internal static void LoadYAML(string? yamlFile = null, string? optionTypes = null)
    {
        Console.Write("Please enter the path to the YAML File to load options from: ");
        var filepath = yamlFile ?? Console.ReadLine();
        if (string.IsNullOrEmpty(filepath))
        {
            Console.WriteLine("Invalid Input!");
            return;
        }
        Console.WriteLine("0) Nothing");
        Console.WriteLine("1) Only Logic Settings");
        Console.WriteLine("2) Only Cosmetic Setting");
        Console.WriteLine("3) All Settings");
        Console.Write("Enter the number corresponding to the setting type(s) you want to load: ");
        var input = optionTypes ?? Console.ReadLine();
        if (string.IsNullOrEmpty(input) || !uint.TryParse(input, out var number) || number > 3)
        {
            Console.WriteLine("Invalid Input!");
            return;
        }

        //TODO: Make a more elegant solution for this
        switch (number)
        {
            case 1:
                YamlController.LoadLogicSettingsFromYaml(filepath);
                ShufflerController.LoadSettingsFromSettingString(YamlController.GetSelectedSettingsString());
                break;
            case 2:
                YamlController.LoadCosmeticsFromYaml(filepath);
                ShufflerController.LoadCosmeticsFromCosmeticsString(YamlController.GetSelectedCosmeticsString());
                break;
            case 3:
                YamlController.LoadAllSettingsFromYaml(filepath);
                ShufflerController.LoadSettingsFromSettingString(YamlController.GetSelectedSettingsString());
                ShufflerController.LoadCosmeticsFromCosmeticsString(YamlController.GetSelectedCosmeticsString());
                break;
        }
        
        Console.WriteLine("Settings loaded successfully!");
    }

    internal static void LoadSettings(string? settings = null)
    {
        Console.Write("Please enter the setting string to load: ");
        var input = settings ?? Console.ReadLine();
        if (!string.IsNullOrEmpty(input)) ShufflerController.LoadSettingsFromSettingString(input);
        Console.WriteLine("Settings loaded successfully!");
    }

    // This option is not supported for use by command files, use the settings string option instead
    internal static void Options()
    {
        Console.WriteLine("Options for current logic file:");
        var options = ShufflerController.GetSelectedOptions();
        for (var i = 0; i < options.Count; )
        {
            var option = options[i];
            Console.WriteLine($"{++i}) Type: {option.GetOptionUiType()}, Option Name: {option.NiceName}, Setting Type: {option.Type}, Value: {GetOptionValue(option)}");
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
                        Console.WriteLine($"{++i}) Type: {option.GetOptionUiType()}, Option Name: {option.NiceName}, Setting Type: {option.Type}, Value: {GetOptionValue(option)}");
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
                        PrintError("Number ouf of range!");
                    }
                }
                else
                {
                    if (options.Exists(option => string.Equals(option.Name, input)))
                    {
                        var option = options.Find(option => string.Equals(option.Name, input));
                        if (option != null)
                        {
                            EditOption(option);
                        }
                        else
                        {
                            PrintError($"Unknown Option {input}!");
                        }
                    }
                    else
                    {
                        PrintError($"Unknown Option {input}!");
                    }
                }
            }
            else
            {
                PrintError("Invalid Input!");
            }
            Console.Write("Please enter the number of the setting you would like to change, enter \"Exit\" to stop editing, or enter \"List\" to list all of the options again: ");
            input = Console.ReadLine();
        }
    }

    internal static void Logging(string? option = null, string? verbosityOption = null, string? logPath = null)
    {
        Console.WriteLine("1) Logger verbosity");
        Console.WriteLine("2) Logger output file");
        Console.WriteLine("3) Force publish log transactions using current verbosity and output file");
        Console.Write("Enter the number of the logger option you want to change: ");
        var input = option ?? Console.ReadLine();
        if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var i))
        {
            PrintError("Invalid Input!");
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
                input = verbosityOption ?? Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out i) || (i != 1 && i != 2))
                {
                    PrintError("Invalid Input!");
                    return;
                }
                ShufflerController.SetLoggerVerbosity(i == 1);
                Console.WriteLine("Logger updated successfully!");
                break;
            }
            case 2:
            {
                Console.Write("Please enter the path to save logs after randomization: ");
                input = logPath ?? Console.ReadLine();
                if (input != null) ShufflerController.SetLogOutputPath(input);
                Console.WriteLine("Logger updated successfully!");
                break;
            }
            case 3:
                Console.WriteLine(ShufflerController.PublishLogs());
                break;
            default:
                PrintError("Invalid Input!");
                return;
        }
    }

    internal static bool Randomize(string? randomizationAttempts = null)
    {
        Console.Write("How many times would you like the randomizer to attempt to generate a new seed if randomization fails? ");
        var attemptsStr = randomizationAttempts ?? Console.ReadLine();
        if (!string.IsNullOrEmpty(attemptsStr) || !int.TryParse(attemptsStr, out var attempts) || attempts <= 0) attempts = 1;

        PreviouslyUsedController = (_cachedYAMLPathLogic != null || _cachedYAMLPathCosmetics != null || _cachedUseGlobalYAML) ? YamlController : ShufflerController;

        //Even if all values are provided, the base shuffler controller ignores 3 of them whereas yaml doesn't so this works
        var result = PreviouslyUsedController.LoadLocations(_cachedLogicPath, _cachedYAMLPathLogic, _cachedYAMLPathCosmetics, _cachedUseGlobalYAML);
        if (result)
        {
            result = PreviouslyUsedController.Randomize(attempts);

            if (result.WasSuccessful)
            {
                Console.WriteLine("Randomization successful!");
                return true;
            }
        }

        PreviouslyUsedController = null;
        PrintError($"Randomization failed! Error: {result.ErrorMessage}");
        return false;
    }

    internal static void SaveRom(string? output = null)
    {
        if (!ValidatePreviouslyUsedController()) return;

        Console.Write("Please enter the path to save the ROM (blank for default): ");
        try
        {
            var input = output ?? Console.ReadLine();
            PreviouslyUsedController!.SaveAndPatchRom(string.IsNullOrEmpty(input) ? $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MinishRandomizer-ROM.gba" : input);
            Console.WriteLine("Rom saved successfully!");
        }
        catch
        {
            PrintError("Failed to save ROM! Please check your file path and make sure you have write access.");
        }
    }

    internal static void SaveSpoiler(string? output = null)
    {
        if (!ValidatePreviouslyUsedController()) return;

        Console.Write("Please enter the path to save the spoiler (blank for default): ");
        try
        {
            var input = output ?? Console.ReadLine();
            PreviouslyUsedController!.SaveSpoiler(string.IsNullOrEmpty(input) ? $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MinishRandomizer-Spoiler.txt" : input);
            Console.WriteLine("Spoiler saved successfully!");
        }
        catch
        {
            PrintError("Failed to save spoiler! Please check your file path and make sure you have write access.");
        }
    }

    internal static void SavePatch(string? output = null)
    {
        if (!ValidatePreviouslyUsedController()) return;

        Console.Write("Please enter the path to save the patch (blank for default): ");
        try
        {
            var input = output ?? Console.ReadLine();
            PreviouslyUsedController!.CreatePatch(string.IsNullOrEmpty(input) ? $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MinishRandomizer-Patch.bps" : input, _cachedPatchPath);
            Console.WriteLine("Patch saved successfully!");
        }
        catch
        {
            PrintError("Failed to save patch! Please check your file path and make sure you have write access.");
        }
    }

    internal static void SaveSeedHash()
    {
        if (!ValidatePreviouslyUsedController()) return;

        Console.Write("Please enter the path to save the hash (blank for default, .png will be appended to the filename): ");
        try
        {
            var input = Console.ReadLine();
            var eventDefines = PreviouslyUsedController!.GetEventWrites().Split('\n');
            using var image = ImageHandler.GetHashImage(eventDefines);
            using var stream = new MemoryStream();
            image.Encode(stream, SKEncodedImageFormat.Png, 100);
            File.WriteAllBytes(string.IsNullOrEmpty(input) ? $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MinishRandomizer-Hash.png" : $"{input}.png", stream.ToArray());
            Console.WriteLine("Hash image saved successfully!");
        }
        catch
        {
            PrintError("Failed to save hash image! Please check your file path and make sure you have write access.");
        }
    }

    internal static void GetSelectedSettingString()
    {
        if (!ValidatePreviouslyUsedController()) return;

        Console.WriteLine("Setting String for selected settings:");
        Console.WriteLine(PreviouslyUsedController!.GetSelectedSettingsString());
    }

    internal static void GetFinalSettingString()
    {
        if (!ValidatePreviouslyUsedController()) return;

        Console.WriteLine("Setting String for settings used for seed generation:");
        Console.WriteLine(PreviouslyUsedController!.GetFinalSettingsString());
    }

    internal static void PatchRom()
    {
        if (!ValidatePreviouslyUsedController()) return;

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
            var result = PreviouslyUsedController!.SaveRomPatch(patch, rom);

            if (result)
            {
                Console.WriteLine("Rom patched successfully!");
            }
            else
            {
                PrintError("Failed to patch ROM! Please check your file paths and make sure you have read/write access. "+result.ErrorMessage);
            }
        }
    }

    internal static ShufflerControllerResult PatchRomWithoutSaving()
    {
        if (!ValidatePreviouslyUsedController()) return new ShufflerControllerResult { WasSuccessful = false };

        PreviouslyUsedController!.CreatePatch(out var result);
        return result;
    }

    internal static void CreatePatch()
    {
        if (!ValidatePreviouslyUsedController()) return;

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
            var result = PreviouslyUsedController!.SaveRomPatch(patch, rom);

            if(result)
            {
                Console.WriteLine("Patch saved successfully!");
            }
            else
            {
                PrintError("Failed to save patch! Please check your file paths and make sure you have read/write access. "+result.ErrorMessage);
            }
        }
    }

    internal static void Strict()
    {
        _strict = !_strict;
        Console.WriteLine($"Toggled strict mode {(_strict ? "on" : "off")}");
    }

    internal static string GetOptionValue(LogicOptionBase option)
    {
        switch (option)
        {
            case LogicFlag:
            {
                return option.Active.ToString();
            }
            case LogicDropdown dropdown:
            {
                return dropdown.Selection;
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

    internal static void EditOption(LogicOptionBase option)
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
                    if (!input!.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        PrintError("Invalid Input!");
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
                    if (!input!.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        PrintError("Invalid Input!");
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
                Console.WriteLine("4) Pick Random Color");
                Console.WriteLine("5) Enter ARGB Color Code");
                Console.Write("Enter the number of the option you want for the color picker: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var i) || i is < 1 or > 5)
                {
                    if (!input!.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        PrintError("Invalid Input!");
                    }
                    break;
                }

                switch (i)
                {
                    case 1:
                        colorPicker.Active = false;
                        colorPicker.UseRandomColor = false;
                        Console.WriteLine("Color set successfully!");
                        break;
                    case 2:
                        colorPicker.Active = true;
                        colorPicker.UseRandomColor = false;
                        colorPicker.DefinedColor = colorPicker.BaseColor;
                        Console.WriteLine("Color set successfully!");
                        break;
                    case 3:
                        colorPicker.Active = true;
                        colorPicker.UseRandomColor = true;
                        colorPicker.PickRandomColor();
                        Console.WriteLine("Color set successfully!");
                        break;
                    case 4:
                        colorPicker.Active = true;
                        colorPicker.UseRandomColor = false;
                        colorPicker.PickRandomColor();
                        Console.WriteLine("Color set successfully!");
                        break;
                    case 5:
                        Console.Write("Please enter the ARGB color string you wish to use: ");
                        var argb = Console.ReadLine();
                        if (string.IsNullOrEmpty(argb) || !int.TryParse(argb, out var color))
                        {
                            if (!argb!.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            {
                                PrintError("Invalid Input!");
                            }
                            break;
                        }

                        colorPicker.Active = true;
                        colorPicker.UseRandomColor = false;
                        colorPicker.DefinedColor = Color.FromArgb(color);
                        Console.WriteLine("Color set successfully!");
                        break;
                }
                break;
            }
            case LogicNumberBox box:
            {
                Console.Write($"Please enter a number from {box.MinValue} to {box.MaxValue} for {box.NiceName}: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var i) || i < box.MinValue || i > box.MaxValue)
                {
                    if (!input!.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        PrintError("Invalid Input!");
                    }
                    break;
                }

                box.Value = input;
                Console.WriteLine("Number box value set successfully!");
                break;
            }
        }
    }

    internal static void PrintError(string msg)
    {
        Console.Error.WriteLine(msg);
        if(_strict)
        {
            Environment.Exit(1);
        }
    }

    private static bool ValidatePreviouslyUsedController()
    {
        if (PreviouslyUsedController == null)
        {
            Console.WriteLine("Randomize has not yet been called or the previous call failed! Please call randomize before running this command.");
            return false;
        }

        return true;
    }
}
