using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinishRandomizer.Utilities
{
	public static class ExtensionMethods
	{
		public static string Hex( this uint num )
		{
			return Convert.ToString( num, 16 ).ToUpper();
		}

		public static string Hex( this int num )
		{
			return Convert.ToString( num, 16 ).ToUpper();
		}

		public static string Hex( this long num )
		{
			return Convert.ToString( num, 16 ).ToUpper();
		}

		public static string Hex( this ushort num )
		{
			return Convert.ToString( num, 16 ).ToUpper();
		}

		public static string Hex( this byte num )
		{
			return Convert.ToString( num, 16 ).ToUpper();
		}
	}
}
