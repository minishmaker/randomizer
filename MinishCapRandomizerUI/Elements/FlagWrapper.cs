using MinishCapRandomizerUI.DrawConstants;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Elements;

public class FlagWrapper : WrapperBase, ILogicOptionObserver
{
    private const int DefaultBottomMargin = 6;
    private const int Width = 240;
    private const int Height = 19;
    private new const int ElementHeight = Height + DefaultBottomMargin;

    private CheckBox? _checkBox;
    
    private LogicFlag _flag;

    public FlagWrapper(LogicFlag flag) : base(Width, ElementHeight, flag.SettingGroup, flag.SettingPage)
    {
        _flag = flag;
        _flag.RegisterObserver(this);
    }
    
    public override List<Control> GetControls(int initialX, int initialY)
    {
        if (_checkBox != null)
            return new List<Control> { _checkBox };

        _checkBox = new CheckBox
        {
            AutoEllipsis = Constants.LabelsAndCheckboxesUseAutoEllipsis,
            AutoSize = false,
            Name = _flag.Name,
            Location = new Point(initialX, initialY),
            Height = (int)(Height*Constants.SpecialScaling),
            Width = (int)(Width*Constants.SpecialScaling),
            Checked = _flag.Active,
            Text = _flag.NiceName,
            UseMnemonic = Constants.UseMnemonic,
        };

        if (!string.IsNullOrEmpty(_flag.DescriptionText.Trim()))
        {
            var tip = new ToolTip();
            tip.UseFading = true;
            tip.AutoPopDelay = Constants.TooltipDisplayLengthMs;
            tip.InitialDelay = Constants.TooltipInitialShowDelayMs;
            tip.ReshowDelay = Constants.TooltipRepeatDelayMs;
            tip.ShowAlways = true;
            tip.SetToolTip(_checkBox, _flag.DescriptionText);
        }

        _checkBox.CheckedChanged += (object? sender, EventArgs e) =>
        {
            _flag.Active = _checkBox.Checked;
        };
        
        return new List<Control> { _checkBox };
    }

	public void NotifyObserver()
	{
        _checkBox!.Checked = _flag.Active;
	}
}
