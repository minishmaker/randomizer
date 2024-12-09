using RandomizerCore.Controllers.Models;
using RandomizerCore.Random;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Shuffler;
using RandomizerCore.Utilities.Logging;

namespace RandomizerCore.Controllers;

public class YamlController : ShufflerController
{
    private readonly YamlShuffler _shuffler;
    private string? _cachedLogicFile;
    
    private string? CachedYamlFileLogic;
    private string? CachedYamlFileCosmetics;
    private bool CachedUseGlobalYaml;

    public bool IsUsingLogicYaml() => _shuffler.IsUsingLogicYaml();

    public bool IsUsingCosmeticsYaml() => _shuffler.IsUsingCosmeticsYaml();

    public bool IsGlobalYamlMode() => _shuffler.IsGlobalYamlMode();

    public string? GetLogicYamlName() => _shuffler.GetLogicYamlName();

    public string? GetCosmeticsYamlName() => _shuffler.GetCosmeticsYamlName();

    public override ulong FinalSeed => _shuffler.Seed;
    
    public override string SeedFilename => _shuffler.IsGlobalYamlMode()
        ? $"Minish Randomizer-{_shuffler.Seed:X}-{VersionIdentifier}-{(_shuffler.IsUsingLogicYaml() ? _shuffler.GetLogicYamlName() : _shuffler.GetSelectedOptions().GetIdentifier(true))}"
        : $"Minish Randomizer-{_shuffler.Seed:X}-{VersionIdentifier}-{(_shuffler.IsUsingLogicYaml() ? _shuffler.GetLogicYamlName() : _shuffler.GetSelectedOptions().OnlyLogic().GetIdentifier(false))}-{(_shuffler.IsUsingCosmeticsYaml() ? _shuffler.GetCosmeticsYamlName() : _shuffler.GetSelectedOptions().OnlyCosmetic().GetIdentifier(false))}";

    public string GetSeedFilename(string? overrideSelectedLogic = null, string? overrideSelectedCosmetics = null)
    {
        if (_shuffler.IsGlobalYamlMode() && _shuffler.IsUsingLogicYaml())
            return $"Minish Randomizer-{_shuffler.Seed:X}-{VersionIdentifier}-{_shuffler.GetLogicYamlName()}";
        
        return $"Minish Randomizer-{_shuffler.Seed:X}-{VersionIdentifier}-" +
               $"{(_shuffler.IsUsingLogicYaml() ? _shuffler.GetLogicYamlName() : overrideSelectedLogic ?? "Custom")}-" +
               $"{(_shuffler.IsUsingCosmeticsYaml() ? _shuffler.GetCosmeticsYamlName() : overrideSelectedCosmetics ?? "Custom")}";
    }

    public YamlController() : base(typeof(YamlController))
    {
        _shuffler = new YamlShuffler();
        Shuffler = _shuffler;
    }

    public override string GetEventWrites()
    {
        return _shuffler.GetEventWrites();
    }
    
    public override ShufflerControllerResult LoadLocations(string? logicFile = null, string? yamlFileLogic = null, string? yamlFileCosmetics = null, bool useGlobalYAML = false)
    {
        try
        {
            _shuffler.ValidateState();
            _shuffler.LoadLocationsYaml(logicFile, yamlFileLogic, yamlFileCosmetics, useGlobalYAML);
            _cachedLogicFile = logicFile;
            CachedYamlFileLogic = yamlFileLogic;
            CachedYamlFileCosmetics = useGlobalYAML ? yamlFileLogic : yamlFileCosmetics;
            CachedUseGlobalYaml = useGlobalYAML;
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
    
    public override ShufflerControllerResult Randomize(int retries = 1, bool useSphereBasedShuffler = false)
    {
        try
        {
            _shuffler.ValidateState();

            var attempts = 1;
            if (retries <= 0) retries = 1;
            var successfulGeneration = false;
            var capturedExceptions = new List<Exception>();
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
                    capturedExceptions.Add(e);
                    attempts++;
                    if (attempts > retries) break;
                    Logger.Instance.SaveLogTransaction();

                    LoadLocations(_cachedLogicFile, CachedYamlFileLogic, CachedYamlFileCosmetics, CachedUseGlobalYaml);
                    _shuffler.SetSeed(new SquaresRandomNumberGenerator().Next());
                    Logger.Instance.SaveLogTransaction();
                }
            }

            if (!successfulGeneration)
                throw new ShuffleException($"Multiple Failures Occurred - Only showing last failure message: {capturedExceptions.Last().Message}");

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
}
