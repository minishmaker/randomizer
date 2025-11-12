using Avalonia.Controls;
using Avalonia.Layout;

namespace MinishCapRandomizerUI.Avalonia.Wrappers;

public static class ControlsPort
{
    public static void AddToGrid(Grid grid, Control control, int col, int row, int colSpan = 1, int rowSpan = 1)
    {
        Grid.SetColumn(control, col);
        Grid.SetRow(control, row);
        if (colSpan > 1) Grid.SetColumnSpan(control, colSpan);
        if (rowSpan > 1) Grid.SetRowSpan(control, rowSpan);
        grid.Children.Add(control);
    }
}

