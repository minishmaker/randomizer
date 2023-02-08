using MinishCapRandomizerUI.DrawConstants;

namespace MinishCapRandomizerUI.Elements;

public abstract class WrapperBase
{
    public int ElementWidth { get; }
    public int ElementHeight { get; }
    public string SettingGrouping { get; }
    public string Page { get; }

    protected WrapperBase(int elementWidth, int elementHeight, string settingGrouping, string page)
    {
        ElementWidth = (int)(elementWidth*Constants.SpecialScaling);
        ElementHeight = (int)(elementHeight*Constants.SpecialScaling);
        SettingGrouping = settingGrouping;
        Page = page;
    }

    public abstract List<Control> GetControls(int initialX, int initialY);
}
