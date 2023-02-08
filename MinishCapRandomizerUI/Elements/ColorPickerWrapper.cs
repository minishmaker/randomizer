using MinishCapRandomizerUI.DrawConstants;
using RandomizerCore.Randomizer.Logic.Options;
using RandomizerCore.Utilities.Models;

namespace MinishCapRandomizerUI.Elements;

public class ColorPickerWrapper : WrapperBase, ILogicOptionObserver
{
    private const int DefaultBottomMargin = 11; 
    private const int CheckboxWidth = 155;
    private const int CheckboxHeight = 19;
    private const int TextWidth = 85;
    private const int TextHeight = 19;
    private const int ButtonWidth = 130;
    private const int ButtonHeight = 23;
    private const int PictureBoxWidth = 60;
    private const int PictureBoxHeight = 23;
    private const int TextAlign = 1;
    private const int ButtonAndPictureBoxAlign = -3;
    private const string CheckboxText = "Use True Random Color";
    private const string SelectColorText = "Select Custom Color";
    private const string SelectRandomColorText = "Pick Random Color";
    private const string UseDefaultColorText = "Use Default Color";
    private const string ColorPreviewText = "Color Preview:";

    private new static readonly int ElementWidth = CheckboxWidth + TextWidth + PictureBoxWidth + 3 * ButtonWidth + 5 * Constants.WidthMargin; // one row
    private new const int ElementHeight = CheckboxHeight + DefaultBottomMargin;

    private CheckBox? _checkBox;
    private Button? _selectColorButton;
    private Button? _selectRandomColorButton;
    private Button? _useDefaultColorButton;
    private Label? _label;
    private PictureBox? _colorPreview;
    private LogicColorPicker _colorPicker;

    public ColorPickerWrapper(LogicColorPicker colorPicker) : base(ElementWidth, ElementHeight, colorPicker.SettingGroup, colorPicker.SettingPage)
    {
        _colorPicker = colorPicker;
        _colorPicker.RegisterObserver(this);
    }

    public override List<Control> GetControls(int initialX, int initialY)
    {
        if (_checkBox != null && _selectColorButton != null && _selectRandomColorButton != null &&
            _useDefaultColorButton != null && _label != null && _colorPreview != null)
        {
            return new List<Control>
            {
                _checkBox, _selectColorButton, _selectRandomColorButton, _useDefaultColorButton, _label, _colorPreview
            };
        }

        _checkBox = new CheckBox
        {
            AutoEllipsis = Constants.LabelsAndCheckboxesUseAutoEllipsis,
            AutoSize = false,
            Checked = false,
            Name = "Checkbox",
            Location = new Point(initialX, initialY),
            Height = (int)(CheckboxHeight*Constants.SpecialScaling),
            Width = (int)(CheckboxWidth*Constants.SpecialScaling),
            Text = CheckboxText,
            UseMnemonic = Constants.UseMnemonic,
        };

        var nextElementX = initialX + (int)((CheckboxWidth + Constants.WidthMargin)*Constants.SpecialScaling);

        _selectColorButton = BuildButton((sender, args) => SelectColor(), 
            "SelectColorButton", SelectColorText, ref nextElementX, initialY + ButtonAndPictureBoxAlign); 

        _selectRandomColorButton = BuildButton((sender, args) => SelectRandomColor(), 
            "RandomColorButton", SelectRandomColorText, ref nextElementX, initialY + ButtonAndPictureBoxAlign);

        _useDefaultColorButton = BuildButton((sender, args) => SelectDefaultColor(), 
            "DefaultColorButton", UseDefaultColorText, ref nextElementX, initialY + ButtonAndPictureBoxAlign);
        
        _label = new Label
        {
            AutoEllipsis = Constants.LabelsAndCheckboxesUseAutoEllipsis,
            AutoSize = false,
            Name = "ColorPreviewLabel",
            Location = new Point(nextElementX, initialY + TextAlign),
            Height = (int)(TextHeight*Constants.SpecialScaling),
            Width = (int)(TextWidth*Constants.SpecialScaling),
            Text = ColorPreviewText,
            UseMnemonic = Constants.UseMnemonic,
        };
        
        nextElementX += (int)((TextWidth + Constants.WidthMargin)*Constants.SpecialScaling);

        _colorPreview = new PictureBox
        {
            AutoSize = false,
            Name = "ColorPreview",
            Location = new Point(nextElementX, initialY + ButtonAndPictureBoxAlign),
            Height = (int)(PictureBoxHeight*Constants.SpecialScaling),
            Width = (int)(PictureBoxWidth*Constants.SpecialScaling),
            BackColor = _colorPicker.DefinedColor,
            BorderStyle = Constants.CategoryBorderStyle,
        };
        
        _checkBox.CheckedChanged += (sender, e) =>
        {
            _colorPicker.UseRandomColor = _checkBox.Checked;
            _selectColorButton.Enabled = !_checkBox.Checked;
            _selectRandomColorButton.Enabled = !_checkBox.Checked;
            _useDefaultColorButton.Enabled = !_checkBox.Checked;
        };

        return new List<Control>
        {
            _checkBox, _selectColorButton, _selectRandomColorButton, _useDefaultColorButton, _label, _colorPreview
        };
    }

    private Button BuildButton(
        EventHandler onClickEvent, 
        string name, 
        string text, 
        ref int xPosition, 
        int yPosition)
    {
        var button = new Button
        {
            AutoEllipsis = Constants.LabelsAndCheckboxesUseAutoEllipsis,
            AutoSize = false,
            Enabled = true,
            Name = name,
            Location = new Point(xPosition, yPosition),
            Height = (int)(ButtonHeight*Constants.SpecialScaling),
            Width = (int)(ButtonWidth*Constants.SpecialScaling),
            Text = text,
            BackColor = Constants.DefaultButtonBackgroundColor,
            UseMnemonic = Constants.UseMnemonic,
        };

        button.Click += onClickEvent;
        
        xPosition += (int)((ButtonWidth + Constants.WidthMargin)*Constants.SpecialScaling);

        return button;
    }

    private void SelectColor()
    {
        var colorPicker = new ColorDialog
        {
            Color = _colorPicker.DefinedColor,
            FullOpen = true
        };

        if (colorPicker.ShowDialog() != DialogResult.OK) return;
        _colorPicker.DefinedColor = new GBAColor(colorPicker.Color).ToColor();
        UpdateColorPreview();
    }

    private void SelectRandomColor()
    {
        _colorPicker.PickRandomColor();
        UpdateColorPreview();
    }

    private void SelectDefaultColor()
    {
        _colorPicker.DefinedColor = _colorPicker.BaseColor;
        UpdateColorPreview();
    }

    private void UpdateColorPreview()
    {
        if (_colorPreview == null) return;

        _colorPreview.BackColor = _colorPicker.DefinedColor;
    }

	public void NotifyObserver()
	{
        UpdateColorPreview();
        _checkBox.Checked = _colorPicker.UseRandomColor;
	}
}
