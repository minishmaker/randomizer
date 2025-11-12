using Avalonia.Media;
using Avalonia;

namespace MinishCapRandomizerUI.Avalonia.DrawConstants;

public static class Constants
{
    public static int TopRowAboveSpacing => 15;
    public static int FirstElementInRowX => 10;
    public static int WidthMargin => 10;
    public static int CategorySpacing => 20;
    public static int CategoryLabelAlignX => 17; // kept for parity
    public static int CategoryLabelAlignY => -8;
    public static int CategoryWidth => 760;

    // Avalonia equivalents for WinForms styling
    public static Thickness CategoryBorderThickness => new(1);
    public static IBrush CategoryBorderBrush => Brushes.Gray;
    public static IBrush DefaultBackgroundBrush => Brushes.White;
    public static IBrush DefaultButtonBackgroundBrush => Brushes.Transparent;

    public static bool CategoryLabelsUseAutosize => true;
    public static bool LabelsAndCheckboxesUseAutoEllipsis => true; // Not directly supported; kept for parity reference

    public static int DefaultStartingPaneX => 6;
    public static int DefaultStartingPaneY => 15;
    public static double SpecialScaling = 1; // DPI scaling factor

    public static bool UseMnemonic => false; // Parity placeholder; Avalonia handles access keys differently

    public const int TotalColorPickersPerRow = 1;
    public const int TotalNumberBoxesPerRow = 2;
    public const int TotalDropdownsPerRow = 2;
    public const int TotalFlagsPerRow = 3;

    public const int TooltipInitialShowDelayMs = 400;
    public const int TooltipRepeatDelayMs = 400;
    public const int TooltipDisplayLengthMs = 30000;
}

