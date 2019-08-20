using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinishRandomizer.Core;
using MinishRandomizer.Utilities;

namespace MinishRandomizer.Randomizer.Logic
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

            string dungeon = "";
            // Colon, must have a dungeon specified
            if (names.Length >= 2)
            {
                dungeon = names[1];
            }

            string locationType = locationParts[1];
            if (!Enum.TryParse(locationType, out LocationType type) || type == LocationType.Untyped)
            {
                Console.WriteLine("Invalid type");
                throw new ShuffleException($"Location \"{name}\" has invalid type \"{locationType}\"!");
            }

            // Get all comma-separated addresses and properly parse them
            string[] addressStrings = locationParts[2].Split(',');
            List<LocationAddress> addresses = new List<LocationAddress>(addressStrings.Length);
            List<EventLocationAddress> defines = new List<EventLocationAddress>();
            foreach (string address in addressStrings)
            {
                LocationAddress parsedAddress = GetAddressFromString(address);
                if (parsedAddress is EventLocationAddress)
                {
                    defines.Add((EventLocationAddress)parsedAddress);
                }
                else
                {
                    addresses.Add(parsedAddress);
                }

            }

            string logic = "";
            // Looks like logic is specified
            if (locationParts.Length >= 4)
            {
                logic = locationParts[3];
            }

            List<Dependency> dependencies = Dependency.GetDependencies(logic);

            Item? itemOverride = null;
            // Has enough parts for an extra item
            if (locationParts.Length >= 5)
            {
                string[] itemParts = locationParts[4].Split(':');
                string[] subParts = itemParts[0].Split('.');

                if (subParts[0] == "Items")
                {
                    // TODO: Break this into another function somewhere later, really not sure where. Maybe a LogicUtil?
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


                        itemOverride = new Item(replacementType, subType, itemDungeon);
                    }
                }
            }

            Location location = new Location(type, name, dungeon, addresses, defines, dependencies, itemOverride);

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
        public static LocationAddress GetAddressFromString(string addressString)
        {
            // Either direct address or area-room-chest
            if (addressString == "")
            {
                return new LocationAddress(AddressType.None, 0);
            }

            // Get the types of the address
            string[] addressParts = addressString.Split(':');

            AddressType addressType = AddressType.None;
            if (addressParts.Length > 1)
            {
                for (int i = 1; i < addressParts.Length; i++)
                {
                    if (Enum.TryParse(addressParts[i], out AddressType subType))
                    {
                        // Set the flag(s) indicated by the given type
                        addressType |= subType;
                    }
                    else
                    {
                        throw new ShuffleException($"{addressParts[i]} in address {addressString} is not a valid address type!");
                    }
                }
            }

            // If a byte isn't set, default to both
            if ((addressType & AddressType.FirstByte | addressType & AddressType.SecondByte) == AddressType.None)
            {
                // Set the type of the address to the default
                addressType |= AddressType.BothBytes;
            }

            // If the address is an event define, make it an EventLocationAddress
            if ((addressType & AddressType.Define) == AddressType.Define)
            {
                // The first part refers to the name of the define
                return new EventLocationAddress(addressType, addressParts[0]);
            }

            // The address is a hexadecimal number, so it can simply be parsed
            if (int.TryParse(addressParts[0], NumberStyles.HexNumber, null, out int address))
            {
                return new LocationAddress(addressType, address);
            }

            // The address is an entity, so it should be parsed as an area-room-entity number
            string[] entityDetails = addressParts[0].Split('-');
            if (entityDetails.Length != 3)
            {
                throw new ShuffleException($"Entity data \"{addressString}\" does not have a full address!");
            }

            if (!int.TryParse(entityDetails[0], NumberStyles.HexNumber, null, out int area))
            {
                throw new ShuffleException($"Entity data \"{addressString}\" has an invalid area index!");
            }

            if (!int.TryParse(entityDetails[1], NumberStyles.HexNumber, null, out int room))
            {
                throw new ShuffleException($"Entity data \"{addressString}\" has an invalid room index!");
            }

            if (!int.TryParse(entityDetails[2], NumberStyles.HexNumber, null, out int chest))
            {
                throw new ShuffleException($"Entity data \"{addressString}\" has an invalid entity index!");
            }

            int addressValue = 0;

            if ((addressType & AddressType.GroundItem) == AddressType.GroundItem)
            {
                throw new NotImplementedException();
            }
            else
            {
                // Look chest address up in table
                int areaTableAddr = ROM.Instance.reader.ReadAddr(ROM.Instance.headers.AreaMetadataBase + (area << 2));
                int roomTableAddr = ROM.Instance.reader.ReadAddr(areaTableAddr + (room << 2));
                int chestTableAddr = ROM.Instance.reader.ReadAddr(roomTableAddr + 0x0C);

                // Chests are 8 bytes long, and the item is stored 2 bytes in
                addressValue = chestTableAddr + chest * 8 + 0x02;
            }


            return new LocationAddress(addressType, addressValue);
        }



        public enum LocationType
        {
            Untyped,
            Major,
            Minor,
            DungeonItem,
            Helper,
            Unshuffled
        }

        public List<Dependency> Dependencies;
        public LocationType Type;
        public string Name;
        public string Dungeon;
        public bool Addressed;
        public bool SecondaryAddressed;
        public bool Filled;
        public Item Contents { get; private set; }
        private bool? AvailableCache;
        private Item DefaultContents;
        private List<LocationAddress> Addresses;
        private List<EventLocationAddress> Defines;

        public Location(LocationType type, string name, string dungeon, List<LocationAddress> addresses, List<EventLocationAddress> defines, List<Dependency> dependencies, Item? replacementContents = null)
        {
            Type = type;
            Name = name;
            Dungeon = dungeon;

            // One location can have several addresses
            Addresses = addresses;

            // Because the two have zero overlap, the EventLocationAddresses are separate.
            Defines = defines;

            // Whether it's addressed or not determines what can be placed
            Addressed = IsAddressed();
            SecondaryAddressed = IsSecondaryAddressed();

            Dependencies = dependencies;

            if (replacementContents == null)
            {
                // Need to get the item from the ROM
                DefaultContents = GetItemContents();
                Contents = DefaultContents;
            }
            else
            {
                // The item is specified by the logic rather than the ROM
                DefaultContents = (Item)replacementContents;
                Contents = DefaultContents;
            }

            Filled = false;
            AvailableCache = null;
        }

        public bool IsAddressed()
        {
            foreach (LocationAddress address in Addresses)
            {
                // If any of the location's addresses validly index the first byte, it can be written to
                if ((address.Type & AddressType.FirstByte) == AddressType.FirstByte && address.Address != 0)
                {
                    return true;
                }
            }

            foreach (EventLocationAddress define in Defines)
            {
                // If any of the defined addresses isn't 
                if ((define.Type & AddressType.FirstByte) == AddressType.FirstByte && !string.IsNullOrEmpty(define.Define.Name))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSecondaryAddressed()
        {
            foreach (LocationAddress address in Addresses)
            {
                // If any of the location's addresses validly index the first byte, it can be written to
                if ((address.Type & AddressType.SecondByte) == AddressType.SecondByte && address.Address != 0)
                {
                    return true;
                }
            }

            foreach (EventLocationAddress define in Defines)
            {
                // If any of the defined addresses is first byte, it's valid
                if ((define.Type & AddressType.SecondByte) == AddressType.SecondByte && !string.IsNullOrEmpty(define.Define.Name))
                {
                    return true;
                }
            }

            Console.WriteLine($"Can't place subvalued items in {Name}");
            return false;
        }

        public void WriteLocation(Writer w)
        {
            // Write each address to ROM
            foreach (LocationAddress address in Addresses)
            {
                address.WriteAddress(w, Contents);
            }

        }

        public void WriteLocationEvent(StringBuilder w)
        {
            foreach (EventLocationAddress define in Defines)
            {
                define.WriteDefine(w, Contents);
            }
        }

        /// <summary>
        /// Read the item from the ROM
        /// </summary>
        /// <returns>The item contained at the address</returns>
        public Item GetItemContents()
        {
            ItemType type = ItemType.Untyped;
            byte subType = 0;

            // Read all the addresses, taking the last of each byte type
            foreach (LocationAddress address in Addresses)
            {
                address.ReadAddress(ROM.Instance.reader, ref type, ref subType);
            }

            // If the contents of the address aren't defined/are untyped, it's probably broken
            if (type == ItemType.Untyped && Type != LocationType.Helper && Type != LocationType.Unshuffled)
            {
                Console.WriteLine($"Untyped contents in {Name}! Addresses may be bad");
            }

            // Dungeon items get the Dungeon part defined
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
            }

            // Unaddressed locations can't contain anything
            if (!Addressed)
            {
                return false;
            }

            // Without a secondary address, items with subvalues cannot be placed
            if (!SecondaryAddressed && itemToPlace.SubValue != 0)
            {
                return false;
            }

            // Can only place in correct dungeon
            if (itemToPlace.Dungeon != "")
            {
                if (itemToPlace.Dungeon != Dungeon)
                {
                    return false;
                }
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
