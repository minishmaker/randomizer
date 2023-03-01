using System.Reflection;
using RandomizerCore.Controllers.Models;
using RandomizerCore.Core;
using RandomizerCore.Randomizer;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Logic.Options;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Models;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Controllers;

/*
 * Changing the randomizer over to a MVC system since it allows us to have less coupling.
 * Having the controllers be the insertion point for the UI and CLI removes hard dependencies on code and lets us
 * encapsulate things better. It also helps force better code practice and makes adding code features easier.
 *
 * Insertion point for the Shuffler
 */
public class ShufflerController
{
    private readonly Shuffler Shuffler;
    private string? CachedLogicFile;
	private string? CachedYAMLFile;

#if DEBUG
    public string AppName => "Minish Cap Randomizer Debug Build";
#else
	public string AppName => "Minish Cap Randomizer";
#endif

    public string VersionName => VersionIdentifier;
    public string RevName => RevisionIdentifier;

    internal static string VersionIdentifier => "v0.7.0";
    internal static string RevisionIdentifier => "alpha-rev3";

    public string SeedFilename =>
        $"Minish Randomizer-{Shuffler.Seed}-{Shuffler.Version}-{(Shuffler.IsUsingYAML() ? Shuffler.GetYAMLName() : Shuffler.GetSelectedOptions().GetIdentifier())}";

    public int FinalSeed => Shuffler.Seed;

    public ShufflerController()
    {
        Shuffler = new Shuffler();
        Logger.Instance.LogInfo($"Minish Cap Randomizer Core Version {VersionName} {RevName} initialized!");
        Logger.Instance.SaveLogTransaction(true);
    }

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

    public bool IsUsingYAML() => Shuffler.IsUsingYAML();

    public string GetYAMLName() => Shuffler.GetYAMLName();

    public string GetYAMLDescription() => Shuffler.GetYAMLDescription();

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

    public void FlushLogger()
    {
        Logger.Instance.Flush();
    }

    public void SetRandomizationSeed(int seed)
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

    public ShufflerControllerResult LoadLocations(string? logicFile = null, string? yamlFile = null)
    {
        try
        {
            Shuffler.ValidateState();
            Shuffler.LoadLocations(logicFile, yamlFile);
            CachedLogicFile = logicFile;
            CachedYAMLFile = yamlFile;
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

    public ShufflerControllerResult Randomize(int retries = 1, bool useSphereBasedShuffler = false)
    {
        try
        {
            Shuffler.ValidateState();

            var attempts = 1;
            if (retries <= 0) retries = 1;
            var successfulGeneration = false;
            var capturedExceptions = new List<Exception>();
            while (attempts <= retries && !successfulGeneration)
                try
                {
                    Shuffler.RandomizeLocations(useSphereBasedShuffler);
                    successfulGeneration = true;
                }
                catch (Exception e)
                {
                    Logger.Instance.LogException(e);
                    capturedExceptions.Add(e);
                    attempts++;
                    LoadLocations(CachedLogicFile, CachedYAMLFile);
                    Shuffler.SetSeed(new Random(Shuffler.Seed).Next());
                    Logger.Instance.SaveLogTransaction();
                }

            if (!successfulGeneration)
                throw new ShuffleException(
                    $"Multiple Failures Occurred - Only showing last failure message: {capturedExceptions.Last().Message}");

            return new ShufflerControllerResult { WasSuccessful = successfulGeneration };
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
            Shuffler.ApplyPatch(filename, patchFile);
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
            Shuffler.ApplyPatch(stream, patchFile);
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
            var assembly = Assembly.GetAssembly(typeof(ShufflerController));
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

	public ShufflerControllerResult ExportYAML(string filepath, bool mystery)
	{
        string? name = null;
        var index = filepath.LastIndexOf(Path.DirectorySeparatorChar);
        if (index != -1)
        {
            name = filepath.Substring(index + 1);
            index = name.LastIndexOf(".");
            if(index != -1)
                name = name.Substring(0, index);
        }
        try
        {
			File.WriteAllText(filepath, Mystery.CreateYAML(name, null, GetSelectedOptions(), mystery));
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
}
