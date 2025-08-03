using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerUI.Elements;

public static class WrappedLogicOptionFactory
{
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
