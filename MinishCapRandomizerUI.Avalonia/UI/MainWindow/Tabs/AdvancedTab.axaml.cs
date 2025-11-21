using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MinishCapRandomizerUI.Avalonia.UI.MainWindow.Tabs;

public partial class AdvancedTab : UserControl
{
    public AdvancedTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
