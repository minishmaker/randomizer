using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomizerCore.Randomizer.Logic.Options
{
	/// <summary>
	/// Interface meant to be used by any wrappers of any logic option base. Allows the options to tell their parents to refresh data on updates.
	/// </summary>
	public interface ILogicOptionObserver
	{
		void NotifyObserver();
	}
}
