using System.Text;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;
using RandomizerCore.Utilities.Extensions;

namespace RandomizerCore.Randomizer.Logic.Options;

public class LogicDropdown : LogicOptionBase
{
    public string Selection;
    public Dictionary<string, string> Selections;

    public LogicDropdown(string name, string niceName, string settingGroup, string settingPage, LogicOptionType type,
        Dictionary<string, string> selections) :
        base(name, niceName, true, settingGroup, settingPage, type)
    {
        Selections = selections;
        Selection = selections.Keys.First();
    }

    public override List<LogicDefine> GetLogicDefines()
    {
        var defineList = new List<LogicDefine>(3);

        // Only true if a color has been selected
        if (!Active) return defineList;

#if DEBUG
        Console.WriteLine("Activedefine");
#endif

        if (!Selections.TryGetValue(Selection, out var content)) return defineList;

        Console.WriteLine(Name);
        defineList.Add(new LogicDefine(Name, content));

        return defineList;
    }

    public override byte GetHashByte()
    {
        return Active ? Encoding.ASCII.GetBytes(Selection).Crc8() : (byte)0x0b;
    }
}