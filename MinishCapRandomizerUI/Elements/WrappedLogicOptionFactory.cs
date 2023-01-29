using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Elements;

public static class WrappedLogicOptionFactory
{
    public static (List<DropdownWrapper> dropdowns, List<FlagWrapper> flags, List<NumberBoxWrapper> numberBoxes,
        List<ColorPickerWrapper> colorPickers) BuildWrappedLogicOptions(List<LogicOptionBase> logicOptions)
    {
        var dropdowns = new List<DropdownWrapper>();
        var flags = new List<FlagWrapper>();
        var numberBoxes = new List<NumberBoxWrapper>();
        var colorPickers = new List<ColorPickerWrapper>();

        foreach (var option in logicOptions)
        {
            switch (option)
            {
                case LogicFlag flag:
                    flags.Add(new FlagWrapper(flag));
                    break;
                case LogicDropdown dropdown:
                    dropdowns.Add(new DropdownWrapper(dropdown));
                    break;
                case LogicNumberBox numberBox:
                    numberBoxes.Add(new NumberBoxWrapper(numberBox));
                    break;
                case LogicColorPicker colorPicker:
                    colorPickers.Add(new ColorPickerWrapper(colorPicker));
                    break;
            }
        }

        return (dropdowns, flags, numberBoxes, colorPickers);
    }

    public static List<WrapperBase> BuildGenericWrappedLogicOptions(List<LogicOptionBase> logicOptions)
    {
        var wrappedOptions = new List<WrapperBase>();
        
        foreach (var option in logicOptions)
        {
            switch (option)
            {
                case LogicFlag flag:
                    wrappedOptions.Add(new FlagWrapper(flag));
                    break;
                case LogicDropdown dropdown:
                    wrappedOptions.Add(new DropdownWrapper(dropdown));
                    break;
                case LogicNumberBox numberBox:
                    wrappedOptions.Add(new NumberBoxWrapper(numberBox));
                    break;
                case LogicColorPicker colorPicker:
                    wrappedOptions.Add(new ColorPickerWrapper(colorPicker));
                    break;
            }
        }

        return wrappedOptions;
    }
}