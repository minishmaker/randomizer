using System;
using System.Collections.Generic;
using System.Globalization;
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
        public static Location GetLocation(string locationText)
        {
            // Location format: Type;Name;Address;Large;Logic
            string[] locationParts = locationText.Split(';');

            if (locationParts.Length < 4)
            {
                return new Location(LocationType.Untyped, "INVALID LOCATION", 0, false, null);
            }

            string name = locationParts[0];

            string locationType = locationParts[1];
            if (!Enum.TryParse(locationType, out LocationType type) || type == LocationType.Untyped)
            {
                return new Location(LocationType.Untyped, "INVALID LOCATION", 0, false, null);
            }

            int address = GetAddressFromString(locationParts[2]);

            string largeText = locationParts[3];
            if (!bool.TryParse(largeText, out bool large))
            {
                large = false;
            }

            string logic = "";
            if (locationParts.Length >= 5)
            {
                logic = locationParts[4];
            }

            List<Dependency> dependencies = Dependency.GetDependencies(logic);

            return new Location(type, name, address, large, dependencies);
        }

        public static int GetAddressFromString(string addressString)
        {
            // Either direct address or area-room-chest
            if (int.TryParse(addressString, NumberStyles.HexNumber, null, out int address))
            {
                return address;
            }
            
            string[] chestDetails = addressString.Split('-');
            if (chestDetails.Length != 3)
            {
                return 0;
            }

            if (!int.TryParse(chestDetails[0], NumberStyles.HexNumber, null, out int area))
            {
                return 0;
            }

            if (!int.TryParse(chestDetails[1], NumberStyles.HexNumber, null, out int room))
            {
                return 0;
            }

            if (!int.TryParse(chestDetails[2], NumberStyles.HexNumber, null, out int chest))
            {
                return 0;
            }

            int areaTableAddr = ROM.Instance.reader.ReadAddr(ROM.Instance.headers.AreaMetadataBase + (area << 2));
            int roomTableAddr = ROM.Instance.reader.ReadAddr(areaTableAddr + (room << 2));
            int chestTableAddr = ROM.Instance.reader.ReadAddr(roomTableAddr + 0x0C);

            return chestTableAddr + chest * 8 + 0x02;
        }

        public enum LocationType
        {
            Untyped,
            Normal,
            Minor,
            DungeonItem,
            NPCItem,
            KinstoneItem,
            HeartPieceItem,
            HelperItem
        }

        public List<Dependency> Dependencies;
        public LocationType Type;
        public string Name;
        public bool Filled;
        public Item Contents { get; private set; }
        private bool Large;
        private Item DefaultContents;
        private int Address;

        public Location(LocationType type, string name, int address, bool large, List<Dependency> dependencies)
        {
            Type = type;
            Name = name;

            Address = address;

            Large = large;

            Dependencies = dependencies;

            if (address != 0)
            {
                DefaultContents = GetItemContents();
                Contents = DefaultContents;
            }

            Filled = false;
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

        public Item GetItemContents()
        {
            ItemType type = (ItemType)ROM.Instance.reader.ReadByte(Address);
            byte subType = 0;
            if (Large)
            {
                subType = ROM.Instance.reader.ReadByte();
            }
            
            return new Item(type, subType);
        }

        public bool IsAccessible(List<Item> unplacedItems)
        {
            foreach (Dependency dependency in Dependencies)
            {
                if (!dependency.DependencyFulfilled(unplacedItems))
                {
                    return false;
                }
            }

            return true;
        }

        public void Fill(Item contents)
        {
            Contents = contents;
            Filled = true;
        }
    }
}
