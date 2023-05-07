using System.Globalization;
using System.Net;
using System.Reflection;
using MinishCapRandomizerUI.DrawConstants;
using MinishCapRandomizerUI.Elements;
using MinishCapRandomizerUI.UI.Config;
using Newtonsoft.Json;
using RandomizerCore.Controllers;
using RandomizerCore.Controllers.Models;
using RandomizerCore.Random;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Utilities.Util;

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
			tip.SetToolTip(UseSphereBasedShuffler, @"Generates the seed using the sphere based shuffler. This shuffler is guaranteed to generate a seed that can beat vaati on the first attempt unless it is impossible to get out of the first sphere or takes more than 50,000 attempts to generate the seed.

This shuffler gets the locations in sphere 1, fills them, checks for progression, advances to the next sphere, etc. until it can beat vaati. 
If no progression is available, it goes to the previous sphere and re-places items there to try and change what is processed the next time.

This shuffler is known for creating very linear and plando-esque seeds, use at your own risk. We have cases where it has generated 30+ sphere seeds when the normal shuffler generated 12-15 sphere seeds.

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

        private static bool CheckForUpdates()
        {
            var githubData = DownloadUrlAsString();
            var releases = JsonConvert.DeserializeObject<List<Github.Release>>(githubData);
            var tag = Assembly.GetExecutingAssembly().GetCustomAttributesData().First(x => x.AttributeType.Name == "AssemblyInformationalVersionAttribute").ConstructorArguments.First().ToString();
            tag = tag[(tag.IndexOf('+') + 1)..^1];
            if (releases!.First().Tag_Name == tag) return false;
            
            var url = new UrlDialog.UrlDialog(releases!.First().Html_Url);
            url.ShowDialog();
            return true;
        }
        
        private static string DownloadUrlAsString()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repositories/177660043/releases");
            request.UserAgent = "MinishCapRandomizerUI";
            request.KeepAlive = false;
            using var response = (HttpWebResponse)request.GetResponse();
            using var responseStream = new StreamReader(response.GetResponseStream());
            return responseStream.ReadToEnd();
        }

        private void InitializeUi()
		{
			TabPane.TabPages.Remove(SeedOutput);
			var seed = new SquaresRandomNumberGenerator().Next();
			Seed.Text = $@"{seed:X}";

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
		    BrowseCustomLogicFile.Enabled = UseCustomLogic.Checked;

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
		    BrowseCustomPatch.Enabled = UseCustomPatch.Checked;

			logAllTransactionsToolStripMenuItem.Checked = _configuration.UseVerboseLogger;
			_shufflerController.SetLoggerVerbosity(_configuration.UseVerboseLogger);

			if (!string.IsNullOrEmpty(_configuration.DefaultLoggerPath))
				_shufflerController.SetLogOutputPath(_configuration.DefaultLoggerPath);

			RandomizationAttempts.Text = $"{_configuration.MaximumRandomizationRetryCount}";
			UseSphereBasedShuffler.Checked = _configuration.UseHendrusShuffler;

			_defaultSettings = _shufflerController.GetSettingsString();
			_defaultCosmetics = _shufflerController.GetCosmeticsString();

			_shufflerController.SetRandomizationSeed(seed);

            checkForUpdatesOnStartToolStripMenuItem.Checked = _configuration.CheckForUpdatesOnStart;
            if (_configuration.CheckForUpdatesOnStart)
                CheckForUpdates();
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
			OutputSeedLabel.Text = @$"{_shufflerController.FinalSeed:X}";

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
            
            DisplaySeedHash();

			TabPane.SelectedTab = SeedOutput;
		}

        private void DisplaySeedHash()
        {
            const byte hashMask = 0b111111;
            
            
            ImagePanel.Controls.Clear();

            var badBgColor = Color.FromArgb(0x30, 0xa0, 0xac).ToArgb();
            var otherBadBgColor = Color.FromArgb(0x30, 0xa0, 0x78).ToArgb();
            var newBgColor = Color.FromArgb(0x08, 0x19, 0xad);
            var eventDefines = _shufflerController.GetEventWrites().Split('\n');
            var customRng = uint.Parse(eventDefines.First(line => line.Contains("customRNG")).Split(' ')[2][2..], 
                NumberStyles.HexNumber);
            var seed = uint.Parse(eventDefines.First(line => line.Contains("seedHashed")).Split(' ')[2][2..],
                NumberStyles.HexNumber);
            var settings = uint.Parse(eventDefines.First(line => line.Contains("settingHash")).Split(' ')[2][2..],
                NumberStyles.HexNumber);

            using var stream = Assembly.GetAssembly(typeof(ShufflerController))?.GetManifestResourceStream("RandomizerCore.Resources.hashicons.png");
            var hashImageList = new Bitmap(stream);

            var image = new PictureBox
            {
                Width = 384,
                Height = 48,
                Margin = Padding.Empty
            };
            var hashImage = new Bitmap(384, 48);
            
            for (var imageNum = 0; imageNum < 8; ++imageNum)
            {

                var imageIndex = imageNum switch
                {
                    0 => (seed >> 24) & hashMask,
                    1 => (seed >> 16) & hashMask,
                    2 => (seed >> 8) & hashMask,
                    3 => seed & hashMask,
                    4 => (customRng >> 8) & hashMask,
                    5 => 64U,
                    6 => (settings >> 8) & hashMask,
                    7 => (settings >> 16) & hashMask
                };

                var k = 16 * (int)imageIndex;
                var l = 16 * imageNum;
                for (var i = 0; i < 16; ++i)
                {
                    var i3 = i * 3;
                    for (var j = 0; j < 16; ++j)
                    {
                        var color = hashImageList.GetPixel(j, i + k);
                        var argb = color.ToArgb();
                        if (argb == badBgColor || argb == otherBadBgColor)
                            color = newBgColor;
                        AddColorToImage3X(hashImage, color, j + l, i3);
                        AddColorToImage3X(hashImage, color, j + l, i3 + 1);
                        AddColorToImage3X(hashImage, color, j + l, i3 + 2);
                    }
                }
            }

            image.Image = hashImage;
            ImagePanel.Controls.Add(image);
        }

        private static void AddColorToImage3X(Bitmap baseImage, Color targetColor, int x, int y)
        {
            x *= 3;
            baseImage.SetPixel(x, y, targetColor);
            baseImage.SetPixel(x + 1, y, targetColor);
            baseImage.SetPixel(x + 2, y, targetColor);
        }
	}
}
