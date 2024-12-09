namespace MinishCapRandomizerUI.UI.MainWindow;

public class SeedPageUIImplementations
{
    
}

public partial class MinishCapRandomizerUI
{
    private void SaveSpoiler_Click(object sender, EventArgs e)
    {
        DisplaySaveDialog(@"Text File|*.txt|All Files|*.*", @"Save Spoiler Log", 
            $"{GetSeedFilename()}-Spoiler.txt", DialogResult.OK, filename =>
                DisplayConditionalAlertFromShufflerResult(_previousShuffler.SaveSpoiler(filename), "Spoiler Saved Successfully!", "Spoiler Saved", "Failed to save spoiler!", "Spoiler Save Failed"));
    }

    private void SavePatch_Click(object sender, EventArgs e)
    {
        DisplaySaveDialog(@"BPS Patch|*.bps|All Files|*.*", @"Save Patch", 
            $"{GetSeedFilename()}-Patch.bps", DialogResult.OK, filename =>
                DisplayConditionalAlertFromShufflerResult(_previousShuffler.CreatePatch(filename, UseCustomPatch.Checked ? RomBuildfilePath.Text : ""), "Patch Saved Successfully!", "Patch Saved", "Failed to save patch!", "Patch Save Failed"));
    }

    private void SaveRom_Click(object sender, EventArgs e)
    {
        DisplaySaveDialog(@"GBA ROM|*.gba|All Files|*.*", @"Save ROM", 
            $"{GetSeedFilename()}-ROM.gba", DialogResult.OK, filename => 
                DisplayConditionalAlertFromShufflerResult(_previousShuffler.SaveAndPatchRom(filename, UseCustomPatch.Checked ? RomBuildfilePath.Text : ""), "ROM Saved Successfully!", "ROM Saved", "Failed to save ROM!", "ROM Save Failed"));
    }

    private void CopySettingsHashToClipboard_Click(object sender, EventArgs e)
    {
        Clipboard.SetText(_outputSettingsString!);
    }

    private void CopyCosmeticsHashToClipboard_Click(object sender, EventArgs e)
    {
        Clipboard.SetText(_outputCosmeticsString!);
    }

    private void CopyHashToClipboard_Click(object sender, EventArgs e)
    {
        if (RomHashPanel.Controls.Count > 0)
            Clipboard.SetImage(((PictureBox)RomHashPanel.Controls[0]).Image);
    }
}
