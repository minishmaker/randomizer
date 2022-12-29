using RandomizerCore.Utilities.IO;

namespace RandomizerCore.Utilities.Util;

public static class PatchUtil
{
    /// <summary>
    ///     Apply a UPS patch to a byte array, in place
    /// </summary>
    /// <param name="inputData">The data to patch</param>
    /// <param name="patchData">The patch to apply</param>
    /// <param name="checkCrc">A flag indicating whether the CRC value is verified</param>
    public static void ApplyUps(byte[] inputData, byte[] patchData, bool checkCrc = true)
    {
        using (var inputStream = new MemoryStream(inputData))
        {
            var dataReader = new Reader(inputStream);
            var dataWriter = new Writer(inputStream);

            using (var patchStream = new MemoryStream(patchData))
            {
                var patchReader = new Reader(patchStream);

                patchReader.SetPosition(patchData.Length - 12);
                var inputCrc = patchReader.ReadUInt32();
                var outputCrc = patchReader.ReadUInt32();
                var patchCrc = patchReader.ReadUInt32();

                patchReader.SetPosition(0);

                // Check for magic number
                if (!patchReader.ReadBytes(4).SequenceEqual(new byte[] { 0x55, 0x50, 0x53, 0x31 }))
                {
                    Console.WriteLine("Invalid patch: Lacks magic number!");
                    return;
                }

                var inputSize = ReadVwi(patchReader);
                var outputSize = ReadVwi(patchReader);

                if (outputSize > inputData.Length)
                {
                    Console.WriteLine("Problem: Can't change ROM size while patching in this implementation.");
                    return;
                }

                if (checkCrc && Crc32(inputData, inputSize) != inputCrc)
                {
                    Console.WriteLine("Input ROM has an invalid CRC!");
                    return;
                }

                if (checkCrc && Crc32(patchData, patchData.Length - 4) != patchCrc)
                {
                    Console.WriteLine("Patch has an invalid CRC!");
                    return;
                }

                dataReader.SetPosition(0);
                dataWriter.SetPosition(0);

                while (patchReader.Position < patchData.Length - 12)
                {
                    var skippedBytes = ReadVwi(patchReader);
                    dataReader.SetPosition(dataReader.Position + skippedBytes);

                    var nextPatch = patchReader.ReadByte();
                    while (nextPatch != 0)
                    {
                        var nextInput = dataReader.PeekByte();

                        dataWriter.WriteByte((byte)(nextInput ^ nextPatch));

                        nextPatch = patchReader.ReadByte();
                    }

                    dataReader.ReadByte(); // Advance the read/write head one byte
                }

                if (Crc32(inputData, outputSize) != outputCrc)
                    Console.WriteLine("Output ROM has an invalid CRC! ...but the array was already patched!");
            }
        }
    }

    /// <summary>
    ///     Read a variable-width integer in UPS and BPS format, adapted from https://www.romhacking.net/patch/crc32
    /// </summary>
    /// <param name="r">The reader to read the integer from</param>
    /// <returns>The value of the integer, restricted to being a 64-bit signed integer</returns>
    private static long ReadVwi(Reader r)
    {
        byte nextByte;
        long output = 0;
        var shift = 1;
        do
        {
            nextByte = r.ReadByte();

            output += (nextByte & 0x7F) * shift;

            shift <<= 7;
            output += shift;
        } while ((nextByte & 0x80) == 0);

        output -= shift;

        return output;
    }

    private static void WriteVwi(Writer w)
    {
    }

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
            //Console.WriteLine(StringUtil.AsStringHex8((int)outputCRC));
            outputCrc = (outputCrc >> 8) ^ crcTable[(outputCrc ^ data[i]) & 0xFF];

        return outputCrc ^ 0xFFFFFFFF;
    }
}