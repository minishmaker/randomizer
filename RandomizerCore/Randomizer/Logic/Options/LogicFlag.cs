using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;

namespace RandomizerCore.Randomizer.Logic.Options;

public class LogicFlag : LogicOptionBase
{
    public bool Active { get; set; }
    public bool Default { get; }

    public LogicFlag(
        string name,
        string niceName,
        bool active,
        string settingGroup,
        string settingPage,
        string descriptionText,
        LogicOptionType type) :
        base(name, niceName, settingGroup, settingPage, descriptionText, type)
    {
        Active = active;
        Default = active;
    }

    public override void Reset()
    {
        Active = Default;
    }

    public override void CopyValueFrom(LogicOptionBase option)
    {
        Active = ((LogicFlag)option).Active;
    }

    public override List<LogicDefine> GetLogicDefines()
    {
        var defineList = new List<LogicDefine>(1);

        // Only define the new thing if the flag is ticked
        if (Active) defineList.Add(new LogicDefine(Name));

        return defineList;
    }

    public override byte GetSelectionHashByte()
    {
        return Active ? (byte)01 : (byte)00;
    }

    public override string GetOptions()
    {
        return "True or False";
    }

    public override string GetOptionUiType()
    {
        return "Flag";
    }
}
