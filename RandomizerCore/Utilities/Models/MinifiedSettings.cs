using RandomizerCore.Randomizer.Logic.Options;
using RandomizerCore.Utilities.Extensions;

namespace RandomizerCore.Utilities.Models;

/// <summary>
///     A minified version of settings strings that can be shared. These require that the logic file crc32 matches,
///     otherwise it will fail to load.
///     These always start with the 6 character string NLogiC to make them unique from the other format (which is WIP)
/// </summary>
internal static class MinifiedSettings
{
    private static readonly byte[] Header = { 0x36, 0x58, 0x02 };

    public static void GenerateSettingsFromBase64String(string settingString, List<LogicOptionBase> sortedLogicOptions,
        uint logicOptionsCrc32)
    {
        if (!settingString.Substring(0, 4).Equals("NlgC", StringComparison.Ordinal))
            throw new InvalidSettingStringException(
                "Tried to load minified settings string but identifier NlgC was not found!");

        var bytes = Convert.FromBase64String(settingString);

        var currentIndex = 3;
        var crc32 = bytes.ByteArrayToUintLe(currentIndex);

        if (crc32 != logicOptionsCrc32)
            throw new InvalidSettingStringException(
                "Tried to load minified settings string with the wrong logic file!");

        currentIndex = 7;
        var currentByte = bytes[currentIndex];
        var optionGroups = sortedLogicOptions.GroupBy(x => x.GetType()).ToList();

        var flags = optionGroups.Any(group => group.Key == typeof(LogicFlag))
            ? optionGroups.First(group => group.Key == typeof(LogicFlag)).Cast<LogicFlag>().ToList()
            : new List<LogicFlag>();

        for (int i = 7, flagsProcressed = 0; flagsProcressed < flags.Count; --i, ++flagsProcressed)
        {
            if (i < 0)
            {
                i = 7;
                currentByte = bytes[++currentIndex];
            }

            var flagBit = (currentByte >> i) & 1;

            flags[flagsProcressed].Active = flagBit == 1;
            flags[flagsProcressed].NotifyObservers();
        }

        currentByte = bytes[++currentIndex];

        var dropdowns = optionGroups.Any(group => group.Key == typeof(LogicDropdown))
            ? optionGroups.First(group => group.Key == typeof(LogicDropdown)).Cast<LogicDropdown>().ToList()
            : new List<LogicDropdown>();

        for (int i = 2, dropdownsProcessed = 0; dropdownsProcessed < dropdowns.Count; i -= 6, ++dropdownsProcessed)
        {
            var dropdownIndex = 0;

            if (i < 0)
            {
                i += 8;
                dropdownIndex |= (currentByte << (8 - i)) & 0x3F;
                currentByte = bytes[++currentIndex];
            }

            dropdownIndex |= (currentByte >> i) & 0x3F;

            var dropdown = dropdowns[dropdownsProcessed];

            dropdown.Selection = dropdown.Selections.Keys.ToList()[dropdownIndex];
            dropdown.NotifyObservers();
        }

        var numberBoxes = optionGroups.Any(group => group.Key == typeof(LogicNumberBox))
            ? optionGroups.First(group => group.Key == typeof(LogicNumberBox)).Cast<LogicNumberBox>().ToList()
            : new List<LogicNumberBox>();

        for (var numberBoxesProcessed = 0; numberBoxesProcessed < numberBoxes.Count; ++numberBoxesProcessed)
        {
            currentByte = bytes[++currentIndex];

            var numberBox = numberBoxes[numberBoxesProcessed];

            numberBox.Value = $"{currentByte}";
            numberBox.NotifyObservers();
        }

        var colorPickers = optionGroups.Any(group => group.Key == typeof(LogicColorPicker))
            ? optionGroups.First(group => group.Key == typeof(LogicColorPicker)).Cast<LogicColorPicker>().ToList()
            : new List<LogicColorPicker>();

        for (var colorPickersProcessed = 0; colorPickersProcessed < colorPickers.Count; ++colorPickersProcessed)
        {
            currentByte = bytes[++currentIndex];
            var colorSecondByte = bytes[++currentIndex];

            var colorPicker = colorPickers[colorPickersProcessed];

            colorPicker.DefinedColor = new GbaColor((currentByte & 0xF8) >> 3,
                ((currentByte & 0x7) << 2) | ((colorSecondByte & 0xC0) >> 6), (colorSecondByte & 0x3E) >> 1).ToColor();
            colorPicker.UseRandomColor = (colorSecondByte & 1) == 1;
            colorPicker.NotifyObservers();
        }
    }

    public static string GenerateSettingsString(List<LogicOptionBase> sortedLogicOptions, uint logicOptionsCrc32)
    {
        var bytes = new List<byte>();

        bytes.AddRange(Header);
        bytes.AddRange(logicOptionsCrc32.UintToByteArrayLe());

        var optionGroups = sortedLogicOptions.GroupBy(x => x.GetType()).ToList();
        var flags = optionGroups.Any(group => group.Key == typeof(LogicFlag))
            ? optionGroups.First(group => group.Key == typeof(LogicFlag)).Cast<LogicFlag>().ToList()
            : new List<LogicFlag>();
        byte currentByte = 0;

        for (int i = 7, flagsProcressed = 0; flagsProcressed < flags.Count; --i, ++flagsProcressed)
        {
            if (i < 0)
            {
                i = 7;
                bytes.Add(currentByte);
                currentByte = 0;
            }

            currentByte |= flags[flagsProcressed].Active ? (byte)(1 << i) : (byte)(0 << i);
        }

        bytes.Add(currentByte);
        currentByte = 0;

        var dropdowns = optionGroups.Any(group => group.Key == typeof(LogicDropdown))
            ? optionGroups.First(group => group.Key == typeof(LogicDropdown)).Cast<LogicDropdown>().ToList()
            : new List<LogicDropdown>();

        for (int i = 2, dropdownsProcessed = 0; dropdownsProcessed < dropdowns.Count; i -= 6, ++dropdownsProcessed)
        {
            var dropdown = dropdowns[dropdownsProcessed];
            var dropdownValueAsByte = dropdown.Selections.Keys.ToList().IndexOf(dropdown.Selection) & 0x3F;

            if (i < 0)
            {
                i += 8;
                currentByte |= (byte)(dropdownValueAsByte >> (8 - i));
                bytes.Add(currentByte);
                currentByte = 0;
                currentByte |= (byte)(dropdownValueAsByte << i);
            }

            currentByte |= (byte)(dropdownValueAsByte << i);
        }

        bytes.Add(currentByte);

        var numberBoxes = optionGroups.Any(group => group.Key == typeof(LogicNumberBox))
            ? optionGroups.First(group => group.Key == typeof(LogicNumberBox)).Cast<LogicNumberBox>().ToList()
            : new List<LogicNumberBox>();

        for (var numberBoxesProcessed = 0; numberBoxesProcessed < numberBoxes.Count; ++numberBoxesProcessed)
        {
            currentByte = numberBoxes[numberBoxesProcessed].GetHashByte();
            bytes.Add(currentByte);
        }

        var colorPickers = optionGroups.Any(group => group.Key == typeof(LogicColorPicker))
            ? optionGroups.First(group => group.Key == typeof(LogicColorPicker)).Cast<LogicColorPicker>().ToList()
            : new List<LogicColorPicker>();

        for (var colorPickersProcessed = 0; colorPickersProcessed < colorPickers.Count; ++colorPickersProcessed)
        {
            var colorPicker = colorPickers[colorPickersProcessed];

            var color = new GbaColor(colorPicker.DefinedColor);

            currentByte = (byte)((color.R << 3) | (color.G >> 2));
            var colorSecondByte = (byte)(((color.G & 3) << 6) | (color.B << 1));

            colorSecondByte |= (byte)(colorPicker.UseRandomColor ? 1 : 0);

            bytes.Add(currentByte);
            bytes.Add(colorSecondByte);
        }

        return Convert.ToBase64String(bytes.ToArray());
    }
}
