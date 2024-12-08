using MinishCapRandomizerUI.DrawConstants;
using MinishCapRandomizerUI.Elements;

namespace MinishCapRandomizerUI.UI.MainWindow.Helpers;

public static class UIGenerator
{
    private static readonly Size TabPaneSize = new Size(772, 535);

    public static TabPage BuildSettingsPage(List<WrapperBase> wrappedLogicOptions, string pageName)
    {
        var page = new TabPage
        {
            Name = pageName,
            Text = pageName,
            AutoScroll = true,
            Size = TabPaneSize,
            BackColor = Constants.DefaultBackgroundColor,
        };

        page.Click += (sender, e) => page.Focus(); // helps with mouse wheel scrolling
        
        var groups = wrappedLogicOptions.GroupBy(option => option.SettingGrouping).ToList();

        var totalPaneHeight = Constants.DefaultStartingPaneY;
        
        foreach (var group in groups)
        {
            var dropdowns = group.OfType<DropdownWrapper>().ToList();
            var flags = group.OfType<FlagWrapper>().ToList();
            var numberBoxes = group.OfType<NumberBoxWrapper>().ToList();
            var colorPickers = group.OfType<ColorPickerWrapper>().ToList();

            var dropdownRows = dropdowns.Count / Constants.TotalDropdownsPerRow +
                               (dropdowns.Count % Constants.TotalDropdownsPerRow == 0 ? 0 : 1);
            var numberBoxRows = numberBoxes.Count / Constants.TotalNumberBoxesPerRow +
                                (numberBoxes.Count % Constants.TotalNumberBoxesPerRow == 0 ? 0 : 1);
            var flagRows = flags.Count / Constants.TotalFlagsPerRow +
                           (flags.Count % Constants.TotalFlagsPerRow == 0 ? 0 : 1);
            var colorPickerRows = colorPickers.Count / Constants.TotalColorPickersPerRow +
                                  (colorPickers.Count % Constants.TotalColorPickersPerRow == 0 ? 0 : 1);

            var height = dropdownRows != 0 ? dropdowns[0].ElementHeight * dropdownRows : 0;
            height += numberBoxRows != 0 ? numberBoxes[0].ElementHeight * numberBoxRows : 0;
            height += flagRows != 0 ? flags[0].ElementHeight * flagRows : 0;
            height += colorPickerRows != 0 ? colorPickers[0].ElementHeight * colorPickerRows : 0;
            height += Constants.TopRowAboveSpacing;
            
            var pane = new Panel
            {
                Name = group.Key,
                Width = (int)(Constants.CategoryWidth*Constants.SpecialScaling),
                Height = height,
                AutoScroll = false,
                Location = new Point(Constants.DefaultStartingPaneX, totalPaneHeight),
                BorderStyle = Constants.CategoryBorderStyle,
            };

            var categoryLabel = new Label
            {
                AutoSize = Constants.CategoryLabelsUseAutosize,
                AutoEllipsis = false,
                Name = $"{group.Key}_label",
                Location = new Point(Constants.CategoryLabelAlignX, totalPaneHeight + Constants.CategoryLabelAlignY),
                Text = group.Key,
                UseMnemonic = Constants.UseMnemonic,
            };

            categoryLabel.Click += (sender, e) => page.Focus(); // helps with mouse wheel scrolling
            pane.Click += (sender, e) => page.Focus(); // helps with mouse wheel scrolling

            var currentElementXLocation = Constants.FirstElementInRowX;
            var currentElementYLocation = Constants.TopRowAboveSpacing;
            
            AddElementsToPane(dropdowns, Constants.TotalDropdownsPerRow, ref pane, 
                ref currentElementXLocation, ref currentElementYLocation);
            AddElementsToPane(flags, Constants.TotalFlagsPerRow, ref pane, 
                ref currentElementXLocation, ref currentElementYLocation);
            AddElementsToPane(numberBoxes, Constants.TotalNumberBoxesPerRow, ref pane, 
                ref currentElementXLocation, ref currentElementYLocation);
            AddElementsToPane(colorPickers, Constants.TotalColorPickersPerRow, ref pane, 
                ref currentElementXLocation, ref currentElementYLocation);
            
            page.Controls.Add(categoryLabel);
            page.Controls.Add(pane);

            totalPaneHeight += height + Constants.CategorySpacing;
        }

        return page;
    }

    private static void AddElementsToPane(
        IReadOnlyList<WrapperBase> elements,
        int totalElementsPerRow,
        ref Panel pane,
        ref int currentElementXLocation,
        ref int currentElementYLocation)
    {
        for (var i = 0; i < elements.Count; )
        {
            var element = elements[i++];
            pane.Controls.AddRange(element.GetControls(currentElementXLocation, currentElementYLocation).ToArray());

            if (i % totalElementsPerRow == 0 || i == elements.Count)
            {
                currentElementXLocation = Constants.FirstElementInRowX;
                currentElementYLocation += element.ElementHeight;
            }
            else
                currentElementXLocation += element.ElementWidth + Constants.WidthMargin;
        }
    }
}
