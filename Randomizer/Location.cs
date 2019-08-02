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
                throw new ShuffleException("A location in the logic file lacks required fields!");
            }

            string[] names = locationParts[0].Split(':');
            string name = names[0];

            string locationType = locationParts[1];
            if (!Enum.TryParse(locationType, out LocationType type) || type == LocationType.Untyped)
            {
                Console.WriteLine("Invalid type");
                throw new ShuffleException($"Location \"{name}\" has invalid type \"{locationType}\"!");
            }

            int address = GetAddressFromString(locationParts[2]);

            string logic = "";
            if (locationParts.Length >= 4)
            {
                logic = locationParts[3];
            }

            List<Dependency> dependencies = Dependency.GetDependencies(logic);

            string dungeon = "";
            if (names.Length >= 2)
            {
                dungeon = names[1];
            }

            Location location = new Location(type, name, dungeon, address, dependencies);

            if (locationParts.Length >= 5)
            {
                string[] itemParts = locationParts[4].Split(':');
                string[] subParts = itemParts[0].Split('.');

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

                        string itemDungeon = "";

                        if (type == LocationType.Unshuffled)
                        {
                            itemDungeon = dungeon;
                        }

                        if (itemParts.Length >= 2)
                        {
                            itemDungeon = itemParts[1];
                        }

                        location.SetItem(new Item(replacementType, subType, itemDungeon));
                    }
                }
            }

            return location;
        }

        public static List<Item> GetItems(List<Location> locations)
        {
            List<Item> items = new List<Item>();

            foreach (Location location in locations)
            {
                items.Add(location.Contents);
            }

            return items;
        }

        /// <summary>
        /// Turn an address string into a file address
        /// </summary>
        /// <param name="addressString">String representing the address</param>
        /// <returns></returns>
        public static int GetAddressFromString(string addressString)
        {
            // Either direct address or area-room-chest
            if (addressString == "")
            {
                return 0;
            }

            // The address is a hexadecimal number, so it can simply be parsed
            if (int.TryParse(addressString, NumberStyles.HexNumber, null, out int address))
            {
                return address;
            }
            
            // The address is a chest, so it should
            string[] chestDetails = addressString.Split('-');
            if (chestDetails.Length != 3)
            {
                throw new ShuffleException($"Chest data \"{addressString}\" does not have a full address!");
            }

            if (!int.TryParse(chestDetails[0], NumberStyles.HexNumber, null, out int area))
            {
                throw new ShuffleException($"Chest data \"{addressString}\" has an invalid area index!");
            }

            if (!int.TryParse(chestDetails[1], NumberStyles.HexNumber, null, out int room))
            {
                throw new ShuffleException($"Chest data \"{addressString}\" has an invalid room index!");
            }

            if (!int.TryParse(chestDetails[2], NumberStyles.HexNumber, null, out int chest))
            {
                throw new ShuffleException($"Chest data \"{addressString}\" has an invalid chest index!");
            }

            // Look chest address up in table
            int areaTableAddr = ROM.Instance.reader.ReadAddr(ROM.Instance.headers.AreaMetadataBase + (area << 2));
            int roomTableAddr = ROM.Instance.reader.ReadAddr(areaTableAddr + (room << 2));
            int chestTableAddr = ROM.Instance.reader.ReadAddr(roomTableAddr + 0x0C);

            // Chests are 8 bytes long, and the item is stored 2 bytes in
            return chestTableAddr + chest * 8 + 0x02;
        }

        public enum LocationType
        {
            Untyped,
            Major,
            Minor,
            DungeonItem,
            Split,
            Helper,
            Half,
            Unshuffled
        }

        public List<Dependency> Dependencies;
        public LocationType Type;
        public string Name;
        public string Dungeon;
        public bool Filled;
        public Item Contents { get; private set; }
        private bool? AvailableCache;
        private Item DefaultContents;
        private int Address;

        public Location(LocationType type, string name, string dungeon, int address, List<Dependency> dependencies)
        {
            Type = type;
            Name = name;
            Dungeon = dungeon;

            Address = address;

            Dependencies = dependencies;

            if (address != 0)
            {
                DefaultContents = GetItemContents();
                Contents = DefaultContents;
            }

            Filled = false;
            AvailableCache = null;
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
                case LocationType.Split:
                    w.WriteByte((byte)Contents.Type, Address);
                    w.WriteByte(Contents.SubValue, Address + 2);
                    break;
                case LocationType.Half:
                    w.SetPosition(Address);
                    if (Contents.Type == ItemType.KinstoneX || Contents.Type == ItemType.ShellsX)
                    {
                        w.WriteByte((byte)ItemType.Shells30);
                    }
                    else
                    {
                        w.WriteByte((byte)Contents.Type);
                    }
                    break;
                case LocationType.Major:
                case LocationType.Minor:
                default:
                    w.SetPosition(Address);
                    w.WriteByte((byte)Contents.Type);
                    w.WriteByte(Contents.SubValue);
                    break;
            }
        }

        /// <summary>
        /// Read the item from the ROM
        /// </summary>
        /// <returns>The item contained at the address</returns>
        public Item GetItemContents()
        {
            ItemType type;
            byte subType;

            switch (Type)
            {
                case LocationType.Split:
                    type = (ItemType)ROM.Instance.reader.ReadByte(Address);
                    subType = ROM.Instance.reader.ReadByte(Address + 2);
                    break;
                case LocationType.Half:
                    type = (ItemType)ROM.Instance.reader.ReadByte(Address);
                    subType = 0x00;
                    break;
                default:
                    type = (ItemType)ROM.Instance.reader.ReadByte(Address);
                    subType = ROM.Instance.reader.ReadByte();
                    break;
            }

            
            if (Type == LocationType.DungeonItem)
            {
                return new Item(type, subType, Dungeon);
            }
            else
            {
                return new Item(type, subType);
            }
        }

        /// <summary>
        /// Check if an item can be placed into the location
        /// </summary>
        /// <param name="itemToPlace">The item to check placability of</param>
        /// <param name="availableItems">The items used for checking accessibility</param>
        /// <param name="locations">The locations used for checkign accessiblility</param>
        /// <returns>If the item can be placed in this location</returns>
        public bool CanPlace(Item itemToPlace, List<Item> availableItems, List<Location> locations)
        {
            switch (Type)
            {
                case LocationType.Helper:
                case LocationType.Untyped:
                    return false;
                case LocationType.Half:
                    if (itemToPlace.SubValue != 0)
                    {
                        return false;
                    }
                    break;
            }

            if (itemToPlace.Dungeon != "")
            {
                if (itemToPlace.Dungeon != Dungeon)
                {
                    return false;
                }
            }

            if (Address == 0)
            {
                return false;
            }

            return IsAccessible(availableItems, locations);
        }

        public bool IsAccessible(List<Item> availableItems, List<Location> locations, bool cache = false)
        {
            if (AvailableCache != null && cache == true)
            {
                return (bool)AvailableCache;
            }

            foreach (Dependency dependency in Dependencies)
            {
                if (!dependency.DependencyFulfilled(availableItems, locations))
                {
                    if (cache)
                    {
                        AvailableCache = false;
                        Console.WriteLine($"Can't reach {Name}");
                    }
                    
                    return false;
                }
            }

            if (cache)
            {
                Console.WriteLine($"Can reach {Name}");
                AvailableCache = true;
            }
            
            return true;
        }

        public void InvalidateCache()
        {
            AvailableCache = null;
        }

        public void Fill(Item contents)
        {
            Contents = contents;
            Filled = true;
        }

        public void SetItem(Item contents)
        {
            Contents = contents;
            DefaultContents = contents;
        }

        public void SetDefaultContents()
        {
            Contents = DefaultContents;
        }
    }
}
