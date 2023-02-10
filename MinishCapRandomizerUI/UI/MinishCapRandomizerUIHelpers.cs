using MinishCapRandomizerUI.DrawConstants;
using MinishCapRandomizerUI.Elements;
using MinishCapRandomizerUI.UI.Config;
using Newtonsoft.Json;
using RandomizerCore.Controllers.Models;
using RandomizerCore.Randomizer.Exceptions;

namespace MinishCapRandomizerUI.UI
{
	/// <summary>
	/// This is just here so you can actually edit this file in the code editor without going through a bunch of designer crap
	/// </summary>
	public class MinishCapRandomizerUIHelpers
	{

	}

	/// <summary>
	/// Useful functions for UI generation, updates, and displaying dialogues. These functions are placed here to reduce clutter and to make the main class only contain UI functions
	/// </summary>
	partial class MinishCapRandomizerUI
	{
		/// <summary>
		/// Displays an alert and runs the specified callback if not null. If callbackExecuteOnResult is null and a callback
		/// is supplied, then the callback will be executed no matter what.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="caption"></param>
		/// <param name="buttons"></param>
		/// <param name="icon"></param>
		/// <param name="callbackExecuteOnResult"></param>
		/// <param name="callback"></param>
		private static void DisplayAlert(
			string text,
			string caption,
			MessageBoxButtons buttons,
			MessageBoxIcon icon,
			DialogResult? callbackExecuteOnResult = null,
			Action? callback = null)
		{
			var result = MessageBox.Show(text, caption, buttons, icon);

			if ((callbackExecuteOnResult == null && callback != null) ||
				(callbackExecuteOnResult != null && callback != null && callbackExecuteOnResult == result))
			{
				callback.Invoke();
			}
		}

		private static void DisplayOpenDialog(string filter, string title, DialogResult expectedResult, Action<string> callback)
		{
			var openDialog = new OpenFileDialog
			{
				Filter = filter,
				Title = title,
			};

			var result = openDialog.ShowDialog();
			if (result != expectedResult) return;

			callback.Invoke(openDialog.FileName);
		}

		private static void DisplaySaveDialog(string filter, string title, string filename, DialogResult expectedResult,
			Action<string> callback)
		{
			var saveFileDialog = new SaveFileDialog
			{
				Filter = filter,
				Title = title,
				FileName = filename,
			};

			var result = saveFileDialog.ShowDialog();

			if (result != expectedResult) return;

			callback.Invoke(saveFileDialog.FileName);
		}

		private static void ShowInputDialog(string title, string message, Action<string> callback)
		{
			var inputDialog = new InputDialog.InputDialog();
			inputDialog.Initialize(title, message, callback);
			inputDialog.ShowDialog();
		}

		private static void DisplayConditionalAlertFromShufflerResult(ShufflerControllerResult result, string successMessage, string successTitle, string startOfErrorMessage, string errorTitle)
		{
			if (result.WasSuccessful)
			{
				DisplayAlert(successMessage, successTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (result.Error != null)
			{
				switch (result.Error)
				{
					case ParserException:
						DisplayAlert($"{startOfErrorMessage}\nFailed to parse!\nError: {result.Error.Message}", errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
					case ShuffleException:
						DisplayAlert($"{startOfErrorMessage}\nFailed to shuffle ROM!\nError: {result.Error.Message}", errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
					case ShufflerConfigurationException:
						DisplayAlert($"{startOfErrorMessage}\nShuffler is in an invalid state!\nError: {result.Error.Message}", errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
					default:
						DisplayAlert($"{startOfErrorMessage}\nCheck the logs for more information.", errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
				}
				return;
			}

			if (!string.IsNullOrEmpty(result.ErrorMessage))
			{
				DisplayAlert($"{startOfErrorMessage}\nError: {result.ErrorMessage}", errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}


		private void LoadTooltips()
		{
			var tip = new ToolTip();
			tip.UseFading = true;
			tip.AutoPopDelay = Constants.TooltipDisplayLengthMs;
			tip.InitialDelay = Constants.TooltipInitialShowDelayMs;
			tip.ReshowDelay = Constants.TooltipRepeatDelayMs;
			tip.SetToolTip(UseCustomCosmetics, "Patches in cosmetics based on what you have selected in the cosmetics tab.\nOnly works if both logic and patches are set to default.");
            tip.SetToolTip(UseSphereBasedShuffler, @"Generates the seed using the sphere based shuffler. This shuffler is guaranteed to generate a seed that can beat vaati on the first attempt unless it is impossible to get out of the first sphere or takes more than 50,000 attempts to gen the seed.

This shuffler gets the locations in sphere 1, fills them, checks for progression, advances to the next sphere, etc. until it can beat vaati. 
If no progression is available, it goes to the previous sphere and re-places items there to try and change what is processed the next time.

This shuffler is known for creating very linear and plando-esque seeds, use as your own risk. We have cases where it has generated 30+ sphere seeds when the normal shuffler generated 12-15 sphere seeds.

Generating seeds with this shuffler may freeze the randomizer application for many minutes while it tries different permutations, however if it exceeds 50,000 attempts it will break out and error. If this happens, let us know so we can make improvements to the shuffler.");
        }

		private void LoadPresets()
		{
			try
			{
				var presets = File.ReadAllText($"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}/SettingPresets.json");
				_settingPresets = JsonConvert.DeserializeObject<SettingPresets>(presets)!;

				if (_settingPresets == null)
				{
					_settingPresets = new SettingPresets();
				}

				var checksum = _shufflerController.GetSettingsChecksum();

				if (_settingPresets.SettingsPresets.TryGetValue(checksum, out var settingsPresets) && settingsPresets.Any())
				{
					SettingPresets.Items.AddRange(settingsPresets.Select(preset => preset.PresetName).ToArray());
					SettingPresets.SelectedIndex = 0;
				};

				checksum = _shufflerController.GetCosmeticsChecksum();

				if (_settingPresets.CosmeticsPresets.TryGetValue(checksum, out var cosmeticsPresets) && cosmeticsPresets.Any())
				{
					CosmeticsPresets.Items.AddRange(cosmeticsPresets.Select(preset => preset.PresetName).ToArray());
					CosmeticsPresets.SelectedIndex = 0;
				}
			}
			catch
			{
				DisplayAlert("Could not load settings presets. The file may have been moved or deleted.", "Could Not Load Settings Presets", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				_settingPresets = new SettingPresets();
			}
		}

		private void LoadConfig()
		{
			try
			{
				var configAsJson = File.ReadAllText($"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}/config.json");
				_configuration = JsonConvert.DeserializeObject<UIConfiguration>(configAsJson)!;
				if (_configuration == null)
					_configuration = new UIConfiguration
					{
						RomPath = "",
						UseHendrusShuffler = false,
						MaximumRandomizationRetryCount = 1,
						DefaultLoggerPath = "",
						UseVerboseLogger = false,
						UseCustomLogic = false,
						CustomLogicFilepath = "",
						UseCustomPatch = false,
						CustomPatchFilepath = "",
					};
			}
			catch
			{
				_configuration = new UIConfiguration
				{
					RomPath = "",
					UseHendrusShuffler = false,
					MaximumRandomizationRetryCount = 1,
					DefaultLoggerPath = "",
					UseVerboseLogger = false,
					UseCustomLogic = false,
					CustomLogicFilepath = "",
					UseCustomPatch = false,
					CustomPatchFilepath = "",
				};
			}
		}

		private void SaveConfig(object? sender, FormClosingEventArgs e)
		{
			try
			{
				var configAsJson = JsonConvert.SerializeObject(_configuration, Formatting.Indented);
				File.WriteAllText($"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}/config.json", configAsJson);
			}
			catch { }
		}

		private void InitializeUi()
		{
			TabPane.TabPages.Remove(SeedOutput);
			var seed = new Random().Next();
			Seed.Text = $@"{seed}";

			var result = _shufflerController.LoadLogicFile(_configuration.UseCustomLogic ? _configuration.CustomLogicFilepath : "");
			if (!result.WasSuccessful)
			{
				DisplayAlert("Could not load custom logic file from path in the config file. The file may have been moved or deleted.", "Could Not Load Logic File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				_shufflerController.LoadLogicFile();
			}
			else
			{
				UseCustomLogic.Checked = _configuration.UseCustomLogic;
				LogicFilePath.Text = _configuration.CustomLogicFilepath;
			}

			if (!string.IsNullOrEmpty(_configuration.RomPath))
			{
				result = _shufflerController.LoadRom(_configuration.RomPath);
				if (!result.WasSuccessful)
					DisplayAlert("Could not load ROM from path in the config file. The file may have been moved or deleted.", "Could Not Load ROM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				else
					RomPath.Text = _configuration.RomPath;
			}

			UseCustomPatch.Checked = _configuration.UseCustomPatch;
			RomBuildfilePath.Text = _configuration.CustomPatchFilepath;

			logAllTransactionsToolStripMenuItem.Checked = _configuration.UseVerboseLogger;
			_shufflerController.SetLoggerVerbosity(_configuration.UseVerboseLogger);

			if (!string.IsNullOrEmpty(_configuration.DefaultLoggerPath))
				_shufflerController.SetLogOutputPath(_configuration.DefaultLoggerPath);

			RandomizationAttempts.Text = $"{_configuration.MaximumRandomizationRetryCount}";
			UseSphereBasedShuffler.Checked = _configuration.UseHendrusShuffler;

			_defaultSettings = _shufflerController.GetSettingsString();
			_defaultCosmetics = _shufflerController.GetCosmeticsString();

			_shufflerController.SetRandomizationSeed(seed);
		}

		private void UpdateUIWithLogicOptions()
		{
			for (var i = TabPane.TabPages.Count - 1; i >= 2; --i)
				TabPane.TabPages.RemoveAt(i);

			if (_randomizedRomCreated)
				TabPane.TabPages.Add(SeedOutput);

			var options = _shufflerController.GetLogicOptions();
			var wrappedOptions = WrappedLogicOptionFactory.BuildGenericWrappedLogicOptions(options);
			var pages = wrappedOptions.GroupBy(option => option.Page);

			foreach (var page in pages)
			{
				TabPane.TabPages.Add(UIGenerator.BuildSettingsPage(page.ToList(), page.Key));
			}
		}

		private void DisplayAndUpdateSeedInfoPage()
		{
			if (!TabPane.TabPages.Contains(SeedOutput))
				TabPane.TabPages.Insert(2, SeedOutput);

			InputSeedLabel.Text = Seed.Text;
			OutputSeedLabel.Text = @$"{_shufflerController.FinalSeed}";

			var settingsString = _shufflerController.GetSettingsString();
			var cosmeticsString = _shufflerController.GetCosmeticsString();

			SettingNameLabel.Text = _settingPresets.SettingsPresets.TryGetValue(_shufflerController.GetSettingsChecksum(), out var settingsPresets) &&
				settingsPresets.Any(preset => preset.PresetString == settingsString) ?
					settingsPresets.First(preset => preset.PresetString == settingsString).PresetName :
					"Custom";
			CosmeticNameLabel.Text = _settingPresets.CosmeticsPresets.TryGetValue(_shufflerController.GetCosmeticsChecksum(), out var cosmeticsPresets) &&
				cosmeticsPresets.Any(preset => preset.PresetString == cosmeticsString) ?
					cosmeticsPresets.First(preset => preset.PresetString == cosmeticsString).PresetName :
					"Custom"; ;

			SettingHashLabel.Text = settingsString;
			CosmeticStringLabel.Text = cosmeticsString;

			TabPane.SelectedTab = SeedOutput;
		}
	}
}
