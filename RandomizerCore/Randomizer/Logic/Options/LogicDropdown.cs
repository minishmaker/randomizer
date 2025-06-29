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
        Dictionary<string, string> selections) :
        base(name, niceName, settingGroup, settingPage, descriptionText, type)
    {
        Selections = selections;
        DefaultSelection = defaultSelection;
        Selection = selections.Keys.ToList()[
            selections.Values.ToList()
                .IndexOf(defaultSelection)];
    }

    public override void Reset()
    {
        Selection = Selections.Keys.ToList()[Selections.Values.ToList().IndexOf(DefaultSelection)];
    }

    public override void CopyValueFrom(LogicOptionBase option)
    {
        var dropdown = (LogicDropdown)option;
        if (!Selections.ContainsKey(dropdown.Selection))
            throw new Exception($"Attempt to load option {Name} failed! Invalid value {dropdown.Selection}");
        Selection = dropdown.Selection;
    }

    // TODO: This could use some refactoring. The dictionary is not guaranteed to keep the intended order of the options, and it is not intuitive which side represents what.
    public string Selection { get; set; } // must be a key
    public string DefaultSelection { get; } // must be a value
    public Dictionary<string, string> Selections { get; }

    public override List<LogicDefine> GetLogicDefines()
    {
        Logger.Instance.LogInfo($"Active Define: {NiceName}, Value: {Selection}");
        return [new LogicDefine(Name, Selections[Selection])];
    }

    public override IEnumerable<byte> GetAdditionalHashBytes()
    {
        List<byte> b = [(byte)Selections.Count];
        b.AddRange(Selections.SelectMany(option => Encoding.UTF8.GetBytes(option.Value)));
        return b;
    }

    public override byte GetSelectionHashByte()
    {
        return Encoding.ASCII.GetBytes(Selection).Crc8();
    }

    public override string GetOptions()
    {
        var builder = new StringBuilder();
        foreach (var selection in Selections)
            builder.Append("{Key: ").Append(selection.Key).Append(" Value: ").Append(selection.Value).Append("}, ");

        return builder.ToString();
    }

    public override string GetOptionUiType()
    {
        return "Dropdown";
    }
}
