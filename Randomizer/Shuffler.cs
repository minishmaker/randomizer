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
        private List<Location> Locations;
        //private List<Location> StartingLocations;
        private List<Item> DungeonItems;
        private List<Item> MajorItems;
        private List<Item> MinorItems;
        private string OutputDirectory;

        public Shuffler(string outputDirectory)
        {
            RNG = new Random();
            Locations = new List<Location>();
            DungeonItems = new List<Item>();
            MajorItems = new List<Item>();
            MinorItems = new List<Item>();
            OutputDirectory = outputDirectory;
        }

        public void SetSeed(int seed)
        {
            RNG = new Random(seed);
        }

        public void LoadLocations(string locationFile)
        {
            Locations.Clear();
            DungeonItems.Clear();
            MajorItems.Clear();
            MinorItems.Clear();

            string[] locationStrings;

            if (locationFile == null)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("MinishRandomizer.Resources.default.logic"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string allLocations = reader.ReadToEnd();
                    locationStrings = allLocations.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                }
            }
            else
            {
                locationStrings = File.ReadAllLines(locationFile);
            }
            
            foreach (string locationLine in locationStrings)
            {
                string locationString = locationLine.Split('#')[0].Replace(" ", "");
                locationString = locationString.Replace(" ", "");
                if (locationString == "")
                {
                    continue;
                }

                Location newLocation = Location.GetLocation(locationString);
                Locations.Add(newLocation);

                switch(newLocation.Type)
                {
                    case Location.LocationType.Untyped:
                    case Location.LocationType.Helper:
                        Console.WriteLine($"Helper or untyped {newLocation.Name}");
                        break;
                    case Location.LocationType.Minor:
                        Console.WriteLine(newLocation.Contents.Type.ToString());
                        MinorItems.Add(newLocation.Contents);
                        break;
                    case Location.LocationType.DungeonItem:
                        Console.WriteLine($"{newLocation.Dungeon}: {newLocation.Contents.Type.ToString()}");
                        DungeonItems.Add(newLocation.Contents);
                        break;
                    case Location.LocationType.Major:
                    case Location.LocationType.Split:
                    case Location.LocationType.PurchaseItem:
                    case Location.LocationType.ScrollItem:
                    default:
                        Console.WriteLine($"Hey! {newLocation.Contents.Type.ToString()}");
                        MajorItems.Add(newLocation.Contents);
                        break;
                }
            }
        }

        public void PatchRom(string locationFile)
        {
            byte[] patchContents;
            if (locationFile == null)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("MinishRandomizer.Resources.randoPatch.ups"))
                {
                    patchContents = new byte[stream.Length];
                    stream.Read(patchContents, 0, (int)stream.Length);
                }
            }
            else
            {
                patchContents = File.ReadAllBytes(locationFile);
            }

            PatchUtil.ApplyUPS(ROM.Instance.romData, patchContents);
        }

        public void RandomizeLocations()
        {
            ResetLocations();

            List<Item> unplacedItems = MajorItems.ToList();
            List<Item> dungeonSpecificItems = DungeonItems.ToList();

            List<Location> unfilledLocations = Locations.Where(location => !location.Filled && location.Type != Location.LocationType.Helper && location.Type != Location.LocationType.Untyped).ToList();
            unfilledLocations.Shuffle(RNG);
            unplacedItems.Shuffle(RNG);

            // Fill dungeon items first so there is room for them all
            List<Location> dungeonLocations = FillLocations(dungeonSpecificItems, unfilledLocations, unplacedItems);

            // Fill non-dungeon items, taking into account the previously placed dungeon items
            // This seems fairly crude; other randomizers don't seem to deal with this problem?
            unfilledLocations.Shuffle(RNG);
            FillLocations(unplacedItems, unfilledLocations);

            unfilledLocations.Shuffle(RNG);
            FastFillLocations(MinorItems.ToList(), unfilledLocations);

            if (unfilledLocations.Count != 0)
            {
                throw new ShuffleException($"There are {unfilledLocations.Count} unfilled locations!");
            }

            using (MemoryStream ms = new MemoryStream(ROM.Instance.romData))
            {
                Writer writer = new Writer(ms);
                foreach (Location location in Locations)
                {
                    location.WriteLocation(writer);
                }
            }

            File.WriteAllBytes(OutputDirectory + "/mcrando.gba", ROM.Instance.romData);
        }

        private void ResetLocations()
        {
            foreach (Location location in Locations)
            {
                location.SetDefaultContents();
                location.InvalidateCache();
                location.Filled = false;
            }
        }

        // Based off of the RandomAssumed ALttPR filler
        private List<Location> FillLocations(List<Item> items, List<Location> locations, List<Item> assumedItems = null)
        {
            List<Location> filledLocations = new List<Location>();

            assumedItems = assumedItems ?? new List<Item>();

            for (int i = items.Count - 1; i >= 0; i--)
            {
                int itemIndex = RNG.Next(items.Count);
                Item item = items[itemIndex];
                Console.WriteLine($"Placing: {item.Type.ToString()}");
                if (item.Dungeon != "")
                {
                    Console.WriteLine($"Dungeon: {item.Dungeon}");
                }
                
                items.RemoveAt(itemIndex);

                List<Item> availableItems = GetAvailableItems(items.Concat(assumedItems).ToList());

                List<Location> availableLocations = locations.Where(location => location.CanPlace(item, availableItems, Locations)).ToList();

                if (availableLocations.Count <= 0)
                {
                    availableItems.ForEach(itm => Console.WriteLine($"{itm.Type} sub {itm.SubValue}"));
                    throw new ShuffleException($"Could not place {item.Type}");
                }

                int locationIndex = RNG.Next(availableLocations.Count);

                availableLocations[locationIndex].Fill(item);
                Console.WriteLine($"Placed {item.Type.ToString()} at {availableLocations[locationIndex].Name} with {items.Count} items remaining\n");

                locations.Remove(availableLocations[locationIndex]);

                filledLocations.Add(availableLocations[locationIndex]);

                // Location caches are no longer valid because available items have changed
                Locations.ForEach(location => location.InvalidateCache());
            }

            return filledLocations;
        }

        private void FastFillLocations(List<Item> items, List<Location> locations)
        {
            foreach (Item item in items)
            {
                locations[0].Fill(item);
                locations.RemoveAt(0);
            }
        }

        // Gets all the available items with a given item set, looping until there are no more items left to get
        private List<Item> GetAvailableItems(List<Item> preAvailableItems)
        {
            List<Item> availableItems = preAvailableItems.ToList();

            List<Location> filledLocations = Locations.Where(location => location.Filled && location.Type != Location.LocationType.Helper && location.Type != Location.LocationType.Untyped).ToList();

            int previousSize;
            do
            {
                // Doesn't touch the cache to prevent incorrect caching
                List<Location> accessibleLocations = filledLocations.Where(location => location.IsAccessible(availableItems, Locations, false)).ToList();
                previousSize = accessibleLocations.Count;

                filledLocations.RemoveAll(location => accessibleLocations.Contains(location));

                availableItems.AddRange(Location.GetItems(accessibleLocations));
            }
            while (previousSize > 0);

            return availableItems;
        }
    }
}
