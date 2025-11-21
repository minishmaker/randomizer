using Avalonia.Controls;
using Avalonia.Media;
using MinishCapRandomizerUI.Avalonia.DrawConstants;
using MinishCapRandomizerUI.Avalonia.Elements;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Avalonia.Elements;

public class FlagWrapper : WrapperBase, ILogicOptionObserver
{
    private const int WidthInternal = 200;
    private const int HeightInternal = 18;
    private CheckBox? _checkBox;
    private readonly LogicFlag _flag;

    public FlagWrapper(LogicFlag flag) : base(WidthInternal, HeightInternal, flag.SettingGroup, flag.SettingPage)
    {
        _flag = flag;
        _flag.RegisterObserver(this);
    }

    public bool IsFigurineHuntFlag => _flag.Name == "FIGURINE_HUNT" || _flag.NiceName == "Figurine Hunt";

    public override IList<Control> BuildControls(int initialX, int initialY)
    {
        if (_checkBox != null) return new List<Control>{ _checkBox };
        var label = new TextBlock{ Text = _flag.NiceName, TextWrapping = TextWrapping.Wrap, MaxWidth = 220 };
        _checkBox = new CheckBox { Content = label, IsChecked = _flag.Active };
        if (!string.IsNullOrWhiteSpace(_flag.DescriptionText))
            ToolTip.SetTip(_checkBox, _flag.DescriptionText);
        _checkBox.IsCheckedChanged += (_, __) => { _flag.Active = _checkBox.IsChecked == true; _flag.NotifyChildren(); };
        return new List<Control>{ _checkBox };
    }

    public void NotifyObserver()
    {
        if (_checkBox != null) _checkBox.IsChecked = _flag.Active;
    }
}
