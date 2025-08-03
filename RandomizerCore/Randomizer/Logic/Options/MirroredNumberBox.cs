using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;

namespace RandomizerCore.Randomizer.Logic.Options;

public class MirroredNumberBox : LogicNumberBox
{
    public LogicNumberBox OriginalSetting { get; }

    public MirroredNumberBox(
        string name,
        string niceName,
        string settingGroup,
        string settingPage,
        byte defaultValue,
        byte minimumValue,
        byte maximumValue,
        string descriptionText,
        LogicOptionType type,
        LogicNumberBox originalSetting) :
        base(name, niceName, settingGroup, settingPage, defaultValue, minimumValue, maximumValue, descriptionText, type)
    {
        OriginalSetting = originalSetting;
    }

    public override void NotifyChildren()
    {
        UpdateChildren([Value.ToString()]);
    }

    public override List<LogicDefine> GetLogicDefines()
    {
        return OriginalSetting.GetLogicDefines();
    }

    public override IList<LogicOptionBase> GetChildren()
    {
        return [OriginalSetting];
    }
}
