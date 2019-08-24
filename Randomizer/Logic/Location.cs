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
        public static List<Item> GetItems(List<Location> locations)
        {
            List<Item> items = new List<Item>();

            foreach (Location location in locations)
            {
                items.Add(location.Contents);
            }

            return items;
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
