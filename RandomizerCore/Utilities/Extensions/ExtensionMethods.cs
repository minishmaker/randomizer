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

    public static byte Crc8(this byte[] bytes)
    {
        var table = new byte[256];
        const byte poly = 0xd5; //See wikipedia for CRC

        for (var i = 0; i < 256; ++i)
        {
            var temp = i;
            for (var j = 0; j < 8; ++j)
                if ((temp & 0x80) != 0)
                    temp = (temp << 1) ^ poly;
                else
                    temp <<= 1;
            table[i] = (byte)temp;
        }

        byte crc = 0;
        if (bytes != null && bytes.Length > 0)
            crc = bytes.Aggregate(crc, (current, b) => table[current ^ b]);

        return crc;
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
}