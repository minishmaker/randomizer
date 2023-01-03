namespace MinishCapRandomizerUI.Elements;

public abstract class WrapperBase
{
    protected const int WidthMargin = 10;
    
    public int BottomHeightMargin { get; }
    public int ElementWidth { get; }

    protected WrapperBase(int bottomHeightMargin, int elementWidth)
    {
        BottomHeightMargin = bottomHeightMargin;
        ElementWidth = elementWidth;
    }

    public abstract List<Control> GetControls(int initialX, int initialY);
}