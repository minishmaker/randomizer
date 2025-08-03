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
        base(name, niceName, settingGroup, settingPage, descriptionText, type)
    {
        MinValue = minimumValue;
        MaxValue = maximumValue;
        DefaultValue = defaultValue;
        Value = defaultValue;
    }

    public byte Value { get; set; }
    public byte MinValue { get; }
    public byte MaxValue { get; }
    public byte DefaultValue { get; }

    public override bool IsReset()
    {
        return Value == DefaultValue;
    }

    public override void Reset()
    {
        Value = DefaultValue;
    }

    public override void CopyValueFrom(LogicOptionBase option)
    {
        LogicNumberBox numberBox = (LogicNumberBox)option;
        if (numberBox.Value < MinValue || numberBox.Value > MaxValue)
            throw new Exception($"Attempt to load option {Name} failed! Invalid value {numberBox.Value}");
        Value = numberBox.Value;
    }

    public override List<LogicDefine> GetLogicDefines()
    {
        Logger.Instance.LogInfo($"Number box name: {Name}, Value: {Value}");
        return [new LogicDefine(Name, $"{Value}")];
    }

    public override IEnumerable<byte> GetAdditionalHashBytes()
    {
        return [MinValue, MaxValue];
    }

    public override byte GetSelectionHashByte()
    {
        return Value;
    }

    public override string GetOptions()
    {
        return $"A number between {MinValue} and {MaxValue}";
    }

    public override string GetOptionUiType()
    {
        return "Number Box";
    }

    public override string GetValueAsString()
    {
        return Value.ToString();
    }

    public override void SetValueFromString(string value)
    {
        if (!int.TryParse(value, out var number) || number < MinValue || number > MaxValue) throw new Exception($"Invalid value \"{value}\"");
        Value = (byte)number;
    }
}
