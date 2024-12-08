using MinishCapRandomizerUI.DrawConstants;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Elements;

public class NumberBoxWrapper : WrapperBase, ILogicOptionObserver
{
    private const int DefaultBottomMargin = 15;
    private const int TextWidth = 225;
    private const int TextHeight = 19;
    private const int NumberBoxWidth = 130;
    private const int NumberBoxHeight = 23;
    private const int NumberBoxAlign = -2;
    private new static readonly int ElementWidth = TextWidth + NumberBoxWidth + Constants.WidthMargin;
    private new const int ElementHeight = NumberBoxHeight + DefaultBottomMargin - 8;
    
    private Label? _label;
    private NumericUpDown? _upDownBox;
    private LogicNumberBox _numberBox;

    public NumberBoxWrapper(LogicNumberBox numberBox) : base(ElementWidth, ElementHeight, numberBox.SettingGroup, numberBox.SettingPage)
    {
        _numberBox = numberBox;
        _numberBox.RegisterObserver(this);
    }

    public override List<Control> GetControls(int initialX, int initialY)
    {
        
        if (_label != null && _upDownBox != null)
            return new List<Control> { _label, _upDownBox };

        _label = new Label
        {
            AutoEllipsis = Constants.LabelsAndCheckboxesUseAutoEllipsis,
            AutoSize = false,
            Name = _numberBox.Name,
            Text = _numberBox.NiceName + ":",
            Location = new Point(initialX, initialY),
            Height = (int)(TextHeight*Constants.SpecialScaling),
            Width = (int)(TextWidth*Constants.SpecialScaling),
            TextAlign = ContentAlignment.MiddleRight,
            UseMnemonic = Constants.UseMnemonic,
        };

        _upDownBox = new NumericUpDown
        {
            AutoSize = false,
            Name = _numberBox.Name,
            Text = _numberBox.Value,
            Minimum = _numberBox.MinValue,
            Maximum = _numberBox.MaxValue,
            Location = new Point(initialX + (int)((TextWidth + Constants.WidthMargin)*Constants.SpecialScaling), initialY + NumberBoxAlign),
            Height = (int)(NumberBoxHeight*Constants.SpecialScaling),
            Width = (int)(NumberBoxWidth*Constants.SpecialScaling),
        };        
        
        if (!string.IsNullOrEmpty(_numberBox.DescriptionText.Trim()))
        {
            var tip = new ToolTip();
            tip.UseFading = true;
            tip.AutoPopDelay = Constants.TooltipDisplayLengthMs;
            tip.InitialDelay = Constants.TooltipInitialShowDelayMs;
            tip.ReshowDelay = Constants.TooltipRepeatDelayMs;
            tip.ShowAlways = true;
            tip.SetToolTip(_label, _numberBox.DescriptionText);
            tip.SetToolTip(_upDownBox, _numberBox.DescriptionText);
        }

        _upDownBox.TextChanged += (object sender, EventArgs e) =>
        {
            if (_upDownBox.Text.Length == 0)
            {
                _numberBox.Value = _upDownBox.Text = _numberBox.DefaultValue.ToString();
                return;
            }

            if (byte.TryParse(_upDownBox.Text, out var val))
            {
                if (val > _numberBox.MaxValue)
                    _numberBox.Value = _upDownBox.Text = _numberBox.MaxValue.ToString();
                else
                {
                    if (val < _numberBox.MinValue)
                        _numberBox.Value = _upDownBox.Text = _numberBox.MinValue.ToString();
                    else
                        _numberBox.Value = val.ToString();
                }
            }
            else
                _upDownBox.Text = _numberBox.Value;
        };

        return new List<Control> { _label, _upDownBox };
    }

	public void NotifyObserver()
	{
        _upDownBox.Text = _numberBox.Value.ToString();
	}
}
