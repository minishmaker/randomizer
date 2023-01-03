using MinishCapRandomizerUI.DrawConstants;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Elements;

public class FlagWrapper : WrapperBase
{
    private const int DefaultBottomMargin = 6;
    private const int Width = 240;
    private const int Height = 19;

    private CheckBox? _checkBox;
    
    private LogicFlag _flag;

    public FlagWrapper(LogicFlag flag) : base(DefaultBottomMargin, Width)
    {
        _flag = flag;
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
            Height = Height,
            Width = Width,
            Checked = _flag.Active,
        };

        _checkBox.CheckedChanged += (object sender, EventArgs e) =>
        {
            _flag.Active = _checkBox.Checked;
        };
        
        return new List<Control> { _checkBox };
    }
}