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
            groupName.Contains("Difficulty", StringComparison.OrdinalIgnoreCase))
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
            StackPanel? verticalWrapper = null;
            if (multiColumn && controls.Count==2 && (controls[1] is ComboBox || controls[1] is TextBox))
            {
                if (controls[0] is TextBlock lbl)
                { lbl.Width = Double.NaN; lbl.MaxWidth = 220; lbl.TextWrapping = TextWrapping.Wrap; lbl.Margin = new Thickness(0,0,0,2); }
                if (controls[1] is Control input)
                { input.Width = Double.NaN; input.HorizontalAlignment = HorizontalAlignment.Stretch; }
                verticalWrapper = new StackPanel{ Orientation = Orientation.Vertical, Spacing = 2, Margin = new Thickness(0,0,6,4) };
                verticalWrapper.Children.Add(controls[0]); verticalWrapper.Children.Add(controls[1]);
            }
            if (multiColumn && controls.Count==1 && controls[0] is CheckBox chk)
            {
                if (chk.Content is TextBlock ctb){ ctb.MaxWidth=200; ctb.TextWrapping=TextWrapping.Wrap; }
                else if (chk.Content is string s){ chk.Content = new TextBlock{ Text=s, MaxWidth=200, TextWrapping=TextWrapping.Wrap }; }
            }

            bool isHeartColorRow = false;
            if (groupName.Contains("Hearts", StringComparison.OrdinalIgnoreCase) || groupName.Contains("Tunic", StringComparison.OrdinalIgnoreCase) || groupName.Contains("Split Bar", StringComparison.OrdinalIgnoreCase))
            {
                isHeartColorRow = controls.OfType<TextBlock>().Any(tb=>tb.Text?.StartsWith("Heart Color")==true) ||
                    (verticalWrapper!=null && verticalWrapper.Children.OfType<TextBlock>().Any(tb=>tb.Text?.StartsWith("Heart Color")==true));
            }
            if (isHeartColorRow && col!=0){ col=0; row++; }

            Control toAdd;
            if (verticalWrapper!=null) toAdd = verticalWrapper;
            else
            {
                var container = new StackPanel{ Orientation = Orientation.Horizontal, Spacing = 4, Margin = new Thickness(0,0,6,4) };
                foreach (var c in controls)
                {
                    if (!multiColumn && c is TextBlock tb && tb.Text?.EndsWith(":")==true){ tb.TextWrapping=TextWrapping.Wrap; tb.MaxWidth=180; }
                    container.Children.Add(c);
                }
                toAdd = container;
            }
            ensureRow(row);
            Grid.SetRow(toAdd,row); Grid.SetColumn(toAdd,col); grid.Children.Add(toAdd);
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
