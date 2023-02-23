using RandomizerCore.Utilities.Extensions;

namespace RandomizerCore.Utilities.Models;

public class Settings
{
    public static int CurrentSettingsVersion = 2;
    public Dictionary<byte, List<byte>> Flags { get; set; }
    public Dictionary<byte, List<byte>> Dropdowns { get; set; }
    public Dictionary<byte, List<byte>> NumberBoxes { get; set; }
    public Dictionary<byte, List<ushort>> ColorPickers { get; set; }

    public int SettingsVersion { get; set; }
    public uint SettingHash { get; set; }

    public static Settings ParseSettingsFromSettingString(string settingString)
    {
        var settingBytes = Convert.FromBase64String(settingString);
        var version = settingBytes[0];

        return version switch
        {
            1 => throw new ArgumentException("Setting string version 1 is not supported!"),
            2 => ParseV2Settings(settingBytes),
            _ => throw new ArgumentException("Illegal settings version!")
        };
    }

    private static Settings ParseV2Settings(byte[] settingsBytes)
    {
        const byte flagValueMask = 0x1;
        const byte typeMask = 0x3;
        const byte settingHashMask = 0x3F;
        const byte dropdownIndexMask = 0x7F;
        const byte numberBoxValueMask = 0xFF;
        const ushort colorPickerValueMask = 0x7FFF;

        var offset = 1;

        var settings = new Settings
        {
            SettingsVersion = 2,
            SettingHash = settingsBytes.ByteArrayToUintLe(offset)
        };

        offset += 4;

        var flags = new Dictionary<byte, List<byte>>();
        var dropdowns = new Dictionary<byte, List<byte>>();
        var numberBoxes = new Dictionary<byte, List<byte>>();
        var colorPickers = new Dictionary<byte, List<ushort>>();

        var length = settingsBytes.ByteArrayToUshortLe(offset);

        offset += 2;

        var settingsAsNumber = settingsBytes.ParseBigIntegerFromByteArray(offset);

        var currentBitShiftIndex = length;

        while (currentBitShiftIndex > 0)
        {
            currentBitShiftIndex -= 2;
            var settingType = (byte)((settingsAsNumber >> currentBitShiftIndex) & typeMask);
            currentBitShiftIndex -= 6;
            var settingHash = (byte)((settingsAsNumber >> currentBitShiftIndex) & settingHashMask);
            switch (settingType)
            {
                case 0: //flag
                    currentBitShiftIndex -= 1;
                    var active = (byte)((settingsAsNumber >> currentBitShiftIndex) & flagValueMask);
                    if (!flags.ContainsKey(settingHash))
                        flags.Add(settingHash, new List<byte> { active });
                    else
                        flags[settingHash].Add(active);
                    break;
                case 1: //dropdown
                    currentBitShiftIndex -= 7;
                    var dropdownIndex = (byte)((settingsAsNumber >> currentBitShiftIndex) & dropdownIndexMask);
                    if (!dropdowns.ContainsKey(settingHash))
                        dropdowns.Add(settingHash, new List<byte> { dropdownIndex });
                    else
                        dropdowns[settingHash].Add(dropdownIndex);
                    break;
                case 2: //number box
                    currentBitShiftIndex -= 8;
                    var numberBoxValue = (byte)((settingsAsNumber >> currentBitShiftIndex) & numberBoxValueMask);
                    if (!numberBoxes.ContainsKey(settingHash))
                        numberBoxes.Add(settingHash, new List<byte> { numberBoxValue });
                    else
                        numberBoxes[settingHash].Add(numberBoxValue);
                    break;
                case 3: //color picker
                    currentBitShiftIndex -= 15;
                    var color = (ushort)((settingsAsNumber >> currentBitShiftIndex) & colorPickerValueMask);
                    if (!colorPickers.ContainsKey(settingHash))
                        colorPickers.Add(settingHash, new List<ushort> { color });
                    else
                        colorPickers[settingHash].Add(color);
                    break;
            }
        }

        settings.Flags = flags;
        settings.Dropdowns = dropdowns;
        settings.NumberBoxes = numberBoxes;
        settings.ColorPickers = colorPickers;

        return settings;
    }
}
