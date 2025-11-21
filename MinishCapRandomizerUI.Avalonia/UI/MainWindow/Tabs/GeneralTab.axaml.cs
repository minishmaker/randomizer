using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MinishCapRandomizerUI.Avalonia.UI.MainWindow.Tabs;

public partial class GeneralTab : UserControl
{
    public GeneralTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
