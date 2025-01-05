using MinishCapRandomizerUI.UI.Config;
using RandomizerCore.Random;

namespace MinishCapRandomizerUI.UI.MainWindow;

public class LandingPageUIImplementations
{
    
}

//TODO: RESX EVERYTHING (This is a lot of long busywork, not high priority)
partial class MinishCapRandomizerUI
{
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
        var seed = new SquaresRandomNumberGenerator().Next();
        Seed.Text = @$"{seed:X}";
        _shufflerController.SetRandomizationSeed(seed);
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
    
    private void LoadSettings_Click(object sender, EventArgs e)
    {
        DisplayConditionalAlertFromShufflerResult(_shufflerController.LoadSettingsFromSettingString(SettingString.Text), "Settings loaded successfully!", "Settings Loaded", "Failed to load Settings string!", "Failed to Load Settings");
    }

    private void GenerateSettings_Click(object sender, EventArgs e)
    {
        SettingString.Text = _shufflerController.GetSelectedSettingsString();
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
        CosmeticsString.Text = _shufflerController.GetSelectedCosmeticsString();
    }

    private void ResetDefaultCosmetics_Click(object sender, EventArgs e)
    {
        DisplayAlert("Are you sure you wish to load default cosmetics?", "Load Default Cosmetics", MessageBoxButtons.YesNo, MessageBoxIcon.Question, DialogResult.Yes,
            () => _shufflerController.LoadCosmeticsFromCosmeticsString(_defaultCosmetics));
    }

	private void LoadSettingPreset_Click(object sender, EventArgs e)
	{
        var presets = _settingPresets.SettingsPresets;

		if (presets.All(preset => preset.PresetName != (string?)SettingPresets.SelectedItem))
		{
			DisplayAlert("No preset matching the specified name could be found! Make sure you select a valid preset.", "Failed to Load Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

        var filename = presets.First(preset => preset.PresetName == (string?)SettingPresets.SelectedItem).Filename;
        var result = _shufflerController.LoadLogicSettingsFromYaml($"{_presetPath}Settings{Path.DirectorySeparatorChar}{filename}.yaml");

        if (result)
        {
            _recentSettingsPreset = (string?)SettingPresets.SelectedItem;
            _recentSettingsPresetHash = _shufflerController.GetSelectedOptions().OnlyLogic().GetHash();
        }

		DisplayConditionalAlertFromShufflerResult(result,
			"Settings loaded successfully!", "Settings Loaded", "Failed to load Settings preset!", "Failed to Load Settings");
	}

	private void SaveSettingPreset_Click(object sender, EventArgs e)
	{
		ShowInputDialog("Enter Setting Preset Name", "Enter the name you would like to use for the setting preset.\nNote: Preset names cannot contain <>:\"/\\|?* and two presets cannot have the same name, even ignoring case!", (name =>
		{
            name = name.Trim();

            var presets = _settingPresets.SettingsPresets;

            if (!IsValidPresetName(name))
            {
                DisplayAlert("Preset name not valid!", "Failed to Save Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (presets.Any(preset => string.Equals(preset.PresetName, name, StringComparison.CurrentCultureIgnoreCase)))
            {
                DisplayAlert("A setting preset with the specified name already exists!", "Failed to Save Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var max = presets.Count == 0 ? 0 : presets.Select(preset => preset.SortIndex).Max();
            var updatedName = name + (max == int.MaxValue ? "" : $"_{max + 1}");
            var result = _shufflerController.SaveSettingsAsYaml($"{_presetPath}Settings{Path.DirectorySeparatorChar}{updatedName}.yaml",
                name, _shufflerController.GetSelectedOptions().OnlyLogic());

            if (result)
            {
                presets.Add(new PresetFileInfo { Filename = updatedName, PresetName = name, SortIndex = max == int.MaxValue ? max : (max + 1) });
                AddItemToPresetsBox(SettingPresets, name);
                _recentSettingsPreset = name;
                _recentSettingsPresetHash = _shufflerController.GetSelectedOptions().OnlyLogic().GetHash();
                DisplayAlert("Preset saved successfully!", "Preset Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DisplayAlert("An error occured while trying to save the preset!", "Failed to Save Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}));
	}

	private void DeleteSettingPreset_Click(object sender, EventArgs e)
	{
		DisplayAlert($"Are you sure you wish to delete preset \"{SettingPresets.SelectedItem}\"? This action cannot be undone!", "Delete Preset", MessageBoxButtons.YesNo, MessageBoxIcon.Question, DialogResult.Yes,
			() =>
			{
                var presets = _settingPresets.SettingsPresets;

				if (presets.All(preset => preset.PresetName != (string?)SettingPresets.SelectedItem))
				{
					DisplayAlert("No preset matching the specified name could be found! Make sure you select a valid preset before deleting.", "Failed to Delete Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
                
                try
                {
                    var info = presets.First(preset => preset.PresetName == (string?)SettingPresets.SelectedItem);
                    presets.Remove(info);
                    File.Delete($"{_presetPath}Settings{Path.DirectorySeparatorChar}{info.Filename}.yaml");

					RemoveItemFromPresetsBox(SettingPresets, (string)SettingPresets.SelectedItem!);
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
        var presets = _settingPresets.CosmeticsPresets;

		if (presets.All(preset => preset.PresetName != (string?)CosmeticsPresets.SelectedItem))
		{
			DisplayAlert("No preset matching the specified name could be found! Make sure you select a valid preset.", "Failed to Load Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

        var filename = presets.First(preset => preset.PresetName == (string?)CosmeticsPresets.SelectedItem).Filename;
        var result = _shufflerController.LoadCosmeticsFromYaml($"{_presetPath}Cosmetics{Path.DirectorySeparatorChar}{filename}.yaml");

        if (result)
        {
            _recentCosmeticsPreset = (string?)CosmeticsPresets.SelectedItem;
            _recentCosmeticsPresetHash = _shufflerController.GetSelectedOptions().OnlyCosmetic().GetHash();
        }

		DisplayConditionalAlertFromShufflerResult(result,
			"Cosmetics loaded successfully!", "Cosmetics Loaded", "Failed to load cosmetics preset!", "Failed to Load Cosmetics");
	}

	private void SaveCosmeticPreset_Click(object sender, EventArgs e)
	{
		ShowInputDialog("Enter Cosmetic Preset Name", "Enter the name you would like to use for the cosmetics preset.\nNote: Preset names cannot contain <>:\"/\\|?* and two presets cannot have the same name, even ignoring case!", name =>
        {
            name = name.Trim();

            var presets = _settingPresets.CosmeticsPresets;

            if (!IsValidPresetName(name))
            {
                DisplayAlert("Preset name not valid!", "Failed to Save Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (presets.Any(preset => string.Equals(preset.PresetName, name, StringComparison.CurrentCultureIgnoreCase)))
            {
                DisplayAlert("A cosmetic preset with the specified name already exists!", "Failed to Save Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var max = presets.Count == 0 ? 0 : presets.Select(preset => preset.SortIndex).Max();
            var updatedName = name + (max == int.MaxValue ? "" : $"_{max + 1}");
            var result = _shufflerController.SaveSettingsAsYaml($"{_presetPath}Cosmetics{Path.DirectorySeparatorChar}{updatedName}.yaml",
                name, _shufflerController.GetSelectedOptions().OnlyCosmetic());

            if (result)
            {
                presets.Add(new PresetFileInfo { Filename = updatedName, PresetName = name, SortIndex = max == int.MaxValue ? max : (max + 1) });
                AddItemToPresetsBox(CosmeticsPresets, name);
                _recentCosmeticsPreset = name;
                _recentCosmeticsPresetHash = _shufflerController.GetSelectedOptions().OnlyCosmetic().GetHash();
                DisplayAlert("Preset saved successfully!", "Preset Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DisplayAlert("An error occured while trying to save the preset!", "Failed to Save Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        });
	}

	private void DeleteCosmeticPreset_Click(object sender, EventArgs e)
	{
		DisplayAlert($"Are you sure you wish to delete preset \"{CosmeticsPresets.SelectedItem}\"? This action cannot be undone!", "Delete Preset", MessageBoxButtons.YesNo, MessageBoxIcon.Question, DialogResult.Yes,
			() =>
			{
                var presets = _settingPresets.CosmeticsPresets;

				if (presets.All(preset => preset.PresetName != (string?)CosmeticsPresets.SelectedItem))
				{
					DisplayAlert("No preset matching the specified name could be found! Make sure you select a valid preset before deleting.", "Failed to Delete Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				try
				{
                    var info = presets.First(preset => preset.PresetName == (string?)CosmeticsPresets.SelectedItem);
                    presets.Remove(info);
                    File.Delete($"{_presetPath}Cosmetics{Path.DirectorySeparatorChar}{info.Filename}.yaml");

					RemoveItemFromPresetsBox(CosmeticsPresets, (string)CosmeticsPresets.SelectedItem!);
					DisplayAlert("Preset deleted successfully!", "Preset deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch
				{
					DisplayAlert("An error occured while trying to delete the preset!", "Failed to delete Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			});
	}

    //TODO: Split this up into varying functions, this is gross
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
}
