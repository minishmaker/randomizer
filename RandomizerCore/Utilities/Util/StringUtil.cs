using System.Globalization;

namespace RandomizerCore.Utilities.Util;

internal class StringUtil
{
    // Convert values to hex strings
    public static string AsStringHex(int val, int spacing)
    {
        return val.ToString("X").PadLeft(spacing, '0');
    }

    public static string AsStringHex2(int val)
    {
        return AsStringHex(val, 2);
    }

    public static string AsStringHex4(int val)
    {
        return AsStringHex(val, 4);
    }

    public static string AsStringGbaAddress(int val)
    {
        return AsStringHex(val, 6);
    }

    public static string AsStringHex8(int val)
    {
        return AsStringHex(val, 8);
    }

    public static string AsStringHex16(int val)
    {
        return AsStringHex(val, 16);
    }

    public static string AsStringHex32(int val)
    {
        return AsStringHex(val, 32);
    }

    //just so its all in one place
    public static bool ParseString(string val, out int value)
    {
        if (val.StartsWith("0x") || val.StartsWith("0X"))
        {
            val = val.Substring(2);
            return int.TryParse(val, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        }

        return int.TryParse(val, out value);
    }

    public static bool ParseString(string val, out byte value)
    {
        if (val.StartsWith("0x") || val.StartsWith("0X"))
        {
            val = val.Substring(2);
            return byte.TryParse(val, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        }

        return byte.TryParse(val, out value);
    }
}
