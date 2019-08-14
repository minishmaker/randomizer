using MinishRandomizer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinishRandomizer.Core;
using System.IO;

namespace MinishRandomizer.Randomizer
{
    [Flags]
    public enum AddressType
    {
        None = 0,
        FirstByte = 1,
        SecondByte = 2,
        BothBytes = FirstByte | SecondByte,
        GroundItem = 4,
        Define = 8
    }

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
            if (Address == 0)
            {
                return;
            }

            // Always start writing at the address
            w.SetPosition(Address);

            // If the first byte is to be written to the address, write it
            if ((Type & AddressType.FirstByte) == AddressType.FirstByte)
            {
                w.WriteByte((byte)item.Type);
            }

            // If the second byte is to be written to the address, write it
            // Note that if the first byte was written the second will be written at Address+1
            if ((Type & AddressType.SecondByte) == AddressType.SecondByte)
            {
                w.WriteByte(item.SubValue);
            }
        }

        /// <summary>
        /// Get all valid information contained at the address
        /// </summary>
        /// <param name="r">The reader to read the data from</param>
        /// <param name="type">The type of the item contained at the address</param>
        /// <param name="subValue">The subvalue contained at the address</param>
        public virtual void ReadAddress(Reader r, ref ItemType type, ref byte subValue)
        {
            // Shouldn't read anything from address 0
            if (Address == 0)
            {
                return;
            }

            // Read at the given address
            r.SetPosition(Address);

            // If the address is valid for the first byte, read it
            if ((Type & AddressType.FirstByte) == AddressType.FirstByte)
            {
                type = (ItemType)r.ReadByte();
            }

            // If the address is valid for the second byte, read it
            // Note that if the first byte was read the second will be read from Address+1
            if ((Type & AddressType.SecondByte) == AddressType.SecondByte)
            {
                subValue = r.ReadByte();
            }
        }
    }

    /// <summary>
    /// A specific type of location address that writes a define to an event file instead of writing to the ROM
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
            return;
        }

        public override void ReadAddress(Reader r, ref ItemType type, ref byte subValue)
        {
            return;
        }

        /// <summary>
        /// Write the define and item value to the event file
        /// </summary>
        /// <param name="w">A writer to the event stream</param>
        /// <param name="item">The item to write to the define</param>
        public void WriteDefine(StringBuilder stringBuilder, Item item)
        {
            if ((Type & AddressType.FirstByte) == AddressType.FirstByte)
            {
                // Write the hex representation of the item ID to the define
                Define.WriteDefine(stringBuilder, "0x" + StringUtil.AsStringHex2((byte)item.Type));
            }

            if ((Type & AddressType.SecondByte) == AddressType.SecondByte)
            {
                // Write the hex representation of the subvalue
                // Probably a very bad thing if the first was also written, might kill EA
                Define.WriteDefine(stringBuilder, "0x" + StringUtil.AsStringHex2(item.SubValue));
            }
        }
    }
}
