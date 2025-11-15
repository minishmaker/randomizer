using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Visuals;
using MinishCapRandomizerUI.Avalonia.DrawConstants;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Avalonia.Elements;

public static class WrappedLogicOptionFactory
{
    private enum RowKind { Flags, Inputs, Color, Mixed }

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
        var elementsList = elements.ToList();
        bool isItemPoolTab = elementsList.Any() && elementsList.First().Page?.Contains("Item Pool", StringComparison.OrdinalIgnoreCase) == true;

        var reordered = elementsList.OrderBy(e => e switch {
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
        }


        var rows = new List<(RowKind Kind, List<WrapperBase> Items)>();

        foreach (var el in reordered)
        {
            var isFlag = el is FlagWrapper;
            var isInput = el is DropdownWrapper || el is NumberBoxWrapper;
            var isColor = el is ColorPickerWrapper;

            if (isColor)
            {
                rows.Add((RowKind.Color, new List<WrapperBase>{el}));
                continue;
            }

            if (isFlag)
            {
                var last = rows.LastOrDefault();
                if (last.Items != null && last.Kind == RowKind.Flags && last.Items.Count < 3)
                {
                    last.Items.Add(el);
                    rows[^1] = last;
                }
                else
                {
                    rows.Add((RowKind.Flags, new List<WrapperBase>{el}));
                }
                continue;
            }

            if (isInput)
            {
                var last = rows.LastOrDefault();
                if (last.Items != null && last.Kind == RowKind.Inputs && last.Items.Count < 2)
                {
                    last.Items.Add(el);
                    rows[^1] = last;
                }
                else
                {
                    rows.Add((RowKind.Inputs, new List<WrapperBase>{el}));
                }
                continue;
            }

            rows.Add((RowKind.Mixed, new List<WrapperBase>{el}));
        }

        double measuredLabelWidth = 0.0;
        foreach (var row in rows)
        {
            foreach (var item in row.Items)
            {
                var controls = item.BuildControls(0, 0);
                var label = controls.OfType<TextBlock>().FirstOrDefault();
                if (label != null)
                {
                    label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    var w = label.DesiredSize.Width;
                    if (w > measuredLabelWidth) measuredLabelWidth = w;
                }
            }
        }

        var preferredCap = isItemPoolTab ? 320.0 : 240.0;
        measuredLabelWidth = Math.Min(measuredLabelWidth + 8.0, preferredCap);
        if (measuredLabelWidth < 60.0) measuredLabelWidth = 60.0;

        var mainGrid = new Grid { HorizontalAlignment = HorizontalAlignment.Stretch };
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(measuredLabelWidth, GridUnitType.Pixel)));
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(measuredLabelWidth, GridUnitType.Pixel)));
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

        int gridRow = 0;
        foreach (var row in rows)
        {
            mainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            if (row.Kind == RowKind.Flags)
            {
                var flagsPanel = new Grid { HorizontalAlignment = HorizontalAlignment.Stretch };
                for (int i = 0; i < Math.Max(1, row.Items.Count); i++) flagsPanel.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                for (int i = 0; i < row.Items.Count; i++)
                {
                    var item = row.Items[i];
                    var ctrls = item.BuildControls(0, 0);
                    var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6, HorizontalAlignment = HorizontalAlignment.Left };
                    foreach (var c in ctrls) panel.Children.Add(c);
                    Grid.SetColumn(panel, i);
                    flagsPanel.Children.Add(panel);
                }
                Grid.SetRow(flagsPanel, gridRow);
                Grid.SetColumnSpan(flagsPanel, 5);
                mainGrid.Children.Add(flagsPanel);
            }
            else if (row.Kind == RowKind.Inputs)
            {
                for (int i = 0; i < row.Items.Count && i < 2; i++)
                {
                    var item = row.Items[i];
                    var controls = item.BuildControls(0, 0);
                    var label = controls.OfType<TextBlock>().FirstOrDefault();
                    var control = controls.FirstOrDefault(c => !(c is TextBlock));

                    if (label != null)
                    {
                        label.FontSize = 11;
                        label.MinWidth = 60;
                        label.MaxWidth = measuredLabelWidth;
                        label.TextWrapping = TextWrapping.NoWrap;
                        label.TextTrimming = TextTrimming.CharacterEllipsis;
                        label.Margin = new Thickness(0, 0, 6, 0);
                        label.VerticalAlignment = VerticalAlignment.Center;
                        label.HorizontalAlignment = HorizontalAlignment.Right;
                        label.TextAlignment = TextAlignment.Right;
                        Grid.SetRow(label, gridRow);
                        Grid.SetColumn(label, i * 2);
                        mainGrid.Children.Add(label);
                    }

                    if (control != null)
                    {
                        if (control is ComboBox cb)
                        {
                            cb.HorizontalAlignment = HorizontalAlignment.Left;
                            cb.Width = 160;
                            cb.FontSize = 11;
                            cb.Margin = new Thickness(0, 0, 6, 0);
                        }
                        else if (control is TextBox tb)
                        {
                            tb.HorizontalAlignment = HorizontalAlignment.Left;
                            tb.Width = 160;
                            tb.FontSize = 11;
                            tb.Margin = new Thickness(0, 0, 6, 0);
                        }

                        Grid.SetRow(control, gridRow);
                        Grid.SetColumn(control, i * 2 + 1);
                        mainGrid.Children.Add(control);
                    }
                }
            }
            else if (row.Kind == RowKind.Color)
            {
                var item = row.Items[0];
                var controls = item.BuildControls(0, 0);

                var panel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 8,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                foreach (var control in controls)
                {
                    if (control is TextBlock tb)
                    {
                        tb.FontSize = 11;
                        tb.VerticalAlignment = VerticalAlignment.Center;
                    }
                    panel.Children.Add(control);
                }

                Grid.SetRow(panel, gridRow);
                Grid.SetColumn(panel, 0);
                Grid.SetColumnSpan(panel, 5);
                mainGrid.Children.Add(panel);
            }
            else // Mixed
            {
                // Place items left-to-right into label/control pairs where possible, fall back to stacked panels
                for (int i = 0; i < row.Items.Count && i < 2; i++)
                {
                    var item = row.Items[i];
                    var controls = item.BuildControls(0, 0);
                    var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
                    foreach (var c in controls) panel.Children.Add(c);
                    Grid.SetRow(panel, gridRow);
                    Grid.SetColumn(panel, i * 2);
                    Grid.SetColumnSpan(panel, 2);
                    mainGrid.Children.Add(panel);
                }
            }

            gridRow++;
        }

        var headered = new HeaderedContentControl
        {
            Header = groupName,
            Content = mainGrid,
            Classes = { "compact-groupbox" }
        };

        headered.AttachedToVisualTree += (s, e) =>
        {
            void UpdateLabelColumns()
            {
                try
                {
                    double available = headered.Bounds.Width;
                    if (available <= 0)
                    {
                        available = preferredCap * 3;
                    }
                    if (available <= 0) available = preferredCap * 3;

                    var target = Math.Min(measuredLabelWidth, Math.Min(preferredCap, available * 0.45));
                    if (mainGrid.ColumnDefinitions.Count >= 3)
                    {
                        mainGrid.ColumnDefinitions[0].Width = new GridLength(target, GridUnitType.Pixel);
                        mainGrid.ColumnDefinitions[2].Width = new GridLength(target, GridUnitType.Pixel);
                    }
                }
                catch { }
            }

            UpdateLabelColumns();
            headered.GetObservable(Control.BoundsProperty).Subscribe(_ => UpdateLabelColumns());
        };

        return headered;
    }
}
