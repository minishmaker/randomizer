using System.Reflection;
using RandomizerCore.Controllers.Models;
using RandomizerCore.Core;
using RandomizerCore.Random;
using RandomizerCore.Randomizer;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Shuffler;
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
public class ShufflerController : ControllerBase
{
    private readonly SphereBasedShuffler _shuffler;
    private string? _cachedLogicFile;

    public override string SeedFilename =>
        $"Minish Randomizer-{_shuffler.Seed:X}-{VersionIdentifier}-{_shuffler.GetSelectedOptions().GetIdentifier()}";

    public override ulong FinalSeed => _shuffler.Seed;

    public ShufflerController() : base(typeof(ShufflerController))
    {
        _shuffler = new SphereBasedShuffler();
        Shuffler = _shuffler;
    }

    protected ShufflerController(Type childType) : base(childType)
    {
        
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
            _shuffler.LoadLocations(logicFile);
            _cachedLogicFile = logicFile;
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
                    _shuffler.RandomizeLocations(useSphereBasedShuffler);
                    successfulGeneration = true;
                }
                catch (Exception e)
                {
                    Logger.Instance.LogException(e);
                    capturedExceptions.Add(e);
                    attempts++;
                    if (attempts > retries) break;
                    Logger.Instance.SaveLogTransaction();

                    LoadLocations(_cachedLogicFile);
                    _shuffler.SetSeed(new SquaresRandomNumberGenerator().Next());
                    Logger.Instance.SaveLogTransaction();
                }
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
}
