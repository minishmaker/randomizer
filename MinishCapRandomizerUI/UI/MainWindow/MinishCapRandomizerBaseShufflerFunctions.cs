using System.Globalization;
using RandomizerCore.Controllers;

namespace MinishCapRandomizerUI.UI.MainWindow;

public class MinishCapRandomizerBaseShufflerFunctions
{
    
}

public partial class MinishCapRandomizerUI
{
    private ShufflerController _shufflerController;
    
    private void InitializeBaseUi()
    {
        _shufflerController = new ShufflerController();
    }
    
    private void RandomizeWithBaseShuffler()
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
        _shufflerController.SetRandomizationSeed(seed);
        _shufflerController.LoadLocations(UseCustomLogic.Checked ? LogicFilePath.Text : "");
        
        var result = _shufflerController.Randomize(retryAttempts, UseSphereBasedShuffler.Checked);
        if (result)
        {
            _randomizedRomCreated = result.WasSuccessful;

            DisplayAndUpdateSeedInfoPage();
        }
        else
            DisplayConditionalAlertFromShufflerResult(result, "You shouldn't be seeing this, but if you are it means something weird happened. Please report to the dev team.", "You Shouldn't See This", "Failed to generate ROM!", "Failed to Generate ROM");

    }

    private void UpdateSeedInfoPageBase(string settingsString, string cosmeticsString)
    {
        SettingNameLabel.Text = _recentSettingsPreset != null && _shufflerController.GetSelectedOptions().OnlyLogic().GetHash() == _recentSettingsPresetHash ? _recentSettingsPreset : "Custom";
        CosmeticNameLabel.Text = _recentCosmeticsPreset != null && _shufflerController.GetSelectedOptions().OnlyCosmetic().GetHash() == _recentCosmeticsPresetHash ? _recentCosmeticsPreset : "Custom";
        _outputSettingsString = settingsString;
        _outputCosmeticsString = cosmeticsString;
        _outputUsedYAML = false;
        SettingHashLabel.Text = settingsString;
        CosmeticStringLabel.Text = cosmeticsString;
        SettingHashLabel.Visible = true;
        CosmeticStringLabel.Visible = true;
    }

    private string GetFilenameBaseShuffler() => _shufflerController.SeedFilename;
}
