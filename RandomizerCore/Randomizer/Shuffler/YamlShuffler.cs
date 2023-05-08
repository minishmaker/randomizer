using System.Text;
using RandomizerCore.Random;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer.Shuffler;

internal class YamlShuffler : Shuffler
{
    private string _yamlNameLogic;
    private string _yamlNameCosmetics;
    private string _yamlDescriptionLogic;
    private string _yamlDescriptionCosmetics;
    private string? _yamlPathLogic;
    private string? _yamlPathCosmetics;
    private bool _yamlUseGlobal;

    public bool IsUsingLogicYaml() => !string.IsNullOrEmpty(_yamlPathLogic);

    public bool IsUsingCosmeticsYaml() => !string.IsNullOrEmpty(_yamlPathCosmetics);

    public bool IsGlobalYamlMode() => _yamlUseGlobal;

    public string GetLogicYamlName() => _yamlNameLogic;

    public string GetCosmeticsYamlName() => _yamlNameCosmetics;

    public override void LoadLocations(string? logicFile = null) => throw new NotImplementedException();
    
    /// <summary>
    ///     Reads the list of locations from a file, or the default logic if none is specified
    /// </summary>
    /// <param name="logicFile">The file to read locations from</param>
    /// <param name="yamlFileLogic">The YAML file to read logic or all options from</param>
    /// <param name="yamlFileCosmetics">The YAML file to read cosmetic options from, ignored if "useGlobalYAML" is true</param>
    /// <param name="useGlobalYaml">Whether a single YAML file (if provided) is used for all setting types</param>
    public override void LoadLocationsYaml(string? logicFile = null, string? yamlFileLogic = null, string? yamlFileCosmetics = null, bool useGlobalYaml = false)
    {
        // Change the logic file path to match
        LogicPath = logicFile;
        _yamlPathLogic = yamlFileLogic;
        _yamlPathCosmetics = useGlobalYaml ? yamlFileLogic : yamlFileCosmetics;
        _yamlUseGlobal = useGlobalYaml;

        // Reset everything to allow rerandomization
        ClearLogic();

        if (string.IsNullOrEmpty(yamlFileLogic))
        {
            if (string.IsNullOrEmpty(yamlFileCosmetics) || useGlobalYaml)
            {
                Options = new OptionList(GetSelectedOptions());
            }
            else
            {
                Options = new OptionList(GetSelectedOptions()).OnlyLogic();
                var result = Mystery.ParseYAML(File.ReadAllText(yamlFileCosmetics), GetSelectedOptions(), new SquaresRandomNumberGenerator(SquaresRandomNumberGenerator.DefaultKey, Seed));
                Options.AddRange(result.Options.OnlyCosmetic());
                _yamlNameCosmetics = result.Name;
                _yamlDescriptionCosmetics = result.Description;
            }
        }
        else
        {
            if (useGlobalYaml)
            {
                var result = Mystery.ParseYAML(File.ReadAllText(yamlFileLogic), GetSelectedOptions(), new SquaresRandomNumberGenerator(SquaresRandomNumberGenerator.DefaultKey, Seed));
                Options = result.Options;
                _yamlNameLogic = _yamlNameCosmetics = result.Name;
                _yamlDescriptionLogic = _yamlDescriptionCosmetics = result.Description;
            }
            else
            {
                if (string.IsNullOrEmpty(yamlFileCosmetics))
                {
                    Options = new OptionList(GetSelectedOptions()).OnlyCosmetic();
                    var result = Mystery.ParseYAML(File.ReadAllText(yamlFileLogic), GetSelectedOptions(), new SquaresRandomNumberGenerator(SquaresRandomNumberGenerator.DefaultKey, Seed));
                    Options.AddRange(result.Options.OnlyLogic());
                    _yamlNameLogic = result.Name;
                    _yamlDescriptionLogic = result.Description;
                }
                else
                {
                    var result = Mystery.ParseYAML(File.ReadAllText(yamlFileLogic), GetSelectedOptions(), new SquaresRandomNumberGenerator(SquaresRandomNumberGenerator.DefaultKey, Seed));
                    Options = result.Options.OnlyLogic();
                    _yamlNameLogic = result.Name;
                    _yamlDescriptionLogic = result.Description;
                    result = Mystery.ParseYAML(File.ReadAllText(yamlFileCosmetics), GetSelectedOptions(), new SquaresRandomNumberGenerator(SquaresRandomNumberGenerator.DefaultKey, Seed));
                    Options.AddRange(result.Options.OnlyCosmetic());
                    _yamlNameCosmetics = result.Name;
                    _yamlDescriptionCosmetics = result.Description;
                }
            }
        }

		// Set option defines
		LogicParser.SubParser.AddOptions(Options);
        
        base.LoadLocations(logicFile);
    }

    public override string GetEventWrites()
    {
        var eventBuilder = new StringBuilder();

        foreach (var location in Locations) location.WriteLocationEvent(eventBuilder);

        foreach (var define in LogicParser.GetEventDefines()) define.WriteDefineString(eventBuilder);

        var seedValues = new byte[8];
        seedValues[0] = (byte)((Seed >> 00) & 0xFF);
        seedValues[1] = (byte)((Seed >> 08) & 0xFF);
        seedValues[2] = (byte)((Seed >> 16) & 0xFF);
        seedValues[3] = (byte)((Seed >> 24) & 0xFF);
        seedValues[4] = (byte)((Seed >> 32) & 0xFF);
        seedValues[5] = (byte)((Seed >> 40) & 0xFF);
        seedValues[6] = (byte)((Seed >> 48) & 0xFF);
        seedValues[7] = (byte)((Seed >> 56) & 0xFF);
        var crc = (int)CrcUtil.Crc32(seedValues, 8);

        eventBuilder.AppendLine("#define seedHashed 0x" + StringUtil.AsStringHex8(crc));
        if (string.IsNullOrEmpty(_yamlPathLogic))
            eventBuilder.AppendLine("#define settingHash 0x" + StringUtil.AsStringHex8((int)GetFinalOptions().OnlyLogic().GetHash()));

        return eventBuilder.ToString();
    }
}
