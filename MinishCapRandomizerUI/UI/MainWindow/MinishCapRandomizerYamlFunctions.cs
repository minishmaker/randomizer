using System.Globalization;
using System.Text.RegularExpressions;
using RandomizerCore.Controllers;

namespace MinishCapRandomizerUI.UI.MainWindow;

/// <summary>
/// This is just here so you can actually edit this file in the code editor without going through a bunch of designer crap
/// </summary>
public class MinishCapRandomizerYamlFunctions
{
    
}

/// <summary>
/// Functions and implementations for YAML features that use functions specific to the YAML controller
/// </summary>
partial class MinishCapRandomizerUI
{
    private YamlController _yamlController;

    private void InitializeYamlUi()
    {
        _yamlController = new YamlController();
    }
    
    // Can't catch all cases, but at least ensures it doesn't contain illegal characters for files on any operating system
    private static bool IsValidPresetName(string? name)
    {
        if (string.IsNullOrEmpty(name) || name != name.Trim())
            return false;
        var match = Regex.Match(name, "[\\x00-\\x1F<>:\"/\\\\|?*]");
        return !match.Success;
    }

    private void UpdateSeedInfoPageYaml(string settingsString, string cosmeticsString)
    {
        if (_yamlController.IsUsingLogicYaml())
            SettingNameLabel.Text = _yamlController.GetLogicYamlName();
        else
        {
            SettingNameLabel.Text = _recentSettingsPreset != null &&
                                    _yamlController.GetSelectedOptions().OnlyLogic().GetHash() == _recentSettingsPresetHash ? _recentSettingsPreset : "Custom";
        }
        
        if (_yamlController.IsUsingCosmeticsYaml())
            CosmeticNameLabel.Text = _yamlController.GetCosmeticsYamlName();
        else
        {
            CosmeticNameLabel.Text = _recentCosmeticsPreset != null &&
                                     _yamlController.GetSelectedOptions().OnlyCosmetic().GetHash() == _recentCosmeticsPresetHash ? _recentCosmeticsPreset : "Custom";
        }
        
        _outputSettingsString = settingsString;
        _outputCosmeticsString = cosmeticsString;
        _outputUsedYAML = true;
        SettingHashLabel.Text = _yamlController.IsUsingLogicYaml() ? "Settings string is not shown when using mystery settings" : settingsString;
        CosmeticStringLabel.Text = _yamlController.IsUsingCosmeticsYaml() ? "Cosmetics string is not shown when using mystery cosmetics" : cosmeticsString;
    }

    private string GetFilenameYamlShuffler()
    {
        return _yamlController.GetSeedFilename(!_yamlController.IsUsingLogicYaml() && _recentSettingsPreset != null &&
                                               _yamlController.GetSelectedOptions().OnlyLogic().GetHash() == _recentSettingsPresetHash ? _recentSettingsPreset : "Custom",
            !_yamlController.IsUsingCosmeticsYaml() && _recentCosmeticsPreset != null &&
            _yamlController.GetSelectedOptions().OnlyCosmetic().GetHash() == _recentCosmeticsPresetHash ? _recentCosmeticsPreset : "Custom");
    }

    private void RandomizeWithYamlShuffler()
    {
        if (!ulong.TryParse(Seed.Text, NumberStyles.HexNumber, default, out var seed))
        {
            DisplayAlert(@"Invalid Seed Provided!", @"Invalid Seed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!int.TryParse(RandomizationAttempts.Text, out var retryAttempts))
            DisplayAlert(@"Invalid randomization attempts! Defaulting to 1.", @"Invalid Retry Attempts",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

        _configuration.MaximumRandomizationRetryCount = retryAttempts;

        //TODO: This isn't the greatest solution to fix the issue with settings not syncing, come up with a better one later
        _yamlController.LoadSettingsFromSettingString(_shufflerController.GetSelectedSettingsString());
        _yamlController.LoadCosmeticsFromCosmeticsString(_shufflerController.GetSelectedCosmeticsString());

        _yamlController.SetRandomizationSeed(seed);
        if (UseCustomYAML.Checked)
            _yamlController.LoadLocations(UseCustomLogic.Checked ? LogicFilePath.Text : "", YAMLPath.Text, YAMLPath.Text, true);
        else
            _yamlController.LoadLocations(UseCustomLogic.Checked ? LogicFilePath.Text : "",
                UseMysterySettings.Checked ? $"{_presetPath}Mystery Settings{Path.DirectorySeparatorChar}{SettingsWeights.SelectedItem}.yaml" : "",
                UseMysteryCosmetics.Checked ? $"{_presetPath}Mystery Cosmetics{Path.DirectorySeparatorChar}{CosmeticsWeights.SelectedItem}.yaml" : "", false);
        
        var result = _yamlController.Randomize(retryAttempts, UseSphereBasedShuffler.Checked);
        
        if (result)
        {
            _randomizedRomCreated = result.WasSuccessful;

            DisplayAndUpdateSeedInfoPage();
        }
        else
            DisplayConditionalAlertFromShufflerResult(result, "You shouldn't be seeing this, but if you are it means something weird happened. Please report to the dev team.", "You Shouldn't See This", "Failed to generate ROM!", "Failed to Generate ROM");

    }

	private void LoadSettingSample_Click(object sender, EventArgs e)
	{
        var presets = _settingPresets.SettingsWeights;

		if (presets.All(preset => preset != (string)SettingsWeights.SelectedItem))
		{
			DisplayAlert("No weights preset matching the specified name could be found! Make sure you select a valid weights preset.", "Failed to Load Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

        var result = _yamlController.LoadLogicSettingsFromYaml($"{_presetPath}Mystery Settings{Path.DirectorySeparatorChar}{SettingsWeights.SelectedItem}.yaml");

        if (result)
        {
            _shufflerController.LoadSettingsFromSettingString(_yamlController.GetSelectedSettingsString());
            _recentSettingsPreset = (string)SettingsWeights.SelectedItem;
            _recentSettingsPresetHash = _yamlController.GetSelectedOptions().OnlyLogic().GetHash();
        }

		DisplayConditionalAlertFromShufflerResult(result,
			"Random settings loaded successfully!", "Settings Loaded", "Failed to load Settings preset!", "Failed to Load Settings");
	}

	private void LoadCosmeticSample_Click(object sender, EventArgs e)
	{
        var presets = _settingPresets.CosmeticsWeights;

		if (presets.All(preset => preset != (string)CosmeticsWeights.SelectedItem))
		{
			DisplayAlert("No weights preset matching the specified name could be found! Make sure you select a valid weights preset.", "Failed to Load Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

        var result = _yamlController.LoadCosmeticsFromYaml($"{_presetPath}Mystery Cosmetics{Path.DirectorySeparatorChar}{CosmeticsWeights.SelectedItem}.yaml");

        if (result)
        {
            _shufflerController.LoadCosmeticsFromCosmeticsString(_yamlController.GetSelectedCosmeticsString());
            _recentCosmeticsPreset = (string)CosmeticsWeights.SelectedItem;
            _recentCosmeticsPresetHash = _yamlController.GetSelectedOptions().OnlyCosmetic().GetHash();
        }

		DisplayConditionalAlertFromShufflerResult(result,
			"Random cosmetics loaded successfully!", "Cosmetics Loaded", "Failed to load cosmetics preset!", "Failed to Load Cosmetics");
	}
}
