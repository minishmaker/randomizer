using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;

namespace RandomizerCore.Randomizer.Logic.Options;

public abstract class LogicOptionBase
{
    protected LogicOptionBase(string name, string niceName, bool active, string settingGroup, string settingPage,
        LogicOptionType type = LogicOptionType.Untyped)
    {
        Name = name;
        NiceName = niceName;
        Active = active;
        Type = type;
        SettingGroup = settingGroup;
        SettingPage = settingPage;
    }

    public string Name { get; set; }
    public string NiceName { get; set; }
    public bool Active { get; set; }
    public LogicOptionType Type { get; set; }
    public string SettingGroup { get; set; }
    public string SettingPage { get; set; }

    public abstract List<LogicDefine> GetLogicDefines();

    public abstract string GetOptions();

    public abstract byte GetHashByte();

    public abstract string GetOptionUIType();
}