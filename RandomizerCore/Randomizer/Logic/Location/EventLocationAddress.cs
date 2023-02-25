using System.Text;
using RandomizerCore.Core;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.IO;
using RandomizerCore.Utilities.Models;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer.Logic.Location;

/// <summary>
///     A specific type of location address that writes a define to an event file instead of writing to the ROM
/// </summary>
public class EventLocationAddress : LocationAddress
{
    public EventDefine Define;

    public EventLocationAddress(AddressType type, string name) : base(type)
    {
        Define = new EventDefine(name);
    }

    public override void WriteAddress(Writer w, Item item)
    {
    }

    public override void ReadAddress(Reader r, ref ItemType type, ref byte subValue)
    {
    }

    /// <summary>
    ///     Write the define and item value to the event file
    /// </summary>
    /// <param name="w">A writer to the event stream</param>
    /// <param name="item">The item to write to the define</param>
    public void WriteDefine(StringBuilder stringBuilder, Item item)
    {
        if ((Type & AddressType.FirstByte) == AddressType.FirstByte)
            // Write the hex representation of the item ID to the define
            Define.WriteDefineString(stringBuilder, "0x" + StringUtil.AsStringHex2((byte)item.Type));

        if ((Type & AddressType.SecondByte) == AddressType.SecondByte)
            // Write the hex representation of the subvalue
            // Probably a very bad thing if the first was also written, might kill EA
            Define.WriteDefineString(stringBuilder, "0x" + StringUtil.AsStringHex2(item.SubValue));
    }
}
