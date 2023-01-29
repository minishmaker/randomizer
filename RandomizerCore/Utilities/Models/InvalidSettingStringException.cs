using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomizerCore.Utilities.Models
{
	internal class InvalidSettingStringException : Exception
	{
		public InvalidSettingStringException(string message) : base(message) { }
	}
}
