using MinishCapRandomizerUI.DrawConstants;
using MinishCapRandomizerUI.Elements;

namespace MinishCapRandomizerUI.UI;

public class UIGenerator
{
    public static TabPage BuildUIPage(List<WrapperBase> wrappedLogicOptions, string pageName)
    {
        var page = new TabPage
        {
            Name = pageName,
            Text = pageName,
            AutoScroll = true,
            Size = new Size(772, 635), //Constant
            BackColor = Constants.DefaultBackgroundColor,
        };
        
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
                Width = Constants.CategoryWidth,
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
            };

            var currentElementXLocation = Constants.FirstElementInRowX;
            var currentElementYLocation = Constants.TopRowAboveSpacing;
            
            for (var i = 0; i < dropdowns.Count; )
            {
                var dropdown = dropdowns[i++];
                pane.Controls.AddRange(dropdown.GetControls(currentElementXLocation, currentElementYLocation).ToArray());

                if (i % Constants.TotalDropdownsPerRow == 0 || i == dropdowns.Count)
                {
                    currentElementXLocation = Constants.FirstElementInRowX;
                    currentElementYLocation += dropdown.ElementHeight;
                }
                else
                    currentElementXLocation += dropdown.ElementWidth + Constants.WidthMargin;
            }
            
            for (var i = 0; i < flags.Count; )
            {
                var flag = flags[i++];
                pane.Controls.AddRange(flag.GetControls(currentElementXLocation, currentElementYLocation).ToArray());

                if (i % Constants.TotalFlagsPerRow == 0 || i == flags.Count)
                {
                    currentElementXLocation = Constants.FirstElementInRowX;
                    currentElementYLocation += flag.ElementHeight;
                }
                else
                    currentElementXLocation += flag.ElementWidth + Constants.WidthMargin;
            }
            
            for (var i = 0; i < numberBoxes.Count; )
            {
                var numberBox = numberBoxes[i++];
                pane.Controls.AddRange(numberBox.GetControls(currentElementXLocation, currentElementYLocation).ToArray());

                if (i % Constants.TotalNumberBoxesPerRow == 0 || i == numberBoxes.Count)
                {
                    currentElementXLocation = Constants.FirstElementInRowX;
                    currentElementYLocation += numberBox.ElementHeight;
                }
                else
                    currentElementXLocation += numberBox.ElementWidth + Constants.WidthMargin;
            }
            
            for (var i = 0; i < colorPickers.Count; )
            {
                var colorPicker = colorPickers[i++];
                pane.Controls.AddRange(colorPicker.GetControls(currentElementXLocation, currentElementYLocation).ToArray());

                if (i % Constants.TotalColorPickersPerRow == 0 || i == colorPickers.Count)
                {
                    currentElementXLocation = Constants.FirstElementInRowX;
                    currentElementYLocation += colorPicker.ElementHeight;
                }
                else
                    currentElementXLocation += colorPicker.ElementWidth + Constants.WidthMargin;
            }
            
            page.Controls.Add(categoryLabel);
            page.Controls.Add(pane);

            totalPaneHeight += height + Constants.CategorySpacing;
        }

        return page;
    }
}