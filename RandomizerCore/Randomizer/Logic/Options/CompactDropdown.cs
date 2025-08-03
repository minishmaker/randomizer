using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;
using RandomizerCore.Utilities.Logging;

namespace RandomizerCore.Randomizer.Logic.Options;

public class CompactDropdown : LogicDropdown
{
    public LogicOptionBase[] OriginalSettings { get; }
    public string[][] OriginalOptions { get; }

    public CompactDropdown(
        string name,
        string niceName,
        string settingGroup,
        string settingPage,
        string descriptionText,
        string defaultSelection,
        LogicOptionType type,
        string[] optionDisplayNames,
        string[] options,
        LogicOptionBase[] originalSettings,
        string[][] originalOptions) :
        base(name, niceName, settingGroup, settingPage, descriptionText, defaultSelection, type, optionDisplayNames, options)
    {
        OriginalSettings = originalSettings;
        OriginalOptions = originalOptions;
    }

    public override void NotifyChildren()
    {
        var selectedIndex = SelectionOptions.ToList().IndexOf(Selection);
        UpdateChildren(OriginalOptions[selectedIndex]);
    }

    public override List<LogicDefine> GetLogicDefines()
    {
        Logger.Instance.LogInfo($"Active Define: {NiceName}, Value: {Selection}");
        return OriginalSettings.SelectMany(setting => setting.GetLogicDefines()).ToList();
    }

    public override IList<LogicOptionBase> GetChildren()
    {
        return OriginalSettings;
    }
}
