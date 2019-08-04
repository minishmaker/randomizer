using System;
using System.Collections.Generic;
using System.Linq;
namespace MinishRandomizer.Utilities
{
    class StringUtil
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

        public static string AsStringGBAAddress(int val)
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
    }
}
