using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;

namespace RandomizerCore.Randomizer.Logic.Options;

public class LogicNumberBox : LogicOptionBase
{
    public bool isSelf = false;
    public string placeholder = "";
    private readonly string _value = "";

    public LogicNumberBox(string name, string niceName, string settingGroup, string settingPage, LogicOptionType type) :
        base(name, niceName, true, settingGroup, settingPage, type)
    {
    }

    public override List<LogicDefine> GetLogicDefines()
    {
        var defineList = new List<LogicDefine>(3);

        // Only true if valid text has been entered
        if (_value != "")
        {
            Console.WriteLine(Name);
            defineList.Add(new LogicDefine(Name, _value));
        }
        else
        {
            defineList.Add(new LogicDefine(Name, "0"));
        }

        return defineList;
    }

    public override byte GetHashByte()
    {
        return _value != "" ? byte.Parse(_value) : (byte)0;
    }
}