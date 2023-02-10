using RandomizerCore.Controllers.Models;
using RandomizerCore.Core;
using RandomizerCore.Randomizer;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Logic.Options;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Models;
using RandomizerCore.Utilities.Util;
using System.Reflection;

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
	private string? _cachedLogicFile;

#if DEBUG
	public string AppName => "Minish Cap Randomizer Debug Build";
#else
	public string AppName => "Minish Cap Randomizer";
#endif

    public string VersionName => VersionIdentifier;
    public string RevName => RevisionIdentifier;

    internal static string VersionIdentifier => "v0.7.0";
    internal static string RevisionIdentifier => "alpha-rev2";

    public string SeedFilename => $"Minish Randomizer-{_shuffler.Seed}-{_shuffler.Version}-{_shuffler.GetOptionsIdentifier()}";

	public int FinalSeed => _shuffler.Seed;

	public ShufflerController()
	{
		_shuffler = new Shuffler();
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

	public ShufflerControllerResult LoadSettingsFromSettingString(string settingString)
	{
		try
		{
			MinifiedSettings.GenerateSettingsFromBase64String(settingString, _shuffler.GetSortedSettings(), _shuffler.GetLogicOptionsCrc32());

			return new ShufflerControllerResult { WasSuccessful = true };
		}
		catch (Exception e) 
		{
			Logger.Instance.LogException(e);
			Logger.Instance.SaveLogTransaction();
			return new ShufflerControllerResult { 
				WasSuccessful = false,
				Error = e,
			};
		}
	}

	public ShufflerControllerResult LoadCosmeticsFromCosmeticsString(string settingString)
	{
		try
		{
			MinifiedSettings.GenerateSettingsFromBase64String(settingString, _shuffler.GetSortedCosmetics(), _shuffler.GetCosmeticOptionsCrc32());

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
			};
		}
	}

	public string GetSettingsString()
	{
		return MinifiedSettings.GenerateSettingsString(_shuffler.GetSortedSettings(), _shuffler.GetLogicOptionsCrc32());
	}

	public string GetCosmeticsString()
	{
		return MinifiedSettings.GenerateSettingsString(_shuffler.GetSortedCosmetics(), _shuffler.GetCosmeticOptionsCrc32());
	}

	public ShufflerControllerResult LoadRom(string filename)
	{
		try
		{
			Rom.Initialize(filename);
			return new ShufflerControllerResult { WasSuccessful = true, };
		}
		catch (Exception e)
		{
			Logger.Instance.LogException(e);
			return new ShufflerControllerResult
			{
				WasSuccessful = false,
				Error = e,
			};
		}
		finally
		{
			Logger.Instance.SaveLogTransaction();
		}
	}

	public uint GetSettingsChecksum()
	{
		return _shuffler.GetLogicOptionsCrc32();
	}

	public uint GetCosmeticsChecksum()
	{
		return _shuffler.GetCosmeticOptionsCrc32();
	}

	public void FlushLogger() => Logger.Instance.Flush();

	public void SetRandomizationSeed(int seed) => _shuffler.SetSeed(seed);

	public List<LogicOptionBase> GetLogicOptions() => _shuffler.GetOptions();

	public ShufflerControllerResult LoadLogicFile(string? filename = null)
	{
		try
		{
            _shuffler.LoadOptions(filename);
			return new ShufflerControllerResult { WasSuccessful = true };
		}
		catch (Exception e)
		{
			Logger.Instance.LogException(e);
			return new ShufflerControllerResult
			{
				WasSuccessful = false,
				Error = e,
			};
		}
		finally
		{
			Logger.Instance.SaveLogTransaction();
		}
	}

	public ShufflerControllerResult LoadLocations(string? filename = null)
	{
		try
		{
			_shuffler.ValidateState();
			_shuffler.LoadLocations(filename);
			_cachedLogicFile = filename;
			return new ShufflerControllerResult { WasSuccessful = true };
		}
		catch (Exception e)
		{
			Logger.Instance.LogException(e);
			return new ShufflerControllerResult
			{
				WasSuccessful = false,
				Error = e,
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
                    LoadLocations(_cachedLogicFile);
                    _shuffler.SetSeed(new Random(_shuffler.Seed).Next());
                    Logger.Instance.SaveLogTransaction();
                }
            }

            if (!successfulGeneration) throw new ShuffleException($"Multiple Failures Occurred - Only showing last failure message: {capturedExceptions.Last().Message}");

            return new ShufflerControllerResult { WasSuccessful = successfulGeneration };
        }
        catch (Exception e)
        {
            Logger.Instance.LogException(e);
            return new ShufflerControllerResult
            {
                WasSuccessful = false,
                Error = e,
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
			_shuffler.ValidateState(true);
			var romBytes = _shuffler.GetRandomizedRom();
			File.WriteAllBytes(filename, romBytes);
			_shuffler.ApplyPatch(filename, patchFile);
			return new ShufflerControllerResult { WasSuccessful = true };
		}
		catch (Exception e)
		{
			Logger.Instance.LogException(e);
			return new ShufflerControllerResult
			{
				WasSuccessful = false,
				Error = e,
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
			_shuffler.ValidateState(true);
			var romBytes = _shuffler.GetRandomizedRom();
			var stream = new MemoryStream(romBytes);
			_shuffler.ApplyPatch(stream, patchFile);
			var patch = BPSPatcher.GeneratePatch(Rom.Instance!.romData, romBytes, patchFilename);
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
			};
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
	public ShufflerControllerResult SaveRomPatch(string patchFilename, string patchedRomFilename)
	{
		if (Rom.Instance == null)
			return new ShufflerControllerResult
			{
				WasSuccessful = false,
				ErrorMessage = "Cannot patch ROM! No base ROM was loaded! Please select a vanilla European Minish Cap ROM and try again.",
			};

		try
		{
			var patchedRom = File.ReadAllBytes(patchedRomFilename);
			var patch = BPSPatcher.GeneratePatch(Rom.Instance.romData, patchedRom, patchFilename);
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
			};
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
	public ShufflerControllerResult PatchRom(string outputFilename, string patchFilename)
	{
		if (Rom.Instance == null)
			return new ShufflerControllerResult
			{
				WasSuccessful = false,
				ErrorMessage = "Cannot patch ROM! No base ROM was loaded! Please select a vanilla European Minish Cap ROM and try again.",
			};

		try
		{
			var patchContent = File.ReadAllBytes(patchFilename);
			var patchedRom = BPSPatcher.ApplyPatch(Rom.Instance.romData, new PatchFile {Content = patchContent});
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
			_shuffler.ValidateState(true);
			var spoiler = _shuffler.GetSpoiler();
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
			};
		}
		finally
		{
			Logger.Instance.SaveLogTransaction();
		}
	}
}
