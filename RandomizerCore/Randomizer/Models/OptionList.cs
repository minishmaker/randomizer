using System.Text;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Options;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer;

public class OptionList : List<LogicOptionBase>
{
    public OptionList() : base()
    {
    }

    public OptionList(IEnumerable<LogicOptionBase> collection) : base(collection)
    {
    }

    public OptionList(int capacity) : base(capacity)
    {
    }

    public OptionList OnlyLogic() => new OptionList(this.Where(option => option.Type is LogicOptionType.Setting));

    public OptionList OnlyCosmetic() => new OptionList(this.Where(option => option.Type is LogicOptionType.Cosmetic));

    public OptionList GetSorted()
    {
        var settings = new OptionList(this);
        settings.Sort((option1, option2) => string.CompareOrdinal(option1.Name, option2.Name));
        return settings;
    }

    public byte[] GetBytes()
    {
        var bytes = new byte[Count];

        for (var i = 0; i < Count; i++) bytes[i] = this[i].GetHashByte();

        return bytes;
    }

    public uint GetCrc32()
    {
        var bytes = new List<byte>();

        foreach (var option in this)
        {
            bytes.AddRange(Encoding.UTF8.GetBytes(option.Name));
            bytes.AddRange(Encoding.UTF8.GetBytes(option.Type.ToString()));
            bytes.AddRange(Encoding.UTF8.GetBytes(option.GetType().ToString()));
        }

        return bytes.ToArray().Crc32();
    }

    public uint GetHash()
    {
        var settingBytes = GetBytes();

        return settingBytes.Length > 0 ? settingBytes.Crc32() : 0;
    }

    public string GetIdentifier(bool splitOptionTypes = true)
    {
        return splitOptionTypes ? StringUtil.AsStringHex8((int)OnlyLogic().GetHash()) + "-" +
            StringUtil.AsStringHex8((int)OnlyCosmetic().GetHash()) : StringUtil.AsStringHex8((int)GetHash());
    }
}
