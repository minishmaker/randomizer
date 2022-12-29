using System;
using System.Text;

namespace ColorzCore.IO
{
	internal static class IOUtility
	{
		public static string UnescapeString(string param)
		{
			var sb = new StringBuilder(param);
			return sb.Replace("\\t", "\t").Replace("\\n", "\n").Replace("\\\\", "\\").Replace("\\r", "\r").ToString();
		}

		public static string UnescapePath(string param)
		{
			var sb = new StringBuilder(param);
			return sb.Replace("\\ ", " ").Replace("\\\\", "\\").ToString();
		}

		public static string GetToolFileName(string name)
		{
			switch (Environment.OSVersion.Platform)
			{
				case PlatformID.Win32Windows: // Who knows, maybe someone runs EA on win 95
				case PlatformID.Win32NT:
					return name + ".exe";

				default:
					return name;
			}
		}
	}
}
