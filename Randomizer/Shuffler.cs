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
        private List<Item> Items;
        private string OutputDirectory;

        public Shuffler(string outputDirectory)
        {
            RNG = new Random();
            Locations = new List<Location>();
            Items = new List<Item>();
            OutputDirectory = outputDirectory;
        }

        public void SetSeed(int seed)
        {
            RNG = new Random(seed);
        }

        public void LoadLocations(string locationFile)
        {
            Locations.Clear();
            Items.Clear();

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
                if (newLocation.Type != Location.LocationType.Helper)
                {
                    Items.Add(newLocation.Contents);
                }
            }
        }

        public void RandomizeLocations()
        {
            List<Item> unplacedItems = Items.ToList();
            List<Location> unfilledLocations = Locations.Where(location => !location.Filled && location.Type != Location.LocationType.Helper).ToList();
            unfilledLocations.Shuffle(RNG);
            unplacedItems.Shuffle(RNG);

            int itemIndex;
            while (unplacedItems.Count > 0)
            {
                // TODO: Make this fill not bad
                itemIndex = RNG.Next(unplacedItems.Count);
                Item item = unplacedItems[itemIndex];
                Console.WriteLine($"Placing: {item.Type.ToString()}");
                unplacedItems.RemoveAt(itemIndex);
                List<Location> availableLocations = unfilledLocations.Where(location => location.CanPlace(item, unplacedItems, unfilledLocations)).ToList();

                if (availableLocations.Count <= 0)
                {
                    Console.WriteLine($"Could not place {item.Type.ToString()}!");
                    return;
                }

                availableLocations[0].Fill(item);
                Console.WriteLine($"Placed {item.Type.ToString()} at {availableLocations[0].Name} with {unplacedItems.Count} items remaining\n");
                unfilledLocations.Remove(availableLocations[0]);
            }
            
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
    }
}
