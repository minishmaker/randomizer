using System.Globalization;

namespace MinishCapRandomizerUI.UI.MainWindow;

public class MinishCapRandomizerUICommonImplementations
{
    
}


/// <summary>
/// Shared implementations of functions, will call into specific implementations based on various flags
/// </summary>
partial class MinishCapRandomizerUI
{
    private void Randomize_Click(object sender, EventArgs e)
    {
        if (UseCustomYAML.Checked || UseMysterySettings.Checked || UseMysteryCosmetics.Checked)
            RandomizeWithYamlShuffler();
        else
            RandomizeWithBaseShuffler();
    }
}
