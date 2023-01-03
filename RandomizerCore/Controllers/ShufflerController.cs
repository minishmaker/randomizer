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
    private Shuffler _shuffler;
    private Rom? _rom;
    private string _patchedRomFilename;
    
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

    public void LoadRom(string filename)
    {
        _rom = new Rom(filename);
    }

    public void SetRandomizationSeed(int seed) => _shuffler.SetSeed(seed);

    public List<LogicOptionBase> GetLogicOptions() => _shuffler.GetOptions();

    public void LoadLogicFile(string? filename = null)
    {
        _shuffler.LoadOptions(filename);
    }

    public void LoadLocations(string? filename = null)
    {
        _shuffler.LoadLocations(filename);
    }

    public bool Randomize(int retries = 1)
    {
        var attempts = 1;
        var successfulGeneration = false;
        while (attempts <= retries && !successfulGeneration)
        {
            try
            {
                _shuffler.RandomizeLocations();
                successfulGeneration = true;
            }
            catch
            {
                attempts++;
                _shuffler.SetSeed(new Random().Next());
            }
        }

        return successfulGeneration;
    }

    public void SaveAndPatchRom(string filename, string? patchFile = null)
    {
        var romBytes = _shuffler.GetRandomizedRom();
        
        File.WriteAllBytes(filename, romBytes);
        
        _shuffler.ApplyPatch(filename, patchFile);

        _patchedRomFilename = filename;
    }

    public void CreatePatch(string patchFilename, string? patchFile = null)
    {
        var romBytes = _shuffler.GetRandomizedRom();

        var stream = new MemoryStream(romBytes);
        
        _shuffler.ApplyPatch(stream, patchFile);

        var patch = BPSPatcher.GeneratePatch(_rom.romData, romBytes, patchFilename);
        
        File.WriteAllBytes(patchFilename, patch.Content);
    }

    public bool SaveRomPatch(string patchFilename, string? patchedRomFilename)
    {
        if (_rom == null) return false;

        try
        {
            var patchedRom =
                File.ReadAllBytes(string.IsNullOrEmpty(patchedRomFilename) ? _patchedRomFilename : patchedRomFilename);

            var patch = BPSPatcher.GeneratePatch(_rom.romData, patchedRom, patchFilename);

            File.WriteAllBytes(patchFilename, patch.Content);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool PatchRom(string outputFilename, string patchFilename)
    {
        if (_rom == null) return false;

        try
        {
            var patchContent = File.ReadAllBytes(patchFilename);
            
            var patchedRom = BPSPatcher.ApplyPatch(_rom.romData, new PatchFile {Content = patchContent});

            File.WriteAllBytes(outputFilename, patchedRom);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void SaveSpoiler(string filename)
    {
        var spoiler = _shuffler.GetSpoiler();
        
        File.WriteAllText(filename, spoiler);
    }
}