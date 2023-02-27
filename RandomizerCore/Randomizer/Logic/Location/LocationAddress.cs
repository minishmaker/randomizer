using RandomizerCore.Core;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.IO;

namespace RandomizerCore.Randomizer.Logic.Location;

public class LocationAddress
{
    public int Address;
    public AddressType Type;

    public LocationAddress(AddressType type, int address = 0)
    {
        Address = address;
        Type = type;
    }

    public virtual void WriteAddress(Writer w, Item item)
    {
        // Shouldn't write anything to address 0
        if (Address == 0) return;

        // Always start writing at the address
        w.SetPosition(Address);

        // If the first byte is to be written to the address, write it
        if ((Type & AddressType.FirstByte) == AddressType.FirstByte) w.WriteByte((byte)item.Type);

        // If the second byte is to be written to the address, write it
        // Note that if the first byte was written the second will be written at Address+1
        if ((Type & AddressType.SecondByte) == AddressType.SecondByte) w.WriteByte(item.SubValue);
    }

    /// <summary>
    ///     Get all valid information contained at the address
    /// </summary>
    /// <param name="r">The reader to read the data from</param>
    /// <param name="type">The type of the item contained at the address</param>
    /// <param name="subValue">The subvalue contained at the address</param>
    public virtual void ReadAddress(Reader r, ref ItemType type, ref byte subValue)
    {
        // Shouldn't read anything from address 0
        if (Address == 0) return;

        // Read at the given address
        r.SetPosition(Address);

        // If the address is valid for the first byte, read it
        if ((Type & AddressType.FirstByte) == AddressType.FirstByte) type = (ItemType)r.ReadByte();

        // If the address is valid for the second byte, read it
        // Note that if the first byte was read the second will be read from Address+1
        if ((Type & AddressType.SecondByte) == AddressType.SecondByte) subValue = r.ReadByte();
    }
}
