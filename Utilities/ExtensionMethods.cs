using System;
using System.Collections;
using System.Collections.Generic;

namespace MinishRandomizer.Utilities
{
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
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Convert a BitArray to a byte[]
        /// </summary>
        /// <param name="array">The BitArray to convert</param>
        /// <param name="outputArray">The byte[] to output to</param>
        /// <returns>The number of bits used in the last byte</returns>
        public static byte[] ToByteArray(this BitArray array)
        {
            // Store lengths for later
            int inputLength = array.Length;
            int outputLength = (int)Math.Ceiling(inputLength / 8.0);

            byte[] outputArray = new byte[outputLength];

            // The number of used bits in the final byte
            int finalByteBits = inputLength - (outputLength - 1) * 8;

            // Convert groups of eight bits into a byte
            for (int i = 0; i < outputLength; i++)
            {
                int bitStart = i * 8;

                int bitCount = 8;

                if (i == outputLength - 1)
                {
                    // Don't need all of the bits anymore
                    bitCount = finalByteBits;
                }

                // Probably useless initialization
                outputArray[i] = 0;

                // OR bits in one by one
                for (int j = 0; j < bitCount; j++)
                {
                    if (array[bitStart + j])
                    {
                        outputArray[i] |= (byte)(1 << j);
                    }
                }
            }

            return outputArray;
        }
    }
}
