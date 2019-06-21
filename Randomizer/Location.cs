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

            if (locationParts.Length < 3)
            {
                Console.WriteLine("Too short");
                return new Location(LocationType.Untyped, "INVALID LOCATION", 0, null);
            }

            string name = locationParts[0];

            string locationType = locationParts[1];
            if (!Enum.TryParse(locationType, out LocationType type) || type == LocationType.Untyped)
            {
                Console.WriteLine("Invalid type");
                return new Location(LocationType.Untyped, "INVALID LOCATION", 0, null);
            }

            int address = GetAddressFromString(locationParts[2]);

            string logic = "";
            if (locationParts.Length >= 4)
            {
                logic = locationParts[3];
            }

            List<Dependency> dependencies = Dependency.GetDependencies(logic);

            Location location = new Location(type, name, address, dependencies);

            if (locationParts.Length >= 5)
            {
                string[] subParts = locationParts[4].Split('.');

                if (subParts[0] == "Items")
                {
                    if (Enum.TryParse(subParts[1], out ItemType replacementType))
                    {
                        byte subType = 0;
                        if (subParts.Length >= 3)
                        {
                            if (!byte.TryParse(subParts[2], NumberStyles.HexNumber, null, out subType))
                            {
                                if (Enum.TryParse(subParts[2], out KinstoneType subKinstoneType))
                                {
                                    subType = (byte)subKinstoneType;
                                }
                            }
                        }

                        location.SetItem(new Item(replacementType, subType));
                    }
                }
            }

            return location;
        }

        public static int GetAddressFromString(string addressString)
        {
            // Either direct address or area-room-chest
            if (addressString == "")
            {
                return 0;
            }

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
            JabberNonsense,
            Helper,
            StartingItem
        }

        public List<Dependency> Dependencies;
        public LocationType Type;
        public string Name;
        public bool Filled;
        public Item Contents { get; private set; }
        private Item DefaultContents;
        private int Address;

        public Location(LocationType type, string name, int address, List<Dependency> dependencies)
        {
            Type = type;
            Name = name;

            Address = address;

            Dependencies = dependencies;

            if (address != 0)
            {
                DefaultContents = GetItemContents();
                Contents = DefaultContents;
            }

            Filled = false;
        }

        public void WriteLocation(Writer w)
        {
            if (Address == 0)
            {
                return;
            }

            switch (Type)
            {
                case LocationType.Helper:
                case LocationType.Untyped:
                    return;
                case LocationType.JabberNonsense:
                    w.WriteByte((byte)Contents.Type, Address);
                    w.WriteByte(Contents.SubValue, Address + 2);
                    break;
                case LocationType.StartingItem:
                    // Nonfunctional in new patches
                    break;
                    w.SetPosition(0xEF3348 + ((byte)Contents.Type >> 2)); // Get items index in the starting item table
                    byte initialByte = ROM.Instance.reader.ReadByte(0xEF3348 + ((byte)Contents.Type >> 2));
                    initialByte |= (byte)(1 << ((byte)Contents.Type & 3) * 2);
                    w.WriteByte(initialByte);
                    break;
                case LocationType.Normal:
                case LocationType.Minor:
                default:
                    
                    w.SetPosition(Address);
                    w.WriteByte((byte)Contents.Type);
                    w.WriteByte(Contents.SubValue);
                    break;
            }
        }

        public Item GetItemContents()
        {
            ItemType type = ItemType.Untyped;
            byte subType = 0;

            switch (Type)
            {
                case LocationType.JabberNonsense:
                    type = (ItemType)ROM.Instance.reader.ReadByte(Address);
                    subType = ROM.Instance.reader.ReadByte(Address + 2);
                    break;
                default:
                    type = (ItemType)ROM.Instance.reader.ReadByte(Address);
                    subType = ROM.Instance.reader.ReadByte();
                    break;
            }

            
            
            return new Item(type, subType);
        }

        public bool CanPlace(Item itemToPlace, List<Item> availableItems, List<Location> locations)
        {
            switch (Type)
            {
                case LocationType.Helper:
                case LocationType.Untyped:
                    return false;
            }

            if (Address == 0)
            {
                return false;
            }

            return IsAccessible(availableItems, locations);
        }

        public bool IsAccessible(List<Item> availableItems, List<Location> locations)
        {
            Console.WriteLine($"Evaluating: {Name}");
            foreach (Dependency dependency in Dependencies)
            {
                if (!dependency.DependencyFulfilled(availableItems, locations))
                {
                    Console.WriteLine("Unavailable :(");
                    return false;
                }
            }

            Console.WriteLine("Available!");
            return true;
        }

        public void Fill(Item contents)
        {
            SetItem(contents);
            Filled = true;
        }

        public void SetItem(Item contents)
        {
            Contents = contents;
        }
    }
}
