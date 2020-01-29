using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MinishRandomizer.Core;
using MinishRandomizer.Randomizer.Logic;
using MinishRandomizer.Utilities;
using MinishRandomizer.Properties;
using System.Globalization;

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
        public readonly bool UseAny;

        public Item(string data, string commandScope = "")
        {
            var dataChunks = data.Split(':');
            var itemData = dataChunks[0].Split('.');
            if (itemData[0].TrimStart(' ').TrimEnd(' ') != "Items") 
            {
                throw new ParserException($"{commandScope}: \"{data}\" is not an item, make sure it has \"Items.\" prepended");
            }
            if (!Enum.TryParse(itemData[1], out Type))
            {
                throw new ParserException($"{commandScope}: \"{data}\" has an invalid itemType");
            }

            UseAny = false;
            SubValue = 0;
            if (itemData.Length >= 3)
            {
                if (itemData[2] == "*")
                {
                    UseAny = true;
                }
                else if (Enum.TryParse<KinstoneType>(itemData[2], out Kinstone))
                {
                    SubValue = (byte)Kinstone;
                }
                else if (!byte.TryParse(itemData[2], NumberStyles.HexNumber, null, out SubValue))
                {
                    throw new ParserException($"{commandScope}: \"{data}\" has an invalid itemSub");
                }
            }

            Dungeon = "";
            if (dataChunks.Length > 1)
            {
                Dungeon = dataChunks[1];
            }

            if (Type == ItemType.KinstoneX)
            {
                Kinstone = (KinstoneType)SubValue;
            }
            else
            {
                Kinstone = KinstoneType.UnTyped;
            }
        }

        public Item(ItemType type, byte subValue, string dungeon = "", bool useAny = false)
        {
            Type = type;
            SubValue = subValue;
            UseAny = useAny;
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
            return asItem.Type == Type && (asItem.SubValue == SubValue || asItem.UseAny || UseAny);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.Type.ToString() + "." + this.SubValue + ":" + this.Dungeon;
        }
    }

    public class Shuffler
    {
        public readonly string Version;
        public int Seed;
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
            Version = GetVersionName();

            Locations = new List<Location>();
            DungeonItems = new List<Item>();
            MajorItems = new List<Item>();
            NiceItems = new List<Item>();
            MinorItems = new List<Item>();
            LogicParser = new Parser();
        }

        public string GetVersionName()
        {
#if DEBUG
                return $"{AssemblyInfo.GetGitTag()}-DEBUG-{AssemblyInfo.GetGitHash()}";
#else
                return $"{AssemblyInfo.GetGitTag()}";
#endif
        }

        public string GetLogicIdentifier()
        {
            string fallbackName;
            string fallbackVersion;
            if (LogicPath != null)
            {
                fallbackName = Path.GetFileNameWithoutExtension(LogicPath);
                fallbackVersion = File.GetLastWriteTime(LogicPath).ToShortDateString();
            }
            else
            {
                fallbackName = "Default";
                fallbackVersion = Version;
            }

            string name = LogicParser.SubParser.LogicName ?? fallbackName;
            string version = LogicParser.SubParser.LogicVersion ?? fallbackVersion;

            return name;
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

        public uint GetSettingHash()
        {
            byte[] settingBytes = LogicParser.SubParser.GetSettingBytes();

            if (settingBytes.Length > 0)
            {
                return PatchUtil.Crc32(settingBytes, settingBytes.Length);
            }
            else
            {
                return 0;
            }

        }

        public uint GetGimmickHash()
        {
            byte[] gimmickBytes = LogicParser.SubParser.GetGimmickBytes();

            if (gimmickBytes.Length > 0)
            {
                return PatchUtil.Crc32(gimmickBytes, gimmickBytes.Length);
            }
            else
            {
                return 0;
            }
        }

        public string GetOptionsIdentifier()
        {
            return StringUtil.AsStringHex8((int)GetSettingHash()) + "-" + StringUtil.AsStringHex8((int)GetGimmickHash());
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

            LogicParser.SubParser.DuplicateAmountReplacements();
            LogicParser.SubParser.DuplicateIncrementalReplacements();
            parsedLocations.ForEach(location => { AddLocation(location); });
        }

        public void AddLocation(Location location)
        {
            bool replaced = false;
            if (LogicParser.SubParser.IncrementalReplacements.ContainsKey(location.Contents))
            {
                var key = location.Contents;
                var set = LogicParser.SubParser.IncrementalReplacements[key];
                var replacement = set[0];
                if (replacement.amount != 0)
                {
                    replacement.amount -= 1;
                    var newItem = new Item(replacement.item.Type, (byte)((replacement.item.SubValue + replacement.amount) % 256), replacement.item.Dungeon);
                    location.SetItem(newItem);
                    replaced = true;
                }

                if (replacement.amount == 0)
                {
                    set.RemoveAt(0);
                    if (LogicParser.SubParser.IncrementalReplacements[key].Count == 0)
                    {
                        LogicParser.SubParser.IncrementalReplacements.Remove(key);
                        Console.WriteLine("removed incremental key:" + key.Type);
                    }
                }
            }
            if (replaced == false && LogicParser.SubParser.AmountReplacements.ContainsKey(location.Contents))
            {
                var key = location.Contents;
                var set = LogicParser.SubParser.AmountReplacements[key];
                var replacement = set[0];
                if(replacement.amount!=0)
                {
                    replacement.amount -= 1;
                    location.SetItem(replacement.item);
                    replaced = true;
                }

                if (replacement.amount == 0)
                {
                    set.RemoveAt(0);
                    if (LogicParser.SubParser.AmountReplacements[key].Count == 0)
                    {
                        LogicParser.SubParser.AmountReplacements.Remove(key);
                        Console.WriteLine("removed key:" + key.Type);
                    }
                }
            }

            if (replaced == false && LogicParser.SubParser.Replacements.ContainsKey(location.Contents))
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

            // Final cache clear
            Locations.ForEach(location => location.InvalidateCache());
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

            if (assumedItems == null)
            {
                assumedItems = new List<Item>();
            }

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
            List<Location> availableLocations = locations.Where(location => location.IsAccessible(finalMajorItems, Locations)).ToList();

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
                List<Location> accessibleLocations = filledLocations.Where(location => location.IsAccessible(availableItems, Locations)).ToList();
                previousSize = accessibleLocations.Count;

                filledLocations.RemoveAll(location => accessibleLocations.Contains(location));

                List<Item> newItems = Location.GetItems(accessibleLocations);

                availableItems.AddRange(newItems);

                // Cache is invalidated between each sphere to make sure things work out
                Locations.ForEach(location => location.InvalidateCache());
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

                WriteElementPositions(writer);
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
        /// <param name="spoilerBuilder">The running spoiler log builder to append the locations to</param>
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
                List<Location> accessibleLocations = filledLocations.Where(location => location.IsAccessible(availableItems, Locations)).ToList();
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

                // Evaluating for different items, so cache is invalidated now
                Locations.ForEach(location => location.InvalidateCache());
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

            foreach (EventDefine define in LogicParser.GetEventDefines())
            {
                define.WriteDefineString(eventBuilder);
            }

            byte[] seedValues = new byte[4];
            seedValues[0] = (byte)((Seed >> 00) & 0xFF);
            seedValues[1] = (byte)((Seed >> 08) & 0xFF);
            seedValues[2] = (byte)((Seed >> 16) & 0xFF);
            seedValues[3] = (byte)((Seed >> 24) & 0xFF);

            eventBuilder.AppendLine("#define seedHashed 0x" + StringUtil.AsStringHex8((int)PatchUtil.Crc32(seedValues, 4)));
            eventBuilder.AppendLine("#define settingHash 0x" + StringUtil.AsStringHex8((int)GetSettingHash()));

            return eventBuilder.ToString();
        }

        /// <summary>
        /// Move the elements around in a randomized ROM
        /// </summary>
        /// <param name="w">Writer to write with</param>
        private void WriteElementPositions(Writer w)
        {
            // Write coordinates for each element
            Location earthLocation = Locations.Where(loc => loc.Contents.Type == ItemType.EarthElement).First();
            MoveElement(w, earthLocation);

            Location fireLocation = Locations.Where(loc => loc.Contents.Type == ItemType.FireElement).First();
            MoveElement(w, fireLocation);

            Location waterLocation = Locations.Where(loc => loc.Contents.Type == ItemType.WaterElement).First();
            MoveElement(w, waterLocation);

            Location windLocation = Locations.Where(loc => loc.Contents.Type == ItemType.WindElement).First();
            MoveElement(w, windLocation);
        }

        /// <summary>
        /// Moves a single element marker to the location that contains it
        /// </summary>
        /// <param name="w">The writer to write to</param>
        /// <param name="location">The location that contains the element</param>
        private void MoveElement(Writer w, Location location)
        {
            // Coordinates for the unzoomed map
            byte[] largeCoords = new byte[2];

            // Coordinates for the zoomed in map
            ushort[] smallCoords = new ushort[2];
            switch (location.Name)
            {
                case "DeepwoodPrize":
                    largeCoords[0] = 0xB2;
                    largeCoords[1] = 0x7A;

                    smallCoords[0] = 0x0D7D;
                    smallCoords[1] = 0x0AC8;
                    break;
                case "CoFPrize":
                    largeCoords[0] = 0x3B;
                    largeCoords[1] = 0x1B;

                    smallCoords[0] = 0x01E8;
                    smallCoords[1] = 0x0178;
                    break;
                case "FortressPrize":
                    largeCoords[0] = 0x4B;
                    largeCoords[1] = 0x77;

                    smallCoords[0] = 0x0378;
                    smallCoords[1] = 0x0A78;
                    break;
                case "DropletsPrize":
                    largeCoords[0] = 0xB5;
                    largeCoords[1] = 0x4B;

                    smallCoords[0] = 0x0DB8;
                    smallCoords[1] = 0x0638;
                    break;
                case "KingGift":
                    largeCoords[0] = 0x5A;
                    largeCoords[1] = 0x15;

                    smallCoords[0] = 0x04DC;
                    smallCoords[1] = 0x0148;
                    break;
                case "PalacePrize":
                    largeCoords[0] = 0xB5;
                    largeCoords[1] = 0x1B;

                    smallCoords[0] = 0x0D88;
                    smallCoords[1] = 0x00E8;
                    break;
                default:
                    return;
            }

            int largeAddress;
            int smallAddress;

            switch (location.Contents.Type)
            {
                case ItemType.EarthElement:
                    largeAddress = 0x128699;
                    smallAddress = 0x12869C;
                    break;
                case ItemType.FireElement:
                    largeAddress = 0x1286A1;
                    smallAddress = 0x1286A4;
                    break;
                case ItemType.WaterElement:
                    largeAddress = 0x1286B1;
                    smallAddress = 0x1286B4;
                    break;
                case ItemType.WindElement:
                    largeAddress = 0x1286A9;
                    smallAddress = 0x1286AC;
                    break;
                default:
                    return;
            }

            // Write zoomed out coordinates
            w.SetPosition(largeAddress);
            w.WriteByte(largeCoords[0]);
            w.WriteByte(largeCoords[1]);

            // Write zoomed in coordinates
            w.SetPosition(smallAddress);
            w.WriteUInt16(smallCoords[0]);
            w.WriteUInt16(smallCoords[1]);
        }

        public void ClearLogic()
        {
            Locations.Clear();
            DungeonItems.Clear();
            MajorItems.Clear();
            NiceItems.Clear();
            MinorItems.Clear();

            LogicParser.SubParser.ClearTypeOverrides();
            LogicParser.SubParser.ClearIncrementalReplacements();
            LogicParser.SubParser.ClearReplacements();
            LogicParser.SubParser.ClearAmountReplacements();
            LogicParser.SubParser.ClearDefines();
            LogicParser.SubParser.AddOptions();
        }
    }
}
