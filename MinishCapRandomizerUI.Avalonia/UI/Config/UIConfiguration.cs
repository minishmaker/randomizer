namespace MinishCapRandomizerUI.Avalonia.UI.Config;

internal class UIConfiguration
{
    public string RomPath { get; set; } = "";
    public bool UseHendrusShuffler { get; set; }
    public int MaximumRandomizationRetryCount { get; set; } = 1;
    public string DefaultLoggerPath { get; set; } = "";
    public bool UseVerboseLogger { get; set; }
    public bool UseCustomLogic { get; set; }
    public string CustomLogicFilepath { get; set; } = "";
    public bool UseCustomPatch { get; set; }
    public string CustomPatchFilepath { get; set; } = "";
    public bool CheckForUpdatesOnStart { get; set; } = true;
    public bool UseCustomYAML { get; set; }
    public string CustomYAMLFilepath { get; set; } = "";
    public bool UseCompactUIOnStart { get; set; } = true;
}

