namespace MinishCapRandomizerUI.UI.Config
{
	/// <summary>
	/// Class that is used for the JSON representation of config data
	/// </summary>
	internal class UIConfiguration
	{
		public string RomPath { get; set; } = "";

		public bool UseHendrusShuffler { get; set; }

		public int MaximumRandomizationRetryCount { get; set; }

		public string DefaultLoggerPath { get; set; } = "";

		public bool UseVerboseLogger { get; set; }

		public bool UseCustomLogic { get; set; }

		public string CustomLogicFilepath { get; set; } = "";

		public bool UseCustomPatch { get; set; }

		public string CustomPatchFilepath { get; set; } = "";

        public bool CheckForUpdatesOnStart { get; set; } = true;

		public bool UseCustomYAML { get; set; }

		public string CustomYAMLFilepath { get; set; } = "";
	}
}
