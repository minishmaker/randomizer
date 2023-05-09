namespace MinishCapRandomizerUI.UI.MainWindow;

public class AdvancedPageUIImplementations
{
    
}

public partial class MinishCapRandomizerUI
{
    private void BrowseCustomLogicFile_Click(object sender, EventArgs e)
    {
        DisplayOpenDialog(@"Logic Files|*.logic|All Files|*.*", @"Select Logic File", DialogResult.OK, filename =>
        {
            var result = _shufflerController.LoadLogicFile(filename);
            _yamlController.LoadLogicFile(filename);

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
                _yamlController.LoadLogicFile();
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

    private void BrowseCustomYAML_Click(object sender, EventArgs e)
    {
        DisplayOpenDialog("YAML Files|*.yaml;*.yml|All Files|*.*", "Select YAML File", DialogResult.OK, filepath =>
        {
            YAMLPath.Text = filepath;
            _configuration.CustomYAMLFilepath = filepath;

            UpdateUIWithLogicOptions();
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
                _yamlController.LoadLogicFile();
                UpdateUIWithLogicOptions();
                _customLogicFileLoaded = false;
                break;
            case true when LogicFilePath.Text.Length > 0:
                _shufflerController.LoadLogicFile(LogicFilePath.Text);
                _yamlController.LoadLogicFile(LogicFilePath.Text);
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

    private void UseCustomYAML_CheckedChanged(object sender, EventArgs e)
    {
        BrowseCustomYAML.Enabled = UseCustomYAML.Checked;
        _configuration.UseCustomYAML = UseCustomYAML.Checked;
        if (!UseMysteryCosmetics.Checked && !UseMysterySettings.Checked)
            UpdateUIWithLogicOptions();
    }

    private void UseMysterySettings_CheckedChanged(object sender, EventArgs e)
    {
        if (!UseMysteryCosmetics.Checked && !UseCustomYAML.Checked)
            UpdateUIWithLogicOptions();
    }

    private void UseMysteryCosmetics_CheckedChanged(object sender, EventArgs e)
    {
        if (!UseMysterySettings.Checked && !UseCustomYAML.Checked)
            UpdateUIWithLogicOptions();
    }

    private void UseSphereBasedShuffler_CheckedChanged(object sender, EventArgs e)
    {
        _configuration.UseHendrusShuffler = UseSphereBasedShuffler.Checked;
    }
}
