using System.Linq;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using MinishCapRandomizerUI.Avalonia.DrawConstants;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Avalonia.Elements;

public class DropdownWrapper : WrapperBase, ILogicOptionObserver
{
    private const int TextWidth = 160; // reduced further for compactness
    private const int DropdownWidth = 0; // use stretch
    private const int DropdownHeight = 23;
    private static readonly int ElementWidthInternal = TextWidth + 160 + Constants.WidthMargin;
    private const int ElementHeightInternal = DropdownHeight;

    private TextBlock? _label;
    private ComboBox? _comboBox;
    private readonly LogicDropdown _dropdown;

    public DropdownWrapper(LogicDropdown dropdown) : base(ElementWidthInternal, ElementHeightInternal, dropdown.SettingGroup, dropdown.SettingPage)
    {
        _dropdown = dropdown;
        _dropdown.RegisterObserver(this);
    }

    public override IList<Control> BuildControls(int initialX, int initialY)
    {
        if (_label != null && _comboBox != null)
            return new List<Control> { _label, _comboBox };

        _label = new TextBlock { Text = _dropdown.NiceName + ":", VerticalAlignment = VerticalAlignment.Center, Width = TextWidth, TextWrapping = TextWrapping.Wrap };
        if (!string.IsNullOrWhiteSpace(_dropdown.DescriptionText))
            ToolTip.SetTip(_label, _dropdown.DescriptionText);

        _comboBox = new ComboBox{ MinWidth = 160, HorizontalAlignment = HorizontalAlignment.Stretch };
        var items = _dropdown.SelectionOptionNames.ToList();
        _comboBox.ItemsSource = items;
        var selectedName = _dropdown.OptionsToNames[_dropdown.Selection];
        _comboBox.SelectedItem = selectedName;
        if (_comboBox.SelectedItem == null && items.Count > 0)
            _comboBox.SelectedIndex = 0;
        if (!string.IsNullOrWhiteSpace(_dropdown.DescriptionText))
            ToolTip.SetTip(_comboBox, _dropdown.DescriptionText);
        _comboBox.SelectionChanged += (_, __) =>
        {
            if (_comboBox.SelectedItem is string s)
            {
                _dropdown.Selection = _dropdown.NamesToOptions[s];
                _dropdown.NotifyChildren();
            }
        };
        return new List<Control>{ _label, _comboBox };
    }

    public void NotifyObserver()
    {
        if (_comboBox == null) return;
        var selectionName = _dropdown.OptionsToNames[_dropdown.Selection];
        _comboBox.SelectedItem = selectionName;
    }
}
