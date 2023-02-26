using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;
using RandomizerCore.Utilities.Logging;

namespace RandomizerCore.Randomizer.Logic.Options;

public class LogicNumberBox : LogicOptionBase
{
    public LogicNumberBox(
        string name,
        string niceName,
        string settingGroup,
        string settingPage,
        byte defaultValue,
        byte minimumValue,
        byte maximumValue,
        string descriptionText,
        LogicOptionType type) :
        base(name, niceName, true, settingGroup, settingPage, descriptionText, type)
    {
        MinValue = minimumValue;
        MaxValue = maximumValue;
        DefaultValue = defaultValue;
        Value = $"{DefaultValue}";
    }

    public string Value { get; set; }
    public byte MinValue { get; }
    public byte MaxValue { get; }
    public byte DefaultValue { get; }

    public override void Reset()
    {
        Value = $"{DefaultValue}";
    }

    public override List<LogicDefine> GetLogicDefines()
    {
        var defineList = new List<LogicDefine>(3);

        // Only true if valid text has been entered
        if (Value != "")
        {
            Logger.Instance.LogInfo($"Number box name: {Name}");
            defineList.Add(new LogicDefine(Name, Value));
        }
        else
        {
            defineList.Add(new LogicDefine(Name, "0"));
        }

        return defineList;
    }

    public override byte GetHashByte()
    {
        return Value != "" ? byte.Parse(Value) : (byte)0;
    }

    public override string GetOptions()
    {
        return "A number between 0 and 255";
    }

    public override string GetOptionUiType()
    {
        return "Number Box";
    }
}
