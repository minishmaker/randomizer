using MinishCapRandomizerUI.DrawConstants;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Elements;

public class DropdownWrapper : WrapperBase
{
    private const int DefaultBottomMargin = 15;
    private const int TextWidth = 225;
    private const int TextHeight = 15;
    private const int DropdownWidth = 130;
    private const int DropdownHeight = 23;
    private const int DropdownAlign = -2;
    private new const int ElementWidth = TextWidth + DropdownWidth + WidthMargin;

    private Label? _label;
    private ComboBox? _comboBox;
    private LogicDropdown _dropdown;

    public DropdownWrapper(LogicDropdown dropdown) : base(DefaultBottomMargin, ElementWidth)
    {
        _dropdown = dropdown;
    }

    public override List<Control> GetControls(int initialX, int initialY)
    {
        if (_label != null && _comboBox != null)
            return new List<Control> { _label, _comboBox };

        _label = new Label
        {
            AutoEllipsis = Constants.LabelsAndCheckboxesUseAutoEllipsis,
            AutoSize = false,
            Name = _dropdown.Name,
            Text = _dropdown.NiceName,
            Location = new Point(initialX, initialY),
            Height = TextHeight,
            Width = TextWidth,
        };

        _comboBox = new ComboBox
        {
            AutoSize = false,
            Name = _dropdown.Name,
            Text = _dropdown.NiceName,
            Location = new Point(initialX + TextWidth + WidthMargin, initialY + DropdownAlign),
            Height = DropdownHeight,
            Width = DropdownWidth,
            SelectedText = _dropdown.Selection,
            DataSource = _dropdown.Selections.Keys.ToList(),
        };
        
        _comboBox.SelectedIndexChanged += (object sender, EventArgs e) =>
        {
            _dropdown.Selection = (string)_comboBox.SelectedValue;
        };
        
        _comboBox.SelectedValueChanged += (object sender, EventArgs e) =>
        {
            _dropdown.Selection = (string)_comboBox.SelectedValue;
        };
        
        return new List<Control> { _label, _comboBox };
    }
}