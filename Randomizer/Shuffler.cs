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
    public readonly struct Item
    {
        public readonly ItemType Type;
        public readonly KinstoneType Kinstone;
        public readonly byte SubValue;

        public Item(ItemType type, byte subValue)
        {
            Type = type;
            SubValue = subValue;
            if (type == ItemType.KinstoneX)
            {
                Kinstone = (KinstoneType)subValue;
            }
            else
            {
                Kinstone = KinstoneType.UnTyped;
            }
        }
    }

    public class Shuffler
    {
     
        List<Location> Locations;
        List<Item> Items;

        public Shuffler()
        {

        }

        public void LoadLocations(string locationFile)
        {
            byte[] locationData;

            if (locationFile == null)
            {
                locationData = File.ReadAllBytes("");
            }
            else
            {
                locationData = File.ReadAllBytes(locationFile);
            }

            Locations = new List<Location>();
            Items = new List<Item>();

            using (MemoryStream ms = new MemoryStream(locationData))
            {
                Reader r = new Reader(ms);

                byte type = r.ReadByte();
                while (type != 0)
                {
                    Location.Region region = (Location.Region)r.ReadByte();
                    int addr = r.ReadAddr();
                    bool large = r.ReadByte() == 1;
                    ItemType itemType = (ItemType)r.ReadByte();
                    byte subValue = r.ReadByte();
                    Item item = new Item(itemType, subValue);

                    Locations.Add(new Location(type, region, addr, large, item));

                    Items.Add(item);

                    type = r.ReadByte();
                }
            }
        }

        public void LoadLogic(string logicFile)
        {

        }

        public void RandomizeLocations()
        {
            
        }
    }
}
