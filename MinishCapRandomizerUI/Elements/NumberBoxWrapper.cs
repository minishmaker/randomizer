using MinishCapRandomizerUI.DrawConstants;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Elements;

public class NumberBoxWrapper : WrapperBase
{
    private const int DefaultBottomMargin = 10;
    private const int TextWidth = 225;
    private const int TextHeight = 15;
    private const int NumberBoxWidth = 130;
    private const int NumberBoxHeight = 23;
    private const int NumberBoxAlign = -2;
    private new const int ElementWidth = TextWidth + NumberBoxWidth + WidthMargin;
    
    private Label? _label;
    private TextBox? _textBox;
    private LogicNumberBox _numberBox;

    public NumberBoxWrapper(LogicNumberBox numberBox) : base(DefaultBottomMargin, ElementWidth)
    {
        _numberBox = numberBox;
    }

    public override List<Control> GetControls(int initialX, int initialY)
    {
        
        if (_label != null && _textBox != null)
            return new List<Control> { _label, _textBox };

        _label = new Label
        {
            AutoEllipsis = Constants.LabelsAndCheckboxesUseAutoEllipsis,
            AutoSize = false,
            Name = _numberBox.Name,
            Text = _numberBox.NiceName,
            Location = new Point(initialX, initialY),
            Height = TextHeight,
            Width = TextWidth,
        };

        _textBox = new TextBox
        {
            AutoSize = false,
            Name = _numberBox.Name,
            Text = _numberBox.Value,
            Location = new Point(initialX + TextWidth + WidthMargin, initialY + NumberBoxAlign),
            Height = NumberBoxHeight,
            Width = NumberBoxWidth,
            //SelectedText = _numberBox.Selection, we will include default values!
        };
        
        _textBox.TextChanged += (object sender, EventArgs e) =>
        {			
            if (byte.TryParse(_textBox.Text, out var val))
            {
                _numberBox.Value = val.ToString();
            }
            else
            {
                var start = _textBox.SelectionStart;
                _textBox.Text = _numberBox.Value;
                _textBox.Select(start - 1, 0);
            }
        };
        
        return new List<Control> { _label, _textBox };
    }
}