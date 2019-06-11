using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinishRandomizer.Core;
using MinishRandomizer.Utilities;

namespace MinishRandomizer.Randomizer
{
    public class Location
    {
        public enum Region
        {
            Overworld,
            Deepwood,
            Flames,
            WindFort,
            Droplets,
            WindTemple
        }

        public byte Type;
        public Region SubRegion;
        public bool Large;
        public Item DefaultContents { get; private set; }
        public Item Contents;
        public int Address { get; private set; }

        public Location(byte type, Region region, int address, bool large, Item contents)
        {
            Type = type;
            DefaultContents = contents;
            Contents = contents;
            Address = address;
            Large = large;
        }

        public void WriteLocation(Writer r)
        {
            r.SetPosition(Address);
            r.WriteByte((byte)Contents.Type);
            if (Large)
            {
                r.WriteByte(Contents.SubValue);
            }
        }
    }
}
