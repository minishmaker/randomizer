namespace MinishCapRandomizerUI.UI.Config;

public record PresetFileInfo
{
    public required string Filename { get; set; }

    public required string PresetName { get; set; }

    public required int SortIndex { get; set; } // smaller numbers get listed near the top, gaps and duplicate values are allowed
}
