namespace RandomizerCore.Utilities.Util;

public static class CrcUtil
{
    
    /// <summary>
    ///     Find the crc32 of a byte array, adapted from https://www.romhacking.net/patch/crc32
    /// </summary>
    /// <param name="data">The byte array to checksum</param>
    /// <param name="length">The size of the array to checksum, negative values equal the size of data</param>
    public static uint Crc32(byte[] data, long length)
    {
        var crcTable = new uint[256];

        for (var i = 0; i < 256; i++)
        {
            var c = (uint)i;

            for (var j = 0; j < 8; j++) c = (c & 1) >= 1 ? 0xedb88320 ^ (c >> 1) : c >> 1;
            crcTable[i] = c;
        }

        var outputCrc = 0xFFFFFFFF;
        for (var i = 0; i < length; i++)
            outputCrc = (outputCrc >> 8) ^ crcTable[(outputCrc ^ data[i]) & 0xFF];

        return outputCrc ^ 0xFFFFFFFF;
    }
    
    public static uint Crc32(this byte[] data)
    {
        return Crc32(data, data.Length);
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
}