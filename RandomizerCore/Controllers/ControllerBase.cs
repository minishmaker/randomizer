using System.Reflection;
using RandomizerCore.Core;
using RandomizerCore.Random;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Helpers;
using RandomizerCore.Randomizer.Logic.Options;
using RandomizerCore.Randomizer.Shuffler;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Models;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Controllers.Models;

public abstract class ControllerBase
{
    #if DEBUG
        public string AppName => "Minish Cap Randomizer Debug Build";
    #else
	    public string AppName => "Minish Cap Randomizer";
    #endif

    public string VersionName => VersionIdentifier;
    public string RevName => RevisionIdentifier;

    public static string VersionIdentifier => "v1.0.0";
    public static string RevisionIdentifier => "RC 1";

    public void SetLogOutputPath(string logFilePath)
    {
        Logger.Instance.OutputFilePath = logFilePath;
    }

    public void SetLoggerVerbosity(bool useVerboseLogger)
    {
        Logger.Instance.UseVerboseLogger = useVerboseLogger;
    }

    public string PublishLogs()
    {
        var published = Logger.Instance.PublishLogs();
        return published
            ? $"Success! Published logs to {Logger.Instance.OutputFilePath}"
            : "Log output failed! Please check your file path and make sure you have write access.";
    }

    public ShufflerControllerResult LoadSettingsFromSettingString(string settingString)
    {
        try
        {
            MinifiedSettings.GenerateSettingsFromBase64String(settingString, Shuffler.GetSelectedOptions().OnlyLogic().GetSorted(),
                Shuffler.GetSelectedOptions().OnlyLogic().GetCrc32());

            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            Logger.Instance.SaveLogTransaction();
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
    }
    
    public ShufflerControllerResult LoadCosmeticsFromCosmeticsString(string settingString)
    {
        try
        {
            MinifiedSettings.GenerateSettingsFromBase64String(settingString, Shuffler.GetSelectedOptions().OnlyCosmetic().GetSorted(),
                Shuffler.GetSelectedOptions().OnlyCosmetic().GetCrc32());

            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            Logger.Instance.SaveLogTransaction();
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
    }    
    
    public string GetSelectedSettingsString()
    {
        return MinifiedSettings.GenerateSettingsString(Shuffler.GetSelectedOptions().OnlyLogic().GetSorted(),
            Shuffler.GetSelectedOptions().OnlyLogic().GetCrc32());
    }

    public string GetSelectedCosmeticsString()
    {
        return MinifiedSettings.GenerateSettingsString(Shuffler.GetSelectedOptions().OnlyCosmetic().GetSorted(),
            Shuffler.GetSelectedOptions().OnlyCosmetic().GetCrc32());
    }

    public string GetFinalSettingsString()
    {
        return MinifiedSettings.GenerateSettingsString(Shuffler.GetFinalOptions().OnlyLogic().GetSorted(),
            Shuffler.GetFinalOptions().OnlyLogic().GetCrc32());
    }

    public string GetFinalCosmeticsString()
    {
        return MinifiedSettings.GenerateSettingsString(Shuffler.GetFinalOptions().OnlyCosmetic().GetSorted(),
            Shuffler.GetFinalOptions().OnlyCosmetic().GetCrc32());
    }

    public ShufflerControllerResult LoadRom(string filename)
    {
        try
        {
            Rom.Initialize(filename);
            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public uint GetSettingsChecksum()
    {
        return Shuffler.GetLogicOptionsCrc32();
    }

    public uint GetCosmeticsChecksum()
    {
        return Shuffler.GetCosmeticOptionsCrc32();
    }

    public void FlushLogger()
    {
        Logger.Instance.Flush();
    }

    public void SetRandomizationSeed(ulong seed)
    {
        Shuffler.SetSeed(seed);
    }

    public OptionList GetSelectedOptions()
    {
        return Shuffler.GetSelectedOptions();
    }

    public OptionList GetFinalOptions()
    {
        return Shuffler.GetFinalOptions();
    }

    public ShufflerControllerResult LoadLogicFile(string? filename = null)
    {
        try
        {
            Shuffler.LoadOptions(filename);
            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public ShufflerControllerResult SaveAndPatchRom(string filename, string? patchFile = null)
    {
        try
        {
            Shuffler.ValidateState(true);
            var romBytes = Shuffler.GetRandomizedRom();
            File.WriteAllBytes(filename, romBytes);
            int exitCode = Shuffler.ApplyPatch(filename, patchFile);
            if (exitCode != 0)
                throw new Exception("Errors occured when saving the rom");
            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public ShufflerControllerResult CreatePatch(string patchFilename, string? patchFile = null)
    {
        try
        {
            Shuffler.ValidateState(true);
            var romBytes = Shuffler.GetRandomizedRom();
            var stream = new MemoryStream(romBytes);
            int exitCode = Shuffler.ApplyPatch(stream, patchFile);
            if (exitCode != 0)
                throw new Exception("Errors occured when saving the rom");
            var patch = BpsPatcher.GeneratePatch(Rom.Instance!.RomData, romBytes, patchFilename);
            File.WriteAllBytes(patchFilename, patch.Content);
            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public PatchFile CreatePatch(out ShufflerControllerResult result)
    {
        Shuffler.ValidateState(true);
        var romBytes = Shuffler.GetRandomizedRom();
        var stream = new MemoryStream(romBytes);
        var exitCode = Shuffler.ApplyPatch(stream);
        if (exitCode == 0)
        {
            result = new ShufflerControllerResult() { WasSuccessful = true };
            return BpsPatcher.GeneratePatch(Rom.Instance!.RomData, romBytes, "Patch");
        }
        result = new ShufflerControllerResult() { WasSuccessful = false, ErrorMessage = $"ColorzCore returned non-zero exit code {exitCode}" };
        return new PatchFile();
    }

    public string CreateSpoiler()
    {
        Shuffler.ValidateState(true);
        return Shuffler.GetSpoiler();
    }

    /// <summary>
    ///     Creates a patch from a patched ROM
    /// </summary>
    /// <param name="patchFilename"></param>
    /// <param name="patchedRomFilename"></param>
    /// <returns></returns>
    public ShufflerControllerResult SaveRomPatch(string patchFilename, string patchedRomFilename)
    {
        if (Rom.Instance == null)
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                ErrorMessage =
                    "Cannot patch ROM! No base ROM was loaded! Please select a vanilla European Minish Cap ROM and try again."
            };

        try
        {
            var patchedRom = File.ReadAllBytes(patchedRomFilename);
            var patch = BpsPatcher.GeneratePatch(Rom.Instance.RomData, patchedRom, patchFilename);
            File.WriteAllBytes(patchFilename, patch.Content);
            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    /// <summary>
    ///     Applies a BPS patch to a vanilla EU Minish Cap ROM
    /// </summary>
    /// <param name="outputFilename"></param>
    /// <param name="patchFilename"></param>
    /// <returns></returns>
    public ShufflerControllerResult PatchRom(string outputFilename, string patchFilename)
    {
        if (Rom.Instance == null)
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                ErrorMessage =
                    "Cannot patch ROM! No base ROM was loaded! Please select a vanilla European Minish Cap ROM and try again."
            };

        try
        {
            var patchContent = File.ReadAllBytes(patchFilename);
            var patchedRom = BpsPatcher.ApplyPatch(Rom.Instance.RomData, new PatchFile { Content = patchContent });
            File.WriteAllBytes(outputFilename, patchedRom);
            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public ShufflerControllerResult SaveSpoiler(string filename)
    {
        try
        {
            Shuffler.ValidateState(true);
            var spoiler = Shuffler.GetSpoiler();
            File.WriteAllText(filename, spoiler);
            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public ShufflerControllerResult ExportDefaultLogic(string filepath)
    {
        try
        {
            var assembly = Assembly.GetAssembly(typeof(ControllerBase));
            using var stream = assembly?.GetManifestResourceStream("RandomizerCore.Resources.default.logic");
            File.WriteAllText(filepath, new StreamReader(stream).ReadToEnd());
            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

	public ShufflerControllerResult ExportYaml(string filepath, bool mystery)
	{
        string? name = null;
        var index = filepath.LastIndexOf(Path.DirectorySeparatorChar);
        if (index != -1)
        {
            name = filepath[(index + 1)..];
            index = name.LastIndexOf(".", StringComparison.Ordinal);
            if(index != -1)
                name = name[..index];
        }
        try
        {
			File.WriteAllText(filepath, YamlParser.CreateYAML(name, null, GetSelectedOptions(), mystery));
			return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
			return new ShufflerControllerResult
			{
				WasSuccessful = false,
				Error = e,
                ErrorMessage = e.Message,
			};
        }
		finally
		{
			Logger.Instance.SaveLogTransaction();
		}
    }

    public ShufflerControllerResult SaveSettingsAsYaml(string filepath, string name, List<LogicOptionBase> options)
    {
        try
        {
            File.WriteAllText(filepath, YamlParser.CreateYAML(name, null, options, false));

            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            Logger.Instance.SaveLogTransaction();
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
    }
    
    public ShufflerControllerResult LoadLogicSettingsFromYaml(string filepath)
    {
        try
        {
            var options = Shuffler.GetSelectedOptions().OnlyLogic();
            var result = YamlParser.ParseYAML(File.ReadAllText(filepath), options, new SquaresRandomNumberGenerator());
            LoadSettings(true, false, options, result);

            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            Logger.Instance.SaveLogTransaction();
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
    }

    public ShufflerControllerResult LoadCosmeticsFromYaml(string filepath)
    {
        try
        {
            var options = Shuffler.GetSelectedOptions().OnlyCosmetic();
            var result = YamlParser.ParseYAML(File.ReadAllText(filepath), options, new SquaresRandomNumberGenerator());
            LoadSettings(false, true, options, result);

            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            Logger.Instance.SaveLogTransaction();
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
    }

    public ShufflerControllerResult LoadAllSettingsFromYaml(string filepath)
    {
        try
        {
            var options = Shuffler.GetSelectedOptions();
            var result = YamlParser.ParseYAML(File.ReadAllText(filepath), options, new SquaresRandomNumberGenerator());
            LoadSettings(true, true, options, result);

            return new ShufflerControllerResult { WasSuccessful = true };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            Logger.Instance.SaveLogTransaction();
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
                ErrorMessage = e.Message
            };
        }
    }

    private void LoadSettings(bool loadLogicSettings, bool loadCosmeticSettings, IEnumerable<LogicOptionBase> options, YAMLResult result)
    {
        var resultOptionsIndex = 0;

        foreach (var option in options)
        {
            if (option.Type == LogicOptionType.Setting ? !loadLogicSettings : !loadCosmeticSettings) continue;
            
            if (option.Name != result.Options[resultOptionsIndex].Name)
                throw new Exception($"Attempt to load options from yaml failed! Expected {result.Options[resultOptionsIndex].Name}, got {option.Name}");
                        
            option.CopyValueFrom(result.Options[resultOptionsIndex++]);
            option.NotifyObservers();
        }
    }

    protected ControllerBase(Type baseClassType)
    {
        Logger.Instance.LogInfo($"Minish Cap Randomizer Core Version {VersionName} {RevName} initialized!");
        Logger.Instance.LogInfo($"Shuffler {baseClassType} initialized!");
        Logger.Instance.SaveLogTransaction(true);
    }
    
    internal ShufflerBase Shuffler { get; set; }

    public abstract ulong FinalSeed { get; }

    public abstract string SeedFilename { get; }

    public abstract ShufflerControllerResult LoadLocations(string? logicFile = null, string? yamlFileLogic = null, string? yamlFileCosmetics = null, bool useGlobalYAML = false);

    public abstract ShufflerControllerResult Randomize(int retries = 1, bool useSphereBasedShuffler = false);

    public abstract string GetEventWrites();
}
