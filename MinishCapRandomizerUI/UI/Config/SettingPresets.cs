namespace MinishCapRandomizerUI.UI.Config
{
	public record SettingPresets
	{
		public List<PresetFileInfo> SettingsPresets { get; set; } = [];

		public List<PresetFileInfo> CosmeticsPresets { get; set; } = [];

		public List<PresetFileInfo> SettingsWeights { get; set; } = [];

		public List<PresetFileInfo> CosmeticsWeights { get; set; } = [];
	}
}
