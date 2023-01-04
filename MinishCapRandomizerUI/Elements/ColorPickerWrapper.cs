using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Elements;

public class ColorPickerWrapper : WrapperBase
{
    private const int DefaultBottomMargin = 15; // figure out later
    private LogicColorPicker _colorPicker;
    private new const int ElementWidth = 0; // figure out later
    private new const int ElementHeight = DefaultBottomMargin; // figure out later

    public ColorPickerWrapper(LogicColorPicker colorPicker) : base(ElementWidth, ElementHeight, colorPicker.SettingGroup, colorPicker.SettingPage)
    {
        _colorPicker = colorPicker;
    }

    public override List<Control> GetControls(int initialX, int initialY)
    {
        return new List<Control>(); //Not implemented yet, will do soon(tm)
    }
}