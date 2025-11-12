using Avalonia.Controls;
using MinishCapRandomizerUI.Avalonia.Elements;
using MinishCapRandomizerUI.Avalonia.DrawConstants;
using RandomizerCore.Randomizer.Logic.Options;
using System;

namespace MinishCapRandomizerUI.Avalonia.Elements;

public class NumberBoxWrapper : WrapperBase, ILogicOptionObserver
{
    private const int TextWidth = 180;
    private const int NumberBoxWidth = 90;
    private TextBlock? _label;
    private TextBox? _textBox;
    private readonly LogicNumberBox _numberBox;

    public NumberBoxWrapper(LogicNumberBox numberBox) : base(TextWidth + NumberBoxWidth + Constants.WidthMargin, 20, numberBox.SettingGroup, numberBox.SettingPage)
    {
        _numberBox = numberBox;
        _numberBox.RegisterObserver(this);
    }

    public bool IsFigurineRelated => _numberBox.Name.StartsWith("FIGURINE", StringComparison.OrdinalIgnoreCase)
                                      || _numberBox.NiceName.Contains("Figurine", StringComparison.OrdinalIgnoreCase);

    public override IList<Control> BuildControls(int initialX, int initialY)
    {
        if (_label != null && _textBox != null) return new List<Control>{ _label, _textBox };
        _label = new TextBlock{ Text = _numberBox.NiceName + ":", Width = TextWidth };
        if (!string.IsNullOrWhiteSpace(_numberBox.DescriptionText))
            ToolTip.SetTip(_label, _numberBox.DescriptionText);
        _textBox = new TextBox{ Text = _numberBox.Value.ToString(), Width = NumberBoxWidth };
        if (!string.IsNullOrWhiteSpace(_numberBox.DescriptionText))
            ToolTip.SetTip(_textBox, _numberBox.DescriptionText);
        _textBox.PropertyChanged += (_, e) =>
        {
            if (e.Property == TextBox.TextProperty)
            {
                if (string.IsNullOrWhiteSpace(_textBox.Text))
                {
                    _numberBox.Value = _numberBox.DefaultValue;
                    _textBox.Text = _numberBox.DefaultValue.ToString();
                    _numberBox.NotifyChildren();
                    return;
                }
                if (byte.TryParse(_textBox.Text, out var val))
                {
                    if (val < _numberBox.MinValue) val = _numberBox.MinValue;
                    if (val > _numberBox.MaxValue) val = _numberBox.MaxValue;
                    _numberBox.Value = val;
                    _textBox.Text = _numberBox.Value.ToString();
                    _numberBox.NotifyChildren();
                }
                else
                {
                    _textBox.Text = _numberBox.Value.ToString();
                }
            }
        };
        return new List<Control>{ _label, _textBox };
    }

    public void NotifyObserver()
    {
        if (_textBox != null) _textBox.Text = _numberBox.Value.ToString();
    }
}
