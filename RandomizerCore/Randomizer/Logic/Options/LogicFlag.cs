using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;

namespace RandomizerCore.Randomizer.Logic.Options;

public class LogicFlag : LogicOptionBase
{
    public LogicFlag(
        string name, 
        string niceName, 
        bool active, 
        string settingGroup, 
        string settingPage, 
        string descriptionText,
        LogicOptionType type) : 
        base(name, niceName, active, settingGroup, settingPage, descriptionText, type)
    {
    }

    public override List<LogicDefine> GetLogicDefines()
    {
        var defineList = new List<LogicDefine>(1);

        // Only define the new thing if the flag is ticked
        if (Active) defineList.Add(new LogicDefine(Name));

        return defineList;
    }

    public override byte GetHashByte()
    {
        return Active ? (byte)01 : (byte)00;
    }

    public override string GetOptions()
    {
        return "True or False";
    }

    public override string GetOptionUIType()
    {
        return "Flag";
    }
}