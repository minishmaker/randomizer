using System;
using System.IO;
using System.Linq;

namespace MinishRandomizer.Utilities
{
    static class PatchUtil
    {
        /// <summary>
        /// Apply a UPS patch to a byte array, in place
        /// </summary>
        /// <param name="inputData">The data to patch</param>
        /// <param name="patchData">The patch to apply</param>
        public static void ApplyUPS(byte[] inputData, byte[] patchData, bool checkCRC = true)
        {
            using (MemoryStream inputStream = new MemoryStream(inputData))
            {
                Reader dataReader = new Reader(inputStream);
                Writer dataWriter = new Writer(inputStream);

                using (MemoryStream patchStream = new MemoryStream(patchData))
                {
                    Reader patchReader = new Reader(patchStream);

                    patchReader.SetPosition(patchData.Length - 12);
                    uint inputCRC = patchReader.ReadUInt32();
                    uint outputCRC = patchReader.ReadUInt32();
                    uint patchCRC = patchReader.ReadUInt32();

                    patchReader.SetPosition(0);

                    // Check for magic number
                    if (!Enumerable.SequenceEqual(patchReader.ReadBytes(4), new byte[] { 0x55, 0x50, 0x53, 0x31 }))
                    {
                        Console.WriteLine("Invalid patch: Lacks magic number!");
                        return;
                    }

                    long inputSize = ReadVWI(patchReader);
                    long outputSize = ReadVWI(patchReader);

                    if (outputSize > inputData.Length)
                    {
                        Console.WriteLine("Problem: Can't change ROM size while patching in this implementation.");
                        return;
                    }

                    if (checkCRC && Crc32(inputData, inputSize) != inputCRC)
                    {
                        Console.WriteLine("Input ROM has an invalid CRC!");
                        return;
                    }

                    if (checkCRC && Crc32(patchData, patchData.Length - 4) != patchCRC)
                    {
                        Console.WriteLine("Patch has an invalid CRC!");
                        return;
                    }

                    dataReader.SetPosition(0);
                    dataWriter.SetPosition(0);

                    while (patchReader.Position < patchData.Length - 12)
                    {
                        long skippedBytes = ReadVWI(patchReader);
                        dataReader.SetPosition(dataReader.Position + skippedBytes);

                        byte nextPatch = patchReader.ReadByte();
                        while (nextPatch != 0)
                        {
                            byte nextInput = dataReader.PeekByte();

                            dataWriter.WriteByte((byte)(nextInput ^ nextPatch));

                            nextPatch = patchReader.ReadByte();
                        }

                        dataReader.ReadByte(); // Advance the read/write head one byte
                    }

                    if (Crc32(inputData, outputSize) != outputCRC)
                    {
                        Console.WriteLine("Output ROM has an invalid CRC! ...but the array was already patched!");
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Read a variable-width integer in UPS and BPS format, adapted from https://www.romhacking.net/patch/crc32
        /// </summary>
        /// <param name="r">The reader to read the integer from</param>
        /// <returns>The value of the integer, restricted to being a 64-bit signed integer</returns>
        private static long ReadVWI(Reader r)
        {
            byte nextByte;
            long output = 0;
            int shift = 1;
            do
            {
                nextByte = r.ReadByte();

                output += (nextByte & 0x7F) * shift;

                shift <<= 7;
                output += shift;
            }
            while ((nextByte & 0x80) == 0);

            output -= shift;

            return output;
        }

        private static void WriteVWI(Writer w)
        {

        }

        /// <summary>
        /// Find the crc32 of a byte array, adapted from https://www.romhacking.net/patch/crc32
        /// </summary>
        /// <param name="data">The byte array to checksum</param>
        /// <param name="length">The size of the array to checksum, negative values equal the size of data</param>
        public static uint Crc32(byte[] data, long length)
        {
            uint[] crcTable = new uint[256];

            for (int i = 0; i < 256; i++)
            {
                uint c = (uint)i;

                for (int j = 0; j < 8; j++)
                {
                    c = ((c & 1) >= 1 ? (0xedb88320 ^ (c >> 1)) : (c >> 1));
                }
                crcTable[i] = c;
            }

            uint outputCRC = 0xFFFFFFFF;
            for (int i = 0; i < length; i++)
            {
                //Console.WriteLine(StringUtil.AsStringHex8((int)outputCRC));
                outputCRC = (outputCRC >> 8) ^ crcTable[(outputCRC ^ data[i]) & 0xFF];
            }

            return outputCRC ^ 0xFFFFFFFF;
        }
    }
}
