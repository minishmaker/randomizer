using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MinishRandomizer.Core;
using MinishRandomizer.Randomizer.Logic;
using MinishRandomizer.Utilities;

namespace MinishRandomizer.Randomizer
{
    public class ShuffleException : Exception
    {
        public ShuffleException(string message) : base(message) { }
    }

    public readonly struct Item
    {
        public readonly ItemType Type;
        public readonly KinstoneType Kinstone;
        public readonly byte SubValue;
        public readonly string Dungeon;

        public Item(ItemType type, byte subValue, string dungeon = "")
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

            Dungeon = dungeon;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Item asItem = (Item)obj;
            return asItem.Type == Type && asItem.SubValue == SubValue;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Shuffler
    {
        // Will replace this with something better...
        public readonly string Version = "DEV 1.0.0";
        public int Seed;
        public RandomizedRom OutputRom;
        private Random RNG;
        private List<Location> Locations;
        //private List<Location> StartingLocations;
        private List<Item> DungeonItems;
        private List<Item> MajorItems;
        private List<Item> NiceItems;
        private List<Item> MinorItems;
        private Parser LogicParser;
        private string LogicPath;

        public Shuffler()
        {
            Locations = new List<Location>();
            DungeonItems = new List<Item>();
            MajorItems = new List<Item>();
            NiceItems = new List<Item>();
            MinorItems = new List<Item>();
            LogicParser = new Parser();
        }

        public void SetSeed(int seed)
        {
            Seed = seed;
            RNG = new Random(seed);
        }

        public List<LogicOption> GetOptions()
        {
            return LogicParser.SubParser.Options;
        }

        /// <summary>
        /// Load the flags that a logic file uses to customize itself
        /// </summary>
        public void LoadOptions(string logicFile = null)
        {
            LogicParser.SubParser.ClearOptions();

            string[] logicStrings;

            if (logicFile == null)
            {
                // Load default logic if no alternative is specified
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("MinishRandomizer.Resources.default.logic"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string allLocations = reader.ReadToEnd();
                    // Each line is a different location, split regardless of return form
                    logicStrings = allLocations.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                }
            }
            else
            {
                logicStrings = File.ReadAllLines(logicFile);
            }

            LogicParser.PreParse(logicStrings);
        }

        public bool RomCrcValid(ROM rom)
        {
            if (LogicParser.SubParser.RomCrc != null)
            {
                return PatchUtil.Crc32(rom.romData, rom.romData.Length) == LogicParser.SubParser.RomCrc;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Reads the list of locations from a file, or the default logic if none is specified
        /// </summary>
        /// <param name="logicFile">The file to read locations from</param>
        public void LoadLocations(string logicFile = null)
        {
            // Change the logic file path to match
            LogicPath = logicFile;

            // Reset everything to allow rerandomization
            ClearLogic();

            string[] locationStrings;

            // Get the logic file as an array of strings that can be parsed
            if (logicFile == null)
            {
                // Load default logic if no alternative is specified
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("MinishRandomizer.Resources.default.logic"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string allLocations = reader.ReadToEnd();
                    // Each line is a different location, split regardless of return form
                    locationStrings = allLocations.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                }
            }
            else
            {
                locationStrings = File.ReadAllLines(logicFile);
            }

            List<Location> parsedLocations = LogicParser.ParseLocations(locationStrings, RNG);

            parsedLocations.ForEach(location => { AddLocation(location); });
        }

        public void AddLocation(Location location)
        {
            if (LogicParser.SubParser.Replacements.ContainsKey(location.Contents))
            {
                var chanceSet = LogicParser.SubParser.Replacements[location.Contents];
                var number = RNG.Next(chanceSet.totalChance);
                int val = 0;

                for (int i = 0; i < chanceSet.randomItems.Count(); i++)
                {
                    val += chanceSet.randomItems[i].chance;
                    if (number < val)
                    {
                        location.SetItem(chanceSet.randomItems[i].item);
                        break;
                    }
                }
            }

            if (LogicParser.SubParser.LocationTypeOverrides.ContainsKey(location.Contents))
            {
                location.Type = LogicParser.SubParser.LocationTypeOverrides[location.Contents];
            }

            // All locations are in the master location list
            Locations.Add(location);

            // The type of the containing location determines how the item is handled
            switch (location.Type)
            {
                // These locations are not filled, because they don't reference an item location
                case Location.LocationType.Untyped:
                case Location.LocationType.Helper:
                    break;
                // Unshuffled locations are filled by default
                case Location.LocationType.Unshuffled:
                    location.Fill(location.Contents);
                    break;
                // Minor locations are not logically accounted for
                case Location.LocationType.Minor:
                    MinorItems.Add(location.Contents);
                    break;
                // Dungeon items can only be placed within the same dungeon, and are placed first
                case Location.LocationType.DungeonItem:
                    DungeonItems.Add(location.Contents);
                    break;
                // Nice items check logic but cannot affect it
                case Location.LocationType.Nice:
                    NiceItems.Add(location.Contents);
                    break;
                // Major/etc items are fully randomized and check logic
                case Location.LocationType.Major:
                default:
                    MajorItems.Add(location.Contents);
                    break;
            }
        }

        public RandomizedRom GenerateRom(string settingsString, string gimmicksString)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shuffles all locations, ensuring the game is beatable within the logic and all Major/Nice items are reachable.
        /// </summary>
        public void RandomizeLocations()
        {
            List<Item> unplacedItems = MajorItems.ToList();
            List<Item> dungeonSpecificItems = DungeonItems.ToList();

            List<Location> unfilledLocations = Locations.Where(location => !location.Filled && location.Type != Location.LocationType.Helper && location.Type != Location.LocationType.Untyped).ToList();
            unfilledLocations.Shuffle(RNG);
            unplacedItems.Shuffle(RNG);

            // Fill dungeon items first so there is room for them all
            List<Location> dungeonLocations = FillLocations(dungeonSpecificItems, unfilledLocations, unplacedItems);

            // Fill non-dungeon major items, checking for logic
            unfilledLocations.Shuffle(RNG);
            FillLocations(unplacedItems, unfilledLocations);

            // Get every item that can be logically obtained, to check if the game can be completed
            List<Item> finalMajorItems = GetAvailableItems(new List<Item>());

            if (!new LocationDependency("BeatVaati").DependencyFulfilled(finalMajorItems, Locations))
            {
                throw new ShuffleException($"Randomization succeded, but could not beat Vaati!");
            }

            // Put nice items in locations, logic is checked but not updated
            unfilledLocations.Shuffle(RNG);
            CheckedFastFillLocations(NiceItems, unfilledLocations);

            // Put minor items in locations, not checking logic
            unfilledLocations.Shuffle(RNG);
            FastFillLocations(MinorItems.ToList(), unfilledLocations);

            if (unfilledLocations.Count != 0)
            {
                // All locations should be filled at this point
                throw new ShuffleException($"There are {unfilledLocations.Count} unfilled locations!");
            }
        }

        /// <summary>
        /// Uniformly fills items in locations, checking to make sure the items are logically available.
        /// </summary>
        /// <param name="items">The items to fill with</param>
        /// <param name="locations">The locations to be filled</param>
        /// <param name="assumedItems">The items that are available by default</param>
        /// <returns>A list of the locations that were filled</returns>
        private List<Location> FillLocations(List<Item> items, List<Location> locations, List<Item> assumedItems = null)
        {
            List<Location> filledLocations = new List<Location>();

            assumedItems ??= new List<Item>();

            for (int i = items.Count - 1; i >= 0; i--)
            {
                // Get a random item from the list and save its index
                int itemIndex = RNG.Next(items.Count);
                Item item = items[itemIndex];
                
                // Take item out of pool
                items.RemoveAt(itemIndex);

                List<Item> availableItems = GetAvailableItems(items.Concat(assumedItems).ToList());

                // Find locations that are available for placing the item
                List<Location> availableLocations = locations.Where(location => location.CanPlace(item, availableItems, Locations)).ToList();

                if (availableLocations.Count <= 0)
                {
                    // The filler broke, show all available items and get out
                    availableItems.ForEach(itm => Console.WriteLine($"{itm.Type} sub {itm.SubValue}"));
                    throw new ShuffleException($"Could not place {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");
                }

                int locationIndex = RNG.Next(availableLocations.Count);

                availableLocations[locationIndex].Fill(item);
                Console.WriteLine($"Placed {item.Type.ToString()} subtype {StringUtil.AsStringHex2(item.SubValue)} at {availableLocations[locationIndex].Name} with {items.Count} items remaining\n");

                locations.Remove(availableLocations[locationIndex]);

                filledLocations.Add(availableLocations[locationIndex]);

                // Location caches are no longer valid because available items have changed
                Locations.ForEach(location => location.InvalidateCache());
            }

            return filledLocations;
        }

        /// <summary>
        /// Fill items in locations that are available at the start of the fill.
        /// Slower than FastFillLocations, but will not place in unavailable locations.
        /// </summary>
        /// <param name="items">The items to fill with</param>
        /// <param name="locations">The locations in which to fill the items</param>
        private void CheckedFastFillLocations(List<Item> items, List<Location> locations)
        {
            List<Item> finalMajorItems = GetAvailableItems(new List<Item>());
            List<Location> availableLocations = locations.Where(location => location.IsAccessible(finalMajorItems, Locations, false)).ToList();

            foreach (Item item in items)
            {
                int locationIndex = RNG.Next(0, availableLocations.Count);
                availableLocations[locationIndex].Fill(item);
            }
        }

        /// <summary>
        /// Fill items in locations without checking logic for speed
        /// </summary>
        /// <param name="items">The items to fill with</param>
        /// <param name="locations">The locations in which to fill the items</param>
        private void FastFillLocations(List<Item> items, List<Location> locations)
        {
            // Don't need to check logic, cause the items being placed do not affect logic
            foreach (Item item in items)
            {
                locations[0].Fill(item);
                locations.RemoveAt(0);
            }
        }


        /// <summary>
        /// Gets all the available items with a given item set, looping until there are no more items left to get
        /// </summary>
        /// <param name="preAvailableItems">Items that are available from the start</param>
        /// <returns>A list of all the items that are logically accessible</returns>
        private List<Item> GetAvailableItems(List<Item> preAvailableItems)
        {
            List<Item> availableItems = preAvailableItems.ToList();

            List<Location> filledLocations = Locations.Where(location => location.Filled && location.Type != Location.LocationType.Helper && location.Type != Location.LocationType.Untyped).ToList();

            int previousSize;

            // Get "spheres" until the next sphere contains no new items
            do
            {
                // Doesn't touch the cache to prevent incorrect caching
                List<Location> accessibleLocations = filledLocations.Where(location => location.IsAccessible(availableItems, Locations, false)).ToList();
                previousSize = accessibleLocations.Count;

                filledLocations.RemoveAll(location => accessibleLocations.Contains(location));

                List<Item> newItems = Location.GetItems(accessibleLocations);

                availableItems.AddRange(newItems);
            }
            while (previousSize > 0);

            return availableItems;
        }

        public void ClearLogic()
        {
            Locations.Clear();
            DungeonItems.Clear();
            MajorItems.Clear();
            NiceItems.Clear();
            MinorItems.Clear();

            LogicParser.SubParser.ClearTypeOverrides();
            LogicParser.SubParser.ClearReplacements();
            LogicParser.SubParser.ClearDefines();
            LogicParser.SubParser.AddOptions();
        }
    }
}
