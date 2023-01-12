using MinishCapRandomizerUI.Elements;
using RandomizerCore.Controllers;

namespace MinishCapRandomizerUI.UI;

public sealed partial class MinishCapRandomizerUI : Form
{
    private ShufflerController _shufflerController;

    private bool _customLogicFileLoaded = false;
    private bool _randomizedRomCreated = false;
    private bool _isApplyPatchMode = true;
    
    public MinishCapRandomizerUI()
    {
        InitializeComponent();
        TabPane.TabPages.Remove(SeedOutput);
        var seed = new Random().Next();
        Seed.Text = $@"{seed}";
        _shufflerController = new ShufflerController();
        _shufflerController.LoadLogicFile();
        _shufflerController.SetRandomizationSeed(seed);
        Text = $@"{_shufflerController.AppName} {_shufflerController.VersionName} {_shufflerController.RevName}";
        UpdateUIWithLogicOptions();
    }

    private void UpdateUIWithLogicOptions()
    {
        for (var i = TabPane.TabPages.Count - 1; i >= 1; --i)
        {
            TabPane.TabPages.RemoveAt(i);
        }
        
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
            TabPane.TabPages.Insert(1, SeedOutput);

        InputSeedLabel.Text = Seed.Text;
        OutputSeedLabel.Text = @$"{_shufflerController.GetFinalSeed()}";

        SettingNameLabel.Text = @"Not Implemented Yet =(";
        CosmeticNameLabel.Text = @"Not Implemented Yet =(";
        
        SettingHashLabel.Text = @"Not Implemented";
        CosmeticStringLabel.Text = @"Not Implemented";

        // SettingHashLabel.Text = _shufflerController.GetSettingString();
        // CosmeticStringLabel.Text = _shufflerController.GetCosmeticsString();

        TabPane.SelectedTab = SeedOutput;
    }

    private void BrowseRom_Click(object sender, EventArgs e)
    {
        DisplayOpenDialog(@"GBA ROMs|*.gba|All Files|*.*", @"Select TMC ROM", DialogResult.OK, filename =>
        {
            if (_shufflerController.LoadRom(filename))
                RomPath.Text = filename;
            else 
                DisplayAlert("Failed to load ROM!", "Failed to Load ROM", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (_shufflerController.LoadLogicFile(filename))
            {
                LogicFilePath.Text = filename;
                UpdateUIWithLogicOptions();
                return;
            }
            
            DisplayAlert("Failed to load Logic File! Please ensure the Logic File is correct.", 
                "Failed to Load Logic File", MessageBoxButtons.OK, MessageBoxIcon.Error);
        });
    }

    private void BrowseCustomPatch_Click(object sender, EventArgs e)
    {
        DisplayOpenDialog(@"Event File|*.event|All Files|*.*", @"Select ROM Buildfile", DialogResult.OK, filename =>
        {        
            RomBuildfilePath.Text = filename;
        });
    }

    private void UseCustomLogic_CheckedChanged(object sender, EventArgs e)
    {
        BrowseCustomLogicFile.Enabled = UseCustomLogic.Checked;

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

        _shufflerController.SetRandomizationSeed(seed);
        _shufflerController.LoadLocations(UseCustomLogic.Checked && LogicFilePath.Text.Length > 0 ? LogicFilePath.Text : null);
        if (_shufflerController.Randomize(retryAttempts))
        {
            _randomizedRomCreated = true;
            
            DisplayAndUpdateSeedInfoPage();
        }
        else
        {
            DisplayAlert(@"Failed to generate ROM! Please check log output for more information.", 
                @"Failed to Generate ROM", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void writeAndFlushLoggerToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DisplayAlert(_shufflerController.PublishLogs(), "Publish Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
        _shufflerController.FlushLogger();
    }
    
    private void SaveSpoiler_Click(object sender, EventArgs e)
    {
        DisplaySaveDialog(@"Text File|*.txt|All Files|*.*", @"Save Spoiler Log", 
            $"{_shufflerController.SeedFilename()}-Spoiler.txt", DialogResult.OK, filename =>
        {                        
            if (_shufflerController.SaveSpoiler(filename))
                DisplayAlert(@"Spoiler Saved Successfully!", @"Spoiler Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                DisplayAlert(@"Failed to save spoiler!", @"Spoiler Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        });
    }

    private void SavePatch_Click(object sender, EventArgs e)
    {
        DisplaySaveDialog(@"BPS Patch|*.bps|All Files|*.*", @"Save Patch", 
            $"{_shufflerController.SeedFilename()}-Patch.bps", DialogResult.OK, filename =>
            {                        
                if (_shufflerController.CreatePatch(filename, RomBuildfilePath.Text))
                    DisplayAlert(@"Patch Saved Successfully!", @"Patch Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    DisplayAlert(@"Failed to save patch!", @"Patch Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
    }

    private void SaveRom_Click(object sender, EventArgs e)
    {
        DisplaySaveDialog(@"GBA ROM|*.gba|All Files|*.*", @"Save ROM", 
            $"{_shufflerController.SeedFilename()}-ROM.gba", DialogResult.OK, filename =>
            {
                if (_shufflerController.SaveAndPatchRom(filename, RomBuildfilePath.Text))
                    DisplayAlert(@"ROM Saved Successfully!", @"ROM Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    DisplayAlert(@"Failed to save ROM!", @"ROM Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
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
                if (_isApplyPatchMode
                        ? _shufflerController.PatchRom(filename, BpsPatchAndPatchedRomPath.Text)
                        : _shufflerController.SaveRomPatch(filename, BpsPatchAndPatchedRomPath.Text))
                {
                    DisplayAlert(
                        _isApplyPatchMode ? applyModeSuccessText : generateModeSuccessText, 
                        _isApplyPatchMode ? applyModeSuccessTitle : generateModeSuccessTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    DisplayAlert(
                        _isApplyPatchMode ? applyModeFailureText : generateModeFailureText, 
                        _isApplyPatchMode ? applyModeFailureTitle : generateModeFailureTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
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
        
    }

    private void GenerateSettings_Click(object sender, EventArgs e)
    {

    }

    private void ResetDefaultSettings_Click(object sender, EventArgs e)
    {

    }

    private void LoadCosmetics_Click(object sender, EventArgs e)
    {

    }

    private void GenerateCosmetics_Click(object sender, EventArgs e)
    {

    }

    private void ResetDefaultCosmetics_Click(object sender, EventArgs e)
    {

    }

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
}