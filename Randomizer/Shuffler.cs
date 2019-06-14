using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private List<Item> MajorItems;
        private List<Item> MinorItems;
        private string OutputDirectory;

        public Shuffler(string outputDirectory)
        {
            RNG = new Random();
            Locations = new List<Location>();
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
            
            foreach (string locationString in locationStrings)
            {
                if (locationString == "" || locationString[0] == '#')
                {
                    continue;
                }

                Location newLocation = Location.GetLocation(locationString);
                Locations.Add(newLocation);
                switch(newLocation.Type)
                {
                    case Location.LocationType.Untyped:
                    case Location.LocationType.Helper:
                        break;
                    case Location.LocationType.Minor:
                        MinorItems.Add(newLocation.Contents);
                        break;
                    case Location.LocationType.Normal:
                    default:
                        Console.WriteLine($"Hey! {newLocation.Contents.Type.ToString()}");
                        MajorItems.Add(newLocation.Contents);
                        break;
                }
            }
        }

        public void RandomizeLocations()
        {
            List<Item> unplacedItems = MajorItems.ToList();
            List<Location> unfilledLocations = Locations.Where(location => !location.Filled && location.Type != Location.LocationType.Helper).ToList();
            unfilledLocations.Shuffle(RNG);
            unplacedItems.Shuffle(RNG);

            FillLocations(MajorItems.ToList(), unfilledLocations);

            unfilledLocations.Shuffle(RNG);
            FastFillLocations(MinorItems.ToList(), unfilledLocations);

            if (unfilledLocations.Count != 0)
            {
                Console.WriteLine("Not all locations filled!");
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

        private void FillLocations(List<Item> items, List<Location> locations)
        {
            int itemIndex;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                itemIndex = RNG.Next(items.Count);
                Item item = items[itemIndex];
                Console.WriteLine($"Placing: {item.Type.ToString()}");
                items.RemoveAt(itemIndex);
                List<Location> availableLocations = locations.Where(location => location.CanPlace(item, items, locations)).ToList();

                if (availableLocations.Count <= 0)
                {
                    Console.WriteLine($"Could not place {item.Type.ToString()}!");
                    return;
                }

                availableLocations[0].Fill(item);
                Console.WriteLine($"Placed {item.Type.ToString()} at {availableLocations[0].Name} with {items.Count} items remaining\n");
                locations.Remove(availableLocations[0]);
            }
        }

        private void FastFillLocations(List<Item> items, List<Location> locations)
        {
            foreach (Item item in items)
            {
                locations[0].Fill(item);
                locations.RemoveAt(0);
            }
        }
    }
}
