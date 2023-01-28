namespace MinishCapRandomizerUI.DrawConstants;

public static class Constants
{
    public static int TopRowAboveSpacing => 15;
    public static int FirstElementInRowX => 10;
    public static int WidthMargin => 10;

    public static int CategorySpacing => 20;

    public static int CategoryLabelAlignX => 17;

    public static int CategoryLabelAlignY => -8;

    public static int CategoryWidth => 760;

    public static BorderStyle CategoryBorderStyle => BorderStyle.FixedSingle;
    public static Color DefaultBackgroundColor => Color.White;
    public static Color DefaultButtonBackgroundColor => Color.Gainsboro; //Never heard of this color but it matches on the UI

    public static bool CategoryLabelsUseAutosize => true;

    public static bool LabelsAndCheckboxesUseAutoEllipsis => true;

    public static int DefaultStartingPaneX => 6;

    public static int DefaultStartingPaneY => 15;

    public static bool UseMnemonic => false; //This is used so that way escape characters like & are used literally
    
    public const int TotalColorPickersPerRow = 1;
    public const int TotalNumberBoxesPerRow = 2;
    public const int TotalDropdownsPerRow = 2;
    public const int TotalFlagsPerRow = 3;

    public const int TooltipInitialShowDelayMs = 400;
    public const int TooltipRepeatDelayMs = 400;
    public const int TooltipDisplayLengthMs = 30000; //This is a hard limit of WinForms, sucks but oh well
}