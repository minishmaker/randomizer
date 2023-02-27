using System.Numerics;

namespace RandomizerCore.Utilities.Extensions;

public static class ExtensionMethods
{
    public static string Hex(this uint num)
    {
        return Convert.ToString(num, 16).ToUpper();
    }

    public static string Hex(this int num)
    {
        return Convert.ToString(num, 16).ToUpper();
    }

    public static string Hex(this long num)
    {
        return Convert.ToString(num, 16).ToUpper();
    }

    public static string Hex(this ushort num)
    {
        return Convert.ToString(num, 16).ToUpper();
    }

    public static string Hex(this byte num)
    {
        return Convert.ToString(num, 16).ToUpper();
    }

    public static void Shuffle<T>(this IList<T> list, Random rng)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static BigInteger ParseBigIntegerFromByteArray(this byte[] bytes, int stoppingIndex = 0)
    {
        var len = bytes.Length - 1;
        var value = new BigInteger(0);
        for (var i = len; i >= stoppingIndex; --i)
        {
            var shift = (len - i) << 3;
            value |= (BigInteger)bytes[i] << shift;
        }

        return value;
    }

    public static byte[] UintToByteArrayLe(this uint value)
    {
        return new[]
        {
            (byte)(value & 0xFF),
            (byte)((value & 0xFF00) >> 8),
            (byte)((value & 0xFF0000) >> 16),
            (byte)((value & 0xFF000000) >> 24)
        };
    }

    public static uint ByteArrayToUintLe(this byte[] bytes, int offset)
    {
        return (uint)(bytes[offset] | (bytes[offset + 1] << 8) | (bytes[offset + 2] << 16) | (bytes[offset + 3] << 24));
    }

    public static ushort ByteArrayToUshortLe(this byte[] bytes, int offset)
    {
        return (ushort)(bytes[offset] | (bytes[offset + 1] << 8));
    }
}
