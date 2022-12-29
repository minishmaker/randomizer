using System.Collections.Generic;

namespace ColorzCore
{
	internal class EAOptions
	{
		public List<string> includePaths = new List<string>();

		public bool nocashSym;

		public bool noColoredLog;
		public bool nowarn, nomess;
		public List<string> toolsPaths = new List<string>();
		public bool werr;

		public EAOptions()
		{
			werr = false;
			nowarn = false;
			nomess = false;
			noColoredLog = false;
			nocashSym = false;
		}
	}
}
