using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MinishRandomizer.Core;
using MinishRandomizer.Utilities;

namespace MinishRandomizer.Randomizer
{
    public enum HeartColorType
    {
        Default,
        Red,
        Yellow,
        Blue,
        Sky,
        Green,
        Lime
    }

    public class ShuffleException : Exception {
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
        private Random RNG;
        public int Seed;
        private List<Location> Locations;
        //private List<Location> StartingLocations;
        private List<Item> DungeonItems;
        private List<Item> MajorItems;
        private List<Item> MinorItems;
        private string OutputDirectory;
        private HeartColorType HeartColor = HeartColorType.Default;

        public Shuffler(string outputDirectory)
        {
            Locations = new List<Location>();
            DungeonItems = new List<Item>();
            MajorItems = new List<Item>();
            MinorItems = new List<Item>();
            OutputDirectory = outputDirectory;
        }

        public void SetSeed(int seed)
        {
            Seed = seed;
            RNG = new Random(seed);
        }

        public void SetHeartColor(HeartColorType color)
        {
            HeartColor = color;
        }

        /// <summary>
        /// Reads the list of locations from a file, or the default logic if none is specified
        /// </summary>
        /// <param name="locationFile">The file to read locations from</param>
        public void LoadLocations(string locationFile = null)
        {
            Locations.Clear();
            DungeonItems.Clear();
            MajorItems.Clear();
            MinorItems.Clear();

            string[] locationStrings;

            if (locationFile == null)
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
                locationStrings = File.ReadAllLines(locationFile);
            }
            
            foreach (string locationLine in locationStrings)
            {
                // Spaces are ignored, and everything after a # is a comment
                string locationString = locationLine.Split('#')[0].Replace(" ", "");

                // Empty lines or locations are ignored
                if (string.IsNullOrWhiteSpace(locationString))
                {
                    continue;
                }

                Location newLocation = Location.GetLocation(locationString);
                AddLocation(newLocation);
            }
        }

        private void AddLocation(Location location)
        {
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
                // Major/etc items are fully randomized
                case Location.LocationType.Major:
                default:
                    MajorItems.Add(location.Contents);
                    break;
            }
        }

        public void ApplyPatch(string romLocation, string patchFile = null)
        {
            if (string.IsNullOrEmpty(patchFile))
            {
                // Get directory of MinishRandomizer 
                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                patchFile = assemblyPath + "/Patches/ROM buildfile.event";
            }

            // Write new patch file to patch folder/extDefinitions.event
            File.WriteAllText(Path.GetDirectoryName(patchFile) + "/extDefinitions.event", GetEventWrites());

            string[] args = new[] { "A", "FE8", "-input:" + patchFile, "-output:" + romLocation };

            ColorzCore.Program.Main(args);
        }

        /// <summary>
        /// Loads and shuffles all locations
        /// </summary>
        /// <param name="seed">The RNG seed used for generation</param>
        public void RandomizeLocations(int seed)
        {
            // Make sure the RNG is set to the seed, so the seed can be regenerated
            SetSeed(seed);

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

            // Put the remaining items into the place wherever
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

            assumedItems = assumedItems ?? new List<Item>();

            for (int i = items.Count - 1; i >= 0; i--)
            {
                // Get a random item from the list and save its index
                int itemIndex = RNG.Next(items.Count);
                Item item = items[itemIndex];

                // Write placing information to spoiler log
                Console.WriteLine($"Placing: {item.Type.ToString()}");
                if (item.Type == ItemType.KinstoneX)
                {
                    Console.WriteLine($"Type: {item.Kinstone.ToString()}");
                }

                if (item.Dungeon != "")
                {
                    Console.WriteLine($"Dungeon: {item.Dungeon}");
                }
                
                // Take item out of pool
                items.RemoveAt(itemIndex);

                List<Item> availableItems = GetAvailableItems(items.Concat(assumedItems).ToList());

                // Find locations that are available for placing the item
                List<Location> availableLocations = locations.Where(location => location.CanPlace(item, availableItems, Locations)).ToList();

                if (availableLocations.Count <= 0)
                {
                    // The filler broke, show all available items and get out
                    availableItems.ForEach(itm => Console.WriteLine($"{itm.Type} sub {itm.SubValue}"));
                    throw new ShuffleException($"Could not place {item.Type}!");
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
        /// Fill items in locations without checking logic for speed
        /// </summary>
        /// <param name="items">The items to be filled</param>
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

        /// <summary>
        /// Get a byte[] of the randomized data
        /// </summary>
        /// <returns>The data of the randomized ROM</returns>
        public byte[] GetRandomizedRom()
        {
            // Create a copy of the ROM data to modify for output
            byte[] outputBytes = new byte[ROM.Instance.romData.Length];
            Array.Copy(ROM.Instance.romData, 0, outputBytes, 0, outputBytes.Length);

            using (MemoryStream ms = new MemoryStream(outputBytes))
            {
                Writer writer = new Writer(ms);
                foreach (Location location in Locations)
                {
                    location.WriteLocation(writer);
                }
            }

            return outputBytes;
        }

        /// <summary>
        /// Get the contents of the spoiler log, including playthrough
        /// </summary>
        /// <returns>The contents of the spoiler log</returns>
        public string GetSpoiler()
        {
            StringBuilder spoilerBuilder = new StringBuilder();
            spoilerBuilder.AppendLine("Spoiler for Minish Cap Randomizer");
            spoilerBuilder.AppendLine($"Seed: {Seed}");

            spoilerBuilder.AppendLine();
            AppendLocationSpoiler(spoilerBuilder);

            spoilerBuilder.AppendLine();
            AppendPlaythroughSpoiler(spoilerBuilder);


            return spoilerBuilder.ToString();
        }

        /// <summary>
        /// Create list of filled locations and their contents
        /// </summary>
        /// <param name="spoilerBuilder">The running spoiler log builder to append the locations to<</param>
        private void AppendLocationSpoiler(StringBuilder spoilerBuilder)
        {
            spoilerBuilder.AppendLine("Location Contents:");
            // Get the locations that have been filled
            List<Location> filledLocations = Locations.Where(location => location.Filled && location.Type != Location.LocationType.Helper && location.Type != Location.LocationType.Untyped).ToList();

            foreach (Location location in filledLocations)
            {
                spoilerBuilder.AppendLine($"{location.Name}: {location.Contents.Type}");

                AppendSubvalue(spoilerBuilder, location);

                spoilerBuilder.AppendLine();
            }
        }

        /// <summary>
        /// Create list of items in the order they can logically be collected
        /// </summary>
        /// <param name="spoilerBuilder">The running spoiler log builder to append the playthrough to</param>
        private void AppendPlaythroughSpoiler(StringBuilder spoilerBuilder)
        {
            spoilerBuilder.AppendLine("Playthrough:");

            List<Location> filledLocations = Locations.Where(location => location.Filled && location.Type != Location.LocationType.Helper && location.Type != Location.LocationType.Untyped).ToList();
            List<Item> availableItems = new List<Item>();

            int previousSize;
            int sphereCounter = 1;

            do
            {
                List<Location> accessibleLocations = filledLocations.Where(location => location.IsAccessible(availableItems, Locations, false)).ToList();
                previousSize = accessibleLocations.Count;

                filledLocations.RemoveAll(location => accessibleLocations.Contains(location));

                List<Item> newItems = Location.GetItems(accessibleLocations);
                availableItems.AddRange(newItems);

                foreach (Location location in accessibleLocations)
                {
                    spoilerBuilder.AppendLine($"Sphere {sphereCounter}: {location.Contents.Type} in {location.Name}");

                    AppendSubvalue(spoilerBuilder, location);
                    spoilerBuilder.AppendLine();
                }

                sphereCounter++;
                spoilerBuilder.AppendLine();
            }
            while (previousSize > 0);
        }

        private void AppendSubvalue(StringBuilder spoilerBuilder, Location location)
        {
            // Display subvalue if relevant
            if (location.Contents.Type == ItemType.KinstoneX)
            {
                spoilerBuilder.AppendLine($"Kinstone Type: {location.Contents.Kinstone}");
            }
            else if (location.Contents.SubValue != 0)
            {
                spoilerBuilder.AppendLine($"Subvalue: {location.Contents.SubValue}");
            }

            // Display dungeon contents if relevant
            if (!string.IsNullOrEmpty(location.Contents.Dungeon))
            {
                spoilerBuilder.AppendLine($"Dungeon: {location.Contents.Dungeon}");
            }
        }

        public string GetEventWrites()
        {
            StringBuilder eventBuilder = new StringBuilder();

            foreach (Location location in Locations)
            {
                location.WriteLocationEvent(eventBuilder);
            }

            if (HeartColor != HeartColorType.Default)
            {
                // Write the heart color defines to change the heart color properly, with the lowercase version of the enum
                eventBuilder.AppendLine("#define heartscolor");
                eventBuilder.AppendLine("#define heartscolor" + HeartColor.ToString().ToLower());
            }

            return eventBuilder.ToString();
        }
    }
}
