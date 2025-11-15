using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using MinishCapRandomizerUI.Avalonia.DrawConstants;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Avalonia.Elements;

public static class WrappedLogicOptionFactory
{
    public static List<WrapperBase> BuildGenericWrappedLogicOptions(List<LogicOptionBase> logicOptions)
    {
        var wrappedOptions = new List<WrapperBase>();
        foreach (var option in logicOptions)
        {
            switch (option)
            {
                case LogicFlag flag: wrappedOptions.Add(new FlagWrapper(flag)); break;
                case LogicDropdown dd: wrappedOptions.Add(new DropdownWrapper(dd)); break;
                case LogicNumberBox nb: wrappedOptions.Add(new NumberBoxWrapper(nb)); break;
                case LogicColorPicker cp: wrappedOptions.Add(new ColorPickerWrapper(cp)); break;
            }
        }
        return wrappedOptions;
    }

    public static List<WrapperBase> BuildGenericWrappedLogicOptions(IEnumerable<LogicOptionBase> logicOptions)
    {
        var wrappedOptions = new List<WrapperBase>();
        foreach (var option in logicOptions)
        {
            switch (option)
            {
                case LogicFlag flag: wrappedOptions.Add(new FlagWrapper(flag)); break;
                case LogicDropdown dd: wrappedOptions.Add(new DropdownWrapper(dd)); break;
                case LogicNumberBox nb: wrappedOptions.Add(new NumberBoxWrapper(nb)); break;
                case LogicColorPicker cp: wrappedOptions.Add(new ColorPickerWrapper(cp)); break;
            }
        }
        return wrappedOptions;
    }

    public static Control BuildGroupContainer(string groupName, IEnumerable<WrapperBase> elements)
    {
        int columns = 2;
        if (groupName.Contains("Fusions", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Progressive", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Main Items", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Quest Status Items", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Sword Scrolls", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Joy Butterflies", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Wind Crests", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Dungeon Warps", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Speed Up", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Global", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Big Keys", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Difficulty", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Overworld", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Dungeons", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Logic", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Small Keys", StringComparison.OrdinalIgnoreCase) ||
            groupName.Contains("Maps & Compasses", StringComparison.OrdinalIgnoreCase))
        {
            columns = 3;
        }

        bool multiColumn = columns == 3;
        var reordered = elements.OrderBy(e => e switch {
            DropdownWrapper => 0,
            FlagWrapper => 1,
            NumberBoxWrapper => 2,
            ColorPickerWrapper => 3,
            _ => 4
        }).ToList();

        if (groupName.Contains("Figurine", StringComparison.OrdinalIgnoreCase))
        {
            var figFlag = reordered.OfType<FlagWrapper>().FirstOrDefault(f => f.IsFigurineHuntFlag);
            if (figFlag != null)
            {
                reordered.Remove(figFlag);
                var figInputs = reordered.Where(w => (w as NumberBoxWrapper)?.IsFigurineRelated == true).ToList();
                foreach (var fi in figInputs) reordered.Remove(fi);
                reordered.Insert(0, figFlag);
                var insertAt = 1;
                foreach (var fi in figInputs) reordered.Insert(insertAt++, fi);
            }
            columns = 2;
            multiColumn = false;
        }

        var grid = new Grid{ Margin = new Thickness(1) };
        for (int i=0;i<columns;i++) grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        int col=0,row=0; void ensureRow(int r){ while(grid.RowDefinitions.Count <= r) grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); }

        bool placedAnyFlags = false;
        bool numberBoxRowStarted = false;
        foreach (var element in reordered)
        {
            if (!placedAnyFlags && element is FlagWrapper)
            {
                placedAnyFlags = true;
                if (col != 0) { col = 0; row++; }
            }
            if (!numberBoxRowStarted && element is NumberBoxWrapper && placedAnyFlags)
            {
                numberBoxRowStarted = true;
                if (col != 0) { col = 0; row++; }
            }

            var controls = element.BuildControls(0,0);

            // For dropdowns/textboxes, ensure label doesn't wrap and dropdown has enough width
            if (controls.Count==2 && (controls[1] is ComboBox || controls[1] is TextBox))
            {
                if (controls[0] is TextBlock lbl)
                {
                    lbl.Width = Double.NaN;
                    lbl.TextWrapping = TextWrapping.NoWrap;
                    lbl.Margin = new Thickness(0,0,6,0);
                    lbl.VerticalAlignment = VerticalAlignment.Center;
                }
                if (controls[1] is ComboBox cb)
                {
                    cb.Width = 180; // Fixed width to prevent resize on selection change
                    cb.HorizontalAlignment = HorizontalAlignment.Left;
                }
                else if (controls[1] is TextBox tb)
                {
                    tb.Width = 180; // Fixed width for consistency
                    tb.HorizontalAlignment = HorizontalAlignment.Left;
                }
            }

            if (multiColumn && controls.Count==1 && controls[0] is CheckBox chk)
            {
                if (chk.Content is TextBlock ctb){ ctb.MaxWidth=240; ctb.TextWrapping=TextWrapping.Wrap; }
                else if (chk.Content is string s){ chk.Content = new TextBlock{ Text=s, MaxWidth=240, TextWrapping=TextWrapping.Wrap }; }
            }

            bool isHeartColorRow = false;
            if (groupName.Contains("Hearts", StringComparison.OrdinalIgnoreCase) || groupName.Contains("Tunic", StringComparison.OrdinalIgnoreCase) || groupName.Contains("Split Bar", StringComparison.OrdinalIgnoreCase))
            {
                isHeartColorRow = controls.OfType<TextBlock>().Any(tb=>tb.Text?.StartsWith("Heart Color")==true);
            }
            if (isHeartColorRow && col!=0){ col=0; row++; }

            // Always use horizontal layout - put all controls in a horizontal container
            var container = new StackPanel{ Orientation = Orientation.Horizontal, Spacing = 4, Margin = new Thickness(0,0,6,4) };
            foreach (var c in controls)
            {
                if (!multiColumn && c is TextBlock tb && tb.Text?.EndsWith(":")==true)
                {
                    tb.TextWrapping = TextWrapping.NoWrap;
                    tb.Margin = new Thickness(0,0,6,0);
                    tb.VerticalAlignment = VerticalAlignment.Center;
                }
                container.Children.Add(c);
            }

            ensureRow(row);
            Grid.SetRow(container,row);
            Grid.SetColumn(container,col);
            grid.Children.Add(container);
            col++; if (col>=columns){ col=0; row++; }
        }
        var headered = new HeaderedContentControl {
            Header = groupName,
            Content = grid,
            Classes = { "compact-groupbox" }
        };
        return headered;
    }
}
