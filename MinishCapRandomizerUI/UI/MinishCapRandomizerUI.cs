using MinishCapRandomizerUI.Elements;
using RandomizerCore.Controllers;

namespace MinishCapRandomizerUI.UI;

public partial class MinishCapRandomizerUI : Form
{
    private ShufflerController _shufflerController;

    private bool _customLogicFileLoaded = false;
    private bool _customPatchLoaded = false;
    private bool _randomizedRomCreated = false;
    
    public MinishCapRandomizerUI()
    {
        InitializeComponent();
        TabPane.TabPages.Remove(SeedOutput);
        Seed.Text = $@"{new Random().Next()}";
        _shufflerController = new ShufflerController();
        _shufflerController.LoadLogicFile();
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

    private void BrowseRom_Click(object sender, EventArgs e)
    {
        DisplayOpenDialog(@"GBA ROMs|*.gba|All Files|*.*", @"Select TMC ROM", DialogResult.OK, filename =>
        {
            if (_shufflerController.LoadRom(filename))
                RomPath.Text = filename;
            else 
                DisplayAlert("Failed to load ROM!", 
                    "Failed to Load ROM", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        _shufflerController.SetRandomizationSeed(seed);
        _shufflerController.LoadLocations(UseCustomLogic.Checked && LogicFilePath.Text.Length > 0 ? LogicFilePath.Text : null);
        if (_shufflerController.Randomize())
        {
            _randomizedRomCreated = true;
            if (!TabPane.TabPages.Contains(SeedOutput))
                TabPane.TabPages.Insert(1, SeedOutput);

            TabPane.SelectedTab = SeedOutput;
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
}