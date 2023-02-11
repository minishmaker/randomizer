using MinishCapRandomizerUI.UI.Config;
using Newtonsoft.Json;
using RandomizerCore.Controllers;

namespace MinishCapRandomizerUI.UI;

public sealed partial class MinishCapRandomizerUI : Form
{
	private ShufflerController _shufflerController;

	private UIConfiguration _configuration;
	private bool _customLogicFileLoaded = false;
	private bool _randomizedRomCreated = false;
	private bool _isApplyPatchMode = true;
	private SettingPresets _settingPresets;
	private string _defaultSettings;
	private string _defaultCosmetics;

#pragma warning disable CS8618
	public MinishCapRandomizerUI()
#pragma warning restore CS8618
	{
		InitializeComponent();
		_shufflerController = new ShufflerController();
		Text = $@"{_shufflerController.AppName} {_shufflerController.VersionName} {_shufflerController.RevName}";
		FormClosing += SaveConfig;
		LoadConfig();
		InitializeUi();
		UpdateUIWithLogicOptions();
		LoadPresets();
		LoadTooltips();
	}

	private void BrowseRom_Click(object sender, EventArgs e)
	{
		DisplayOpenDialog(@"GBA ROMs|*.gba|All Files|*.*", @"Select TMC ROM", DialogResult.OK, filename =>
		{
			var result = _shufflerController.LoadRom(filename);

			if (result.WasSuccessful)
			{
				RomPath.Text = filename;
				_configuration.RomPath = filename;
			}
			else
				DisplayConditionalAlertFromShufflerResult(result, "You shouldn't be seeing this, but if you are it means something weird happened. Please report to the dev team.", "You Shouldn't See This", "Failed to load ROM!", "Failed to Load ROM");

		});
	}

	private void RandomSeed_Click(object sender, EventArgs e)
	{
		var seed = new Random().Next();
		Seed.Text = @$"{seed}";
		_shufflerController.SetRandomizationSeed(seed);
	}

	private void BrowseCustomLogicFile_Click(object sender, EventArgs e)
	{
		DisplayOpenDialog(@"Logic Files|*.logic|All Files|*.*", @"Select Logic File", DialogResult.OK, filename =>
		{
			var result = _shufflerController.LoadLogicFile(filename);

            if (result.WasSuccessful)
            {
                _configuration.CustomLogicFilepath = filename;
                LogicFilePath.Text = filename;
                UpdateUIWithLogicOptions();
            }
            else
            {
                DisplayConditionalAlertFromShufflerResult(result, "You shouldn't be seeing this, but if you are it means something weird happened. Please report to the dev team.", "You Shouldn't See This", "Failed to load Logic File!", "Failed to Load Logic File");
                _shufflerController.LoadLogicFile();
                UpdateUIWithLogicOptions();
            }
		});
	}

	private void BrowseCustomPatch_Click(object sender, EventArgs e)
	{
		DisplayOpenDialog(@"Event File|*.event|All Files|*.*", @"Select ROM Buildfile", DialogResult.OK, filename =>
		{        
			RomBuildfilePath.Text = filename;
			_configuration.CustomPatchFilepath = filename;
		});
	}

	private void UseCustomLogic_CheckedChanged(object sender, EventArgs e)
	{
		BrowseCustomLogicFile.Enabled = UseCustomLogic.Checked;
		_configuration.UseCustomLogic = UseCustomLogic.Checked;

		switch (UseCustomLogic.Checked)
		{
			case false when _customLogicFileLoaded:
				_shufflerController.LoadLogicFile();
				UpdateUIWithLogicOptions();
				_customLogicFileLoaded = false;
				break;
			case true when LogicFilePath.Text.Length > 0:
				_shufflerController.LoadLogicFile(LogicFilePath.Text);
				UpdateUIWithLogicOptions();
				_customLogicFileLoaded = true;
				break;
		}
	}

	private void UseCustomPatch_CheckedChanged(object sender, EventArgs e)
	{
		BrowseCustomPatch.Enabled = UseCustomPatch.Checked;
		_configuration.UseCustomPatch = UseCustomPatch.Checked;
	}

	private void Randomize_Click(object sender, EventArgs e)
	{
		if (!int.TryParse(Seed.Text, out var seed))
		{
			DisplayAlert(@"Invalid Seed Provided!", @"Invalid Seed", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		if (!int.TryParse(RandomizationAttempts.Text, out var retryAttempts))
			DisplayAlert(@"Invalid randomization attempts! Defaulting to 1.", @"Invalid Retry Attempts",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);

		_configuration.MaximumRandomizationRetryCount = retryAttempts;
		_shufflerController.SetRandomizationSeed(seed);
		_shufflerController.LoadLocations(UseCustomLogic.Checked ? LogicFilePath.Text : "");
		var result = _shufflerController.Randomize(retryAttempts, UseSphereBasedShuffler.Checked);
		if (result.WasSuccessful)
		{
			_randomizedRomCreated = result.WasSuccessful;

			DisplayAndUpdateSeedInfoPage();
		}
		else
			DisplayConditionalAlertFromShufflerResult(result, "You shouldn't be seeing this, but if you are it means something weird happened. Please report to the dev team.", "You Shouldn't See This", "Failed to generate ROM!", "Failed to Generate ROM");
	}

	private void writeAndFlushLoggerToolStripMenuItem_Click(object sender, EventArgs e)
	{
		DisplayAlert(_shufflerController.PublishLogs(), "Publish Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
		_shufflerController.FlushLogger();
	}
	
	private void SaveSpoiler_Click(object sender, EventArgs e)
	{
		DisplaySaveDialog(@"Text File|*.txt|All Files|*.*", @"Save Spoiler Log", 
			$"{_shufflerController.SeedFilename}-Spoiler.txt", DialogResult.OK, filename =>
				DisplayConditionalAlertFromShufflerResult(_shufflerController.SaveSpoiler(filename), "Spoiler Saved Successfully!", "Spoiler Saved", "Failed to save spoiler!", "Spoiler Save Failed"));
	}

	private void SavePatch_Click(object sender, EventArgs e)
	{
		DisplaySaveDialog(@"BPS Patch|*.bps|All Files|*.*", @"Save Patch", 
			$"{_shufflerController.SeedFilename}-Patch.bps", DialogResult.OK, filename =>
				DisplayConditionalAlertFromShufflerResult(_shufflerController.CreatePatch(filename, UseCustomPatch.Checked ? RomBuildfilePath.Text : ""), "Patch Saved Successfully!", "Patch Saved", "Failed to save patch!", "Patch Save Failed"));
	}

	private void SaveRom_Click(object sender, EventArgs e)
	{
		DisplaySaveDialog(@"GBA ROM|*.gba|All Files|*.*", @"Save ROM", 
			$"{_shufflerController.SeedFilename}-ROM.gba", DialogResult.OK, filename => 
				DisplayConditionalAlertFromShufflerResult(_shufflerController.SaveAndPatchRom(filename, UseCustomPatch.Checked ? RomBuildfilePath.Text : ""), "ROM Saved Successfully!", "ROM Saved", "Failed to save ROM!", "ROM Save Failed"));
	}

	private void ApplyPatchButton_CheckedChanged(object sender, EventArgs e)
	{
		const string applyModeString = "BPS Patch File Path:";
		const string generateModeString = "Patched ROM Path:";
		const string applyModeButtonText = "Patch ROM";
		const string generateModeButtonText = "Generate BPS Patch";

		_isApplyPatchMode = ApplyPatchButton.Checked;

		BpsPatchAndPatchedRomPath.Text = "";

		BpsPatchAndPatchedRomLabel.Text = _isApplyPatchMode ? applyModeString : generateModeString;
		PatchRomAndGenPatch.Text = _isApplyPatchMode ? applyModeButtonText : generateModeButtonText;
	}

	private void BrowsePatchOrRom_Click(object sender, EventArgs e)
	{
		const string applyModeFilter = "BPS Patch|*.bps|All Files|*.*";
		const string generateModeFilter = "GBA ROMs|*.gba|All Files|*.*";
		const string applyModeTitle = "BPS Patch";
		const string generateModeTitle = "Patched ROM";

		DisplayOpenDialog(
			_isApplyPatchMode ? applyModeFilter : generateModeFilter, 
			_isApplyPatchMode ? applyModeTitle : generateModeTitle,
			DialogResult.OK,
			filename =>
			{
				BpsPatchAndPatchedRomPath.Text = filename;
			});
	}

	private void PatchRomAndGenPatch_Click(object sender, EventArgs e)
	{
		const string applyModeAlertText = "You must select a patch file before patching the ROM!";
		const string generateModeAlertText = "You must select a patched ROM before generating a patch!";
		const string applyModeAlertTitle = "Missing BPS Patch";
		const string generateModeAlertTitle = "Missing Patched ROM";
		
		const string applyModeSuccessText = "ROM Patched Successfully!";
		const string generateModeSuccessText = "Patch Generated Successfully!";
		const string applyModeSuccessTitle = "ROM Generated";
		const string generateModeSuccessTitle = "Patch Generated";
		
		const string applyModeFailureText = "Failed to patch ROM!";
		const string generateModeFailureText = "Failed to generate patch!";
		const string applyModeFailureTitle = "ROM Patch Failed";
		const string generateModeFailureTitle = "Patch Generation Failed";
		
		const string generateModeFilter = "BPS Patch|*.bps|All Files|*.*";
		const string applyModeFilter = "GBA ROMs|*.gba|All Files|*.*";
		const string generateModeTitle = "BPS Patch";
		const string applyModeTitle = "Patched ROM";
		const string generateModeFilename = "Patch.bps";
		const string applyModeFilename = "Patched ROM.gba";
		
		if (string.IsNullOrEmpty(BpsPatchAndPatchedRomPath.Text))
		{
			DisplayAlert(
				_isApplyPatchMode ? applyModeAlertText : generateModeAlertText, 
				_isApplyPatchMode ? applyModeAlertTitle : generateModeAlertTitle,
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
			return;
		}
		
		DisplaySaveDialog(
			_isApplyPatchMode ? applyModeFilter : generateModeFilter, 
			_isApplyPatchMode ? applyModeTitle : generateModeTitle,
			_isApplyPatchMode ? applyModeFilename : generateModeFilename,
			DialogResult.OK,
			filename =>
			{
				var result = _isApplyPatchMode
						? _shufflerController.PatchRom(filename, BpsPatchAndPatchedRomPath.Text)
						: _shufflerController.SaveRomPatch(filename, BpsPatchAndPatchedRomPath.Text);

				DisplayConditionalAlertFromShufflerResult(result, 
					_isApplyPatchMode ? applyModeSuccessText : generateModeSuccessText, 
					_isApplyPatchMode ? applyModeSuccessTitle : generateModeSuccessTitle, 
					_isApplyPatchMode ? applyModeFailureText : generateModeFailureText, 
					_isApplyPatchMode ? applyModeFailureTitle : generateModeFailureTitle);
			});
	}

	private void CopySettingsHashToClipboard_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(SettingHashLabel.Text);
	}

	private void CopyCosmeticsHashToClipboard_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(CosmeticStringLabel.Text);
	}

	private void LoadSettings_Click(object sender, EventArgs e)
	{
		DisplayConditionalAlertFromShufflerResult(_shufflerController.LoadSettingsFromSettingString(SettingString.Text), "Settings loaded successfully!", "Settings Loaded", "Failed to load Settings string!", "Failed to Load Settings");
	}

	private void GenerateSettings_Click(object sender, EventArgs e)
	{
		SettingString.Text = _shufflerController.GetSettingsString();
	}

	private void ResetDefaultSettings_Click(object sender, EventArgs e)
	{
		DisplayAlert("Are you sure you wish to load default settings?", "Load Default Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question, DialogResult.Yes, 
			() =>_shufflerController.LoadSettingsFromSettingString(_defaultSettings));
	}

	private void LoadCosmetics_Click(object sender, EventArgs e)
	{
		DisplayConditionalAlertFromShufflerResult(_shufflerController.LoadCosmeticsFromCosmeticsString(CosmeticsString.Text), "Cosmetics loaded successfully!", "Cosmetics Loaded", "Failed to load cosmetics string!", "Failed to Load Cosmetics");
	}

	private void GenerateCosmetics_Click(object sender, EventArgs e)
	{
		CosmeticsString.Text = _shufflerController.GetCosmeticsString();
	}

	private void ResetDefaultCosmetics_Click(object sender, EventArgs e)
	{
		DisplayAlert("Are you sure you wish to load default cosmetics?", "Load Default Cosmetics", MessageBoxButtons.YesNo, MessageBoxIcon.Question, DialogResult.Yes,
			() => _shufflerController.LoadCosmeticsFromCosmeticsString(_defaultCosmetics));
	}

	private void exportDefaultLogicToolStripMenuItem_Click(object sender, EventArgs e)
	{
		DisplaySaveDialog("Logic Files|*.logic|Text Files|*.txt", "Choose where to save logic file", "default.logic", DialogResult.OK, (filename) => DisplayConditionalAlertFromShufflerResult(_shufflerController.ExportDefaultLogic(filename), "Logic file saved successfully!", "Saved Logic File", "Failed to save logic file!", "Failed to Save"));
	}

	private void setLoggerOutputPathToolStripMenuItem_Click(object sender, EventArgs e)
	{

		DisplaySaveDialog("Log Files|*.json", "Choose where to save logs to", "log.json", DialogResult.OK, (filename) =>
		{
			_shufflerController.SetLogOutputPath(filename);
			DisplayAlert("Log path saved successfully!", "Saved Log Path", MessageBoxButtons.OK, MessageBoxIcon.Information);
			_configuration.DefaultLoggerPath = filename;
		});
	}

	private void logAllTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		DisplayAlert("Due to an issue with log size exploding you cannot enable the verbose logger at this time. This will be fixed in the next release.", "Cannot Enable Verbose Logger", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		//logAllTransactionsToolStripMenuItem.Checked = !logAllTransactionsToolStripMenuItem.Checked;
		//_configuration.UseVerboseLogger = logAllTransactionsToolStripMenuItem.Checked;
		//_shufflerController.SetLoggerVerbosity(_configuration.UseVerboseLogger);
	}

	private void UseSphereBasedShuffler_CheckedChanged(object sender, EventArgs e)
	{
		_configuration.UseHendrusShuffler = UseSphereBasedShuffler.Checked;
	}

	private void useLoggerToolStripMenuItem_Click(object sender, EventArgs e)
	{
		DisplayAlert("Cannot disable logger. This is a testing build, we need the logger enabled at all times for error reporting. Don't worry, in the full release you will be able disable it, although we strongly suggest NOT doing that for when something inevitably breaks. If you encounter an error with the logger disabled, we will not be able to help you, because WE WON'T HAVE ANY LOGS because you turned it off like a dummy.", 
			"Cannot Disable Logger", MessageBoxButtons.OK, MessageBoxIcon.Warning);
	}

	private void LoadSettingPreset_Click(object sender, EventArgs e)
	{
		var logicChecksum = _shufflerController.GetSettingsChecksum();
		if (!_settingPresets.SettingsPresets.TryGetValue(logicChecksum, out var presets)) {
			DisplayAlert("Could not find any presets for the current logic file! Please try a different logic file.", "Failed to Load Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		if (!presets.Any(preset => preset.PresetName == (string)SettingPresets.SelectedItem))
		{
			DisplayAlert("No preset matching the specified name could be found! Make sure you select a valid preset.", "Failed to Load Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		DisplayConditionalAlertFromShufflerResult(_shufflerController.LoadSettingsFromSettingString(presets.First(preset => preset.PresetName == (string)SettingPresets.SelectedItem).PresetString), 
			"Settings loaded successfully!", "Settings Loaded", "Failed to load Settings string!", "Failed to Load Settings");
	}

	private void SaveSettingPreset_Click(object sender, EventArgs e)
	{
		ShowInputDialog("Enter Setting Preset Name", "Enter the name you would like to use for the setting preset.\nNote:  Preset names are case sensitive and two presets cannot have the same name!", (name =>
		{
			var logicChecksum = _shufflerController.GetSettingsChecksum();
			var preset = new Preset
			{
				PresetName = name,
				PresetString = _shufflerController.GetSettingsString(),
			};

			if (!_settingPresets.SettingsPresets.TryGetValue(logicChecksum, out var presets))
			{
				presets = new List<Preset> { preset };

				_settingPresets.SettingsPresets.Add(logicChecksum, presets);
			} 
			else
			{
				if (presets.Any(preset => preset.PresetName == name))
				{
					DisplayAlert("A setting preset with the specified name already exists!", "Failed to Save Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				presets.Add(preset);
			}

			try
			{
				var presetsAsJson = JsonConvert.SerializeObject(_settingPresets, Formatting.Indented);
				File.WriteAllText($"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}/SettingPresets.json", presetsAsJson);
				DisplayAlert("Preset saved successfully!", "Preset Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
				AddItemToPresetsBox(SettingPresets, preset.PresetName);
			}
			catch
			{
				DisplayAlert("An error occured while trying to save the preset!", "Failed to Save Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}));
	}

	private void DeleteSettingPreset_Click(object sender, EventArgs e)
	{
		DisplayAlert($"Are you sure you wish to delete preset {SettingPresets.SelectedItem}? This action cannot be undone!", "Delete Preset", MessageBoxButtons.YesNo, MessageBoxIcon.Question, DialogResult.Yes,
			() =>
			{
				var logicChecksum = _shufflerController.GetSettingsChecksum();

				if (!_settingPresets.SettingsPresets.TryGetValue(logicChecksum, out var presets))
				{
					DisplayAlert("Could not find any presets for the current logic file! Please try a different logic file.", "Failed to Delete Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (!presets.Any(preset => preset.PresetName == (string)SettingPresets.SelectedItem))
				{
					DisplayAlert("No preset matching the specified name could be found! Make sure you select a valid preset before deleting.", "Failed to Delete Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				try
				{
					presets.Remove(presets.First(preset => preset.PresetName == (string)SettingPresets.SelectedItem));
					var presetsAsJson = JsonConvert.SerializeObject(_settingPresets, Formatting.Indented);
					File.WriteAllText($"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}/SettingPresets.json", presetsAsJson);

					RemoveItemFromPresetsBox(SettingPresets, (string)SettingPresets.SelectedItem);
					DisplayAlert("Preset deleted successfully!", "Preset deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch
				{
					DisplayAlert("An error occured while trying to delete the preset!", "Failed to delete Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			});
	}

	private void LoadCosmeticPreset_Click(object sender, EventArgs e)
	{
		var logicChecksum = _shufflerController.GetCosmeticsChecksum();
		if (!_settingPresets.CosmeticsPresets.TryGetValue(logicChecksum, out var presets))
		{
			DisplayAlert("Could not find any presets for the current logic file! Please try a different logic file.", "Failed to Load Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		if (!presets.Any(preset => preset.PresetName == (string)CosmeticsPresets.SelectedItem))
		{
			DisplayAlert("No preset matching the specified name could be found! Make sure you select a valid preset.", "Failed to Load Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		DisplayConditionalAlertFromShufflerResult(_shufflerController.LoadCosmeticsFromCosmeticsString(presets.First(preset => preset.PresetName == (string)CosmeticsPresets.SelectedItem).PresetString),
			"Cosmetics loaded successfully!", "Cosmetics Loaded", "Failed to load cosmetics string!", "Failed to Load Cosmetics");
	}

	private void SaveCosmeticPreset_Click(object sender, EventArgs e)
	{
		ShowInputDialog("Enter Cosmetic Preset Name", "Enter the name you would like to use for the cosmetics preset.\nNote: Preset names are case sensitive and two presets cannot have the same name!", (name =>
		{
			var logicChecksum = _shufflerController.GetCosmeticsChecksum();
			var preset = new Preset
			{
				PresetName = name,
				PresetString = _shufflerController.GetCosmeticsString(),
			};

			if (!_settingPresets.CosmeticsPresets.TryGetValue(logicChecksum, out var presets))
			{
				presets = new List<Preset> { preset };

				_settingPresets.CosmeticsPresets.Add(logicChecksum, presets);
			}
			else
			{
				if (presets.Any(preset => preset.PresetName == name))
				{
					DisplayAlert("A cosmetic preset with the specified name already exists!", "Failed to Save Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				presets.Add(preset);
			}

			try
			{
				var presetsAsJson = JsonConvert.SerializeObject(_settingPresets, Formatting.Indented);
				File.WriteAllText($"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}/SettingPresets.json", presetsAsJson);
				AddItemToPresetsBox(CosmeticsPresets, preset.PresetName);
				DisplayAlert("Preset saved successfully!", "Preset Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch
			{
				DisplayAlert("An error occured while trying to save the preset!", "Failed to Save Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}));
	}

	private void DeleteCosmeticPreset_Click(object sender, EventArgs e)
	{
		DisplayAlert($"Are you sure you wish to delete preset {CosmeticsPresets.SelectedItem}? This action cannot be undone!", "Delete Preset", MessageBoxButtons.YesNo, MessageBoxIcon.Question, DialogResult.Yes,
			() =>
			{
				var logicChecksum = _shufflerController.GetCosmeticsChecksum();

				if (!_settingPresets.CosmeticsPresets.TryGetValue(logicChecksum, out var presets))
				{
					DisplayAlert("Could not find any presets for the current logic file! Please try a different logic file.", "Failed to Delete Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (!presets.Any(preset => preset.PresetName == (string)CosmeticsPresets.SelectedItem))
				{
					DisplayAlert("No preset matching the specified name could be found! Make sure you select a valid preset before deleting.", "Failed to Delete Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				try
				{
					presets.Remove(presets.First(preset => preset.PresetName == (string)CosmeticsPresets.SelectedItem));
					var presetsAsJson = JsonConvert.SerializeObject(_settingPresets, Formatting.Indented);
					File.WriteAllText($"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}/SettingPresets.json", presetsAsJson);

					RemoveItemFromPresetsBox(CosmeticsPresets, (string)CosmeticsPresets.SelectedItem);
					DisplayAlert("Preset deleted successfully!", "Preset deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch
				{
					DisplayAlert("An error occured while trying to delete the preset!", "Failed to delete Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			});
	}

	private static void AddItemToPresetsBox(ComboBox box, string presetName)
	{
		box.Items.Add(presetName);
		if (box.Items.Count == 1) 
			box.SelectedIndex = 0;
	}

	private static void RemoveItemFromPresetsBox(ComboBox box, string presetName)
	{
		var isRemovedPresetSelected = (string)box.SelectedItem == presetName;

		box.Items.Remove(presetName);
		box.Text = "";

		if (isRemovedPresetSelected)
		{
			var newIndex = box.SelectedIndex - 1 >= 0 ? box.SelectedIndex - 1 : 0;

			if (box.Items.Count > 0)
			{
				box.SelectedIndex = newIndex;
			}
		}
	}
}
