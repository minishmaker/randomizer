using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;

namespace RandomizerCore.Randomizer.Logic.Options;

public class CompactFlag : LogicFlag
{
    public LogicOptionBase[] OriginalSettings { get; }
    public string[] OriginalOptionsFalse { get; }
    public string[] OriginalOptionsTrue { get; }

    public CompactFlag(
        string name,
        string niceName,
        bool activeDefault,
        string settingGroup,
        string settingPage,
        string descriptionText,
        LogicOptionType type,
        LogicOptionBase[] originalSettings,
        string[] originalOptionsFalse,
        string[] originalOptionsTrue) :
        base(name, niceName, activeDefault, settingGroup, settingPage, descriptionText, type)
    {
        OriginalSettings = originalSettings;
        OriginalOptionsFalse = originalOptionsFalse;
        OriginalOptionsTrue = originalOptionsTrue;
    }

    public override void NotifyChildren()
    {
        UpdateChildren(Active ? OriginalOptionsTrue : OriginalOptionsFalse);
    }

    public override List<LogicDefine> GetLogicDefines()
    {
        return OriginalSettings.SelectMany(setting => setting.GetLogicDefines()).ToList();
    }

    public override IList<LogicOptionBase> GetChildren()
    {
        return OriginalSettings;
    }
}
