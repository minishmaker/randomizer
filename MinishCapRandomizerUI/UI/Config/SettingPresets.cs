namespace MinishCapRandomizerUI.UI.Config
{
	public class SettingPresets
	{
		public Dictionary<uint, List<Preset>> SettingsPresets { get; set; } = new Dictionary<uint, List<Preset>>();

		public Dictionary<uint, List<Preset>> CosmeticsPresets { get; set; } = new Dictionary<uint, List<Preset>>();
	}
}
