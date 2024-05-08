using System.Globalization;
using System.Text.RegularExpressions;
using MinishCapRandomizerUI.UI.Config;
using RandomizerCore.Controllers;
using RandomizerCore.Controllers.Models;
using RandomizerCore.Random;

namespace MinishCapRandomizerUI.UI.MainWindow;

public sealed partial class MinishCapRandomizerUI : Form
{
    private UIConfiguration _configuration;
    private bool _customLogicFileLoaded = false;
    private bool _randomizedRomCreated = false;
    private bool _isApplyPatchMode = true;
    private SettingPresets _settingPresets;
    private string _defaultSettings;
    private string _defaultCosmetics;
    private string? _recentSettingsPreset = null;
    private string? _recentCosmeticsPreset = null;
    private uint _recentSettingsPresetHash;
    private uint _recentCosmeticsPresetHash;
    private string? _outputSettingsString = null;
    private string? _outputCosmeticsString = null;
    private bool _outputUsedYAML = false;
    private ControllerBase _previousShuffler;

    private readonly string _presetPath =
        $@"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}{Path.DirectorySeparatorChar}Presets{Path.DirectorySeparatorChar}";

#pragma warning disable CS8618
    public MinishCapRandomizerUI()
#pragma warning restore CS8618
    {
        InitializeComponent();
        InitializeYamlUi();
        InitializeBaseUi();
        Text = $@"{_shufflerController.AppName} {_shufflerController.VersionName} {_shufflerController.RevName}";
        FormClosing += SaveConfig;
        LoadConfig();
        InitializeUi();
        UpdateUIWithLogicOptions();
        LoadPresets();
        LoadTooltips();
    }

    private void writeAndFlushLoggerToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DisplayAlert(_shufflerController.PublishLogs(), "Publish Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
        _shufflerController.FlushLogger();
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
        logAllTransactionsToolStripMenuItem.Checked = !logAllTransactionsToolStripMenuItem.Checked;
        _configuration.UseVerboseLogger = logAllTransactionsToolStripMenuItem.Checked;
        _shufflerController.SetLoggerVerbosity(_configuration.UseVerboseLogger);
    }

    private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        var aboutPage = new About.About();
        aboutPage.Show();
    }

    private async void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var noUpdates = await CheckForUpdates();
        if (noUpdates is { wasSuccessful: true, hasUpdates: false })
            DisplayAlert("You have the latest version!", "Up to Date", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void checkForUpdatesOnStartToolStripMenuItem_Click(object sender, EventArgs e)
    {
        checkForUpdatesOnStartToolStripMenuItem.Checked = !checkForUpdatesOnStartToolStripMenuItem.Checked;
        _configuration.CheckForUpdatesOnStart = checkForUpdatesOnStartToolStripMenuItem.Checked;
    }

    private void savePresetYAMLMenuItem_Click(object sender, EventArgs e)
    {
        DisplaySaveDialog("YAML Files|*.yaml;*.yml|All Files|*.*", "Choose where to save Preset YAML file", "Preset.yaml", DialogResult.OK, (filepath) => DisplayConditionalAlertFromShufflerResult(_shufflerController.ExportYaml(filepath, false), "Preset YAML file saved successfully!", "Saved YAML File", "Failed to save Preset YAML file!", "Failed to Save"));
    }

    private void saveMysteryYAMLMenuItem_Click(object sender, EventArgs e)
    {
        DisplaySaveDialog("YAML Files|*.yaml;*.yml|All Files|*.*", "Choose where to save Mystery YAML file", "Mystery.yaml", DialogResult.OK, (filepath) => DisplayConditionalAlertFromShufflerResult(_shufflerController.ExportYaml(filepath, true), "Mystery YAML file saved successfully!", "Saved YAML File", "Failed to save Mystery YAML file!", "Failed to Save"));
    }
}
