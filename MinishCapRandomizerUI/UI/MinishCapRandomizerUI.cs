using MinishCapRandomizerUI.Elements;
using RandomizerCore.Controllers;

namespace MinishCapRandomizerUI.UI;

public partial class MinishCapRandomizerUI : Form
{
    private ShufflerController _shufflerController;

    private bool _customLogicFileLoaded = false;
    private bool _customPatchLoaded = false;
    
    public MinishCapRandomizerUI()
    {
        InitializeComponent();
        _shufflerController = new ShufflerController();
        _shufflerController.LoadLogicFile();
        UpdateUIWithLogicOptions();
    }

    private void UpdateUIWithLogicOptions()
    {
        for (var i = TabPane.TabPages.Count - 1; i >= 2; --i)
        {
            TabPane.TabPages.RemoveAt(i);
        }
        
        var options = _shufflerController.GetLogicOptions();
        var wrappedOptions = WrappedLogicOptionFactory.BuildGenericWrappedLogicOptions(options);
        var pages = wrappedOptions.GroupBy(option => option.Page);

        foreach (var page in pages)
        {
            TabPane.TabPages.Add(UIGenerator.BuildUIPage(page.ToList(), page.Key));
        }
    }

    private void BrowseRom_Click(object sender, EventArgs e)
    {
        var openDialog = new OpenFileDialog
        {
            Filter = "GBA ROMs|*.gba|All Files|*.*",
            Title = "Select TMC ROM"
        };
        var result = openDialog.ShowDialog();

        if (result != DialogResult.OK) return;

        _shufflerController.LoadRom(openDialog.FileName);
        RomPath.Text = openDialog.FileName;
    }

    private void RandomSeed_Click(object sender, EventArgs e)
    {
        Seed.Text = $"{new Random().Next()}";
    }

    private void BrowseCustomLogicFile_Click(object sender, EventArgs e)
    {
        var openDialog = new OpenFileDialog
        {
            Filter = "Logic File|*.logic|All Files|*.*",
            Title = "Select Logic File"
        };
        var result = openDialog.ShowDialog();

        if (result != DialogResult.OK) return;

        _shufflerController.LoadLogicFile(openDialog.FileName);
        LogicFilePath.Text = openDialog.FileName;
        UpdateUIWithLogicOptions();
    }

    private void BrowseCustomPatch_Click(object sender, EventArgs e)
    {
        var openDialog = new OpenFileDialog
        {
            Filter = "Event File|*.event|All Files|*.*",
            Title = "Select ROM Buildfile"
        };
        var result = openDialog.ShowDialog();

        if (result != DialogResult.OK) return;

        RomBuildfilePath.Text = openDialog.FileName;
    }

    private void UseCustomLogic_CheckedChanged(object sender, EventArgs e)
    {
        BrowseCustomLogicFile.Enabled = UseCustomLogic.Checked;

        if (!UseCustomLogic.Checked && _customLogicFileLoaded)
        {
            _shufflerController.LoadLogicFile();
            UpdateUIWithLogicOptions();
            _customLogicFileLoaded = false;
        } 
        else if (UseCustomLogic.Checked && LogicFilePath.Text.Length > 0)
        {
            _shufflerController.LoadLogicFile(LogicFilePath.Text);
            _customLogicFileLoaded = true;
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
            return; //Will display an error box later
        }

        _shufflerController.SetRandomizationSeed(seed);
        _shufflerController.Randomize();
        _shufflerController.SaveAndPatchRom($"{Directory.GetCurrentDirectory()}/MinishRandomizer-ROM.gba",
            UseCustomPatch.Checked && RomBuildfilePath.Text.Length > 0 ? RomBuildfilePath.Text : null);
        _shufflerController.SaveSpoiler($"{Directory.GetCurrentDirectory()}/MinishRandomizer-Spoiler.txt");
    }
}