using RandomizerCore.Core;
using RandomizerCore.Randomizer;
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
    private readonly Shuffler _shuffler;
    
    public ShufflerController()
    {
        _shuffler = new Shuffler();
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

    public void LoadSettingsFromSettingString(string settingString)
    {
        
    }

    public string GetSettingString()
    {
        return "";
    }

    public bool LoadRom(string filename)
    {
        try
        {
            Rom.Initialize(filename);
            return true;
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return false;
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public void SetRandomizationSeed(int seed) => _shuffler.SetSeed(seed);

    public List<LogicOptionBase> GetLogicOptions() => _shuffler.GetOptions();

    public bool LoadLogicFile(string? filename = null)
    {
        try
        {
            _shuffler.LoadOptions(filename);
            return true;
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return false;
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public bool LoadLocations(string? filename = null)
    {
        try
        {
            _shuffler.LoadLocations(filename);
            return true;
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return false;
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public bool Randomize(int retries = 1)
    {
        try
        {
            _shuffler.ValidateState();

            var attempts = 1;
            var successfulGeneration = false;
            while (attempts <= retries && !successfulGeneration)
            {
                try
                {
                    _shuffler.RandomizeLocations();
                    successfulGeneration = true;
                }
                catch (Exception e)
                {
                    Logger.Instance.LogException(e);
                    attempts++;
                    _shuffler.SetSeed(new Random(_shuffler.Seed).Next());
                    Logger.Instance.SaveLogTransaction();
                }
            }

            return successfulGeneration;
        } 
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return false;
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public bool SaveAndPatchRom(string filename, string? patchFile = null)
    {
        try
        {
            _shuffler.ValidateState(true);
            var romBytes = _shuffler.GetRandomizedRom();
            File.WriteAllBytes(filename, romBytes);
            _shuffler.ApplyPatch(filename, patchFile);
            return true;
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return false;
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public bool CreatePatch(string patchFilename, string? patchFile = null)
    {
        try
        {
            _shuffler.ValidateState(true);
            var romBytes = _shuffler.GetRandomizedRom();
            var stream = new MemoryStream(romBytes);
            _shuffler.ApplyPatch(stream, patchFile);
            var patch = BPSPatcher.GeneratePatch(Rom.Instance!.romData, romBytes, patchFilename);
            File.WriteAllBytes(patchFilename, patch.Content);
            return true;
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return false;
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    /// <summary>
    /// Creates a patch from a patched ROM
    /// </summary>
    /// <param name="patchFilename"></param>
    /// <param name="patchedRomFilename"></param>
    /// <returns></returns>
    public bool SaveRomPatch(string patchFilename, string patchedRomFilename)
    {
        if (Rom.Instance == null) return false;
        
        try
        {
            var patchedRom = File.ReadAllBytes(patchedRomFilename);
            var patch = BPSPatcher.GeneratePatch(Rom.Instance.romData, patchedRom, patchFilename);
            File.WriteAllBytes(patchFilename, patch.Content);
            return true;
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return false;
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    /// <summary>
    /// Applies a BPS patch to a vanilla EU Minish Cap ROM
    /// </summary>
    /// <param name="outputFilename"></param>
    /// <param name="patchFilename"></param>
    /// <returns></returns>
    public bool PatchRom(string outputFilename, string patchFilename)
    {
        if (Rom.Instance == null) return false;

        try
        {
            var patchContent = File.ReadAllBytes(patchFilename);
            var patchedRom = BPSPatcher.ApplyPatch(Rom.Instance.romData, new PatchFile {Content = patchContent});
            File.WriteAllBytes(outputFilename, patchedRom);
            return true;
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return false;
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }

    public bool SaveSpoiler(string filename)
    {
        try
        {
            _shuffler.ValidateState(true);
            var spoiler = _shuffler.GetSpoiler();
            File.WriteAllText(filename, spoiler);
            return true;
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return false;
        }
        finally
        {
            Logger.Instance.SaveLogTransaction();
        }
    }
}