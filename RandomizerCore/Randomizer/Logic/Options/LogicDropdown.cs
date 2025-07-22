using System.Collections.Immutable;
using System.Text;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer.Logic.Options;

public class LogicDropdown : LogicOptionBase
{
    public LogicDropdown(
        string name,
        string niceName,
        string settingGroup,
        string settingPage,
        string descriptionText,
        string defaultSelection,
        LogicOptionType type,
        string[] optionDisplayNames,
        string[] options) :
        base(name, niceName, settingGroup, settingPage, descriptionText, type)
    {
        SelectionOptionNames = optionDisplayNames;
        SelectionOptions = options;
        DefaultSelection = defaultSelection;
        Selection = defaultSelection;
        OptionsToNames = Enumerable.Range(0, options.Length).ToImmutableDictionary(i => options[i], i => optionDisplayNames[i]);
        NamesToOptions = Enumerable.Range(0, options.Length).ToImmutableDictionary(i => optionDisplayNames[i], i => options[i]);
    }

    public override void Reset()
    {
        Selection = DefaultSelection;
    }

    public override void CopyValueFrom(LogicOptionBase option)
    {
        var dropdown = (LogicDropdown)option;
        if (!SelectionOptions.Contains(dropdown.Selection))
            throw new Exception($"Attempt to load option {Name} failed! Invalid value {dropdown.Selection}");
        Selection = dropdown.Selection;
    }

    public string Selection { get; set; }
    public string DefaultSelection { get; }
    public string[] SelectionOptionNames { get; }
    public string[] SelectionOptions { get; }
    public ImmutableDictionary<string, string> OptionsToNames { get; }
    public ImmutableDictionary<string, string> NamesToOptions { get; }

    public override List<LogicDefine> GetLogicDefines()
    {
        Logger.Instance.LogInfo($"Active Define: {NiceName}, Value: {Selection}");
        return [new LogicDefine(Name, Selection)];
    }

    public override IEnumerable<byte> GetAdditionalHashBytes()
    {
        List<byte> b = [(byte)SelectionOptions.Length];
        b.AddRange(SelectionOptions.SelectMany(Encoding.UTF8.GetBytes));
        return b;
    }

    public override byte GetSelectionHashByte()
    {
        return Encoding.ASCII.GetBytes(Selection).Crc8();
    }

    public override string GetOptions()
    {
        var builder = new StringBuilder();
        for (var i = 0; i < SelectionOptions.Length; i++)
            builder.Append("{Key: ").Append(SelectionOptionNames[i]).Append(" Value: ").Append(SelectionOptions[i]).Append("}, ");

        return builder.ToString();
    }

    public override string GetOptionUiType()
    {
        return "Dropdown";
    }
}
