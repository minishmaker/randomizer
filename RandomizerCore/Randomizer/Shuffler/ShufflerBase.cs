using System.Reflection;
using System.Text;
using ColorzCore;
using RandomizerCore.Controllers.Models;
using RandomizerCore.Core;
using RandomizerCore.Random;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Helpers;
using RandomizerCore.Randomizer.Helpers.Models;
using RandomizerCore.Randomizer.Logic.Dependency;
using RandomizerCore.Randomizer.Logic.Location;
using RandomizerCore.Randomizer.Logic.Options;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.Extensions;
using RandomizerCore.Utilities.IO;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Models;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer.Shuffler;

internal abstract class ShufflerBase
{
    #region Properties
        #region Public
        
            public readonly string Version;

            public ulong Seed { get; private set; } = 0xe1fb1ade2dabb1edUL;

        #endregion

        #region Protected

            //Item lists are sorted in the order they are processed
            protected readonly List<Item> DungeonConstraints;
            protected readonly List<Item> DungeonEntrances;
            protected readonly List<Item> DungeonMajorItems;
            protected readonly List<Item> DungeonMinorItems;
            protected readonly List<Item> DungeonPrizes;
            protected readonly List<Item> FillerItems;

            protected readonly List<Location> Locations;
            protected readonly Parser.Parser LogicParser;
            protected readonly List<Item> MajorItems;
            protected readonly List<Item> MinorItems;
            
            protected readonly List<Item> Music;
            protected readonly List<Item> OverworldConstraints;
            protected readonly List<Item> UnshuffledItems;
            
            protected List<Location> FilledLocations;
            
            protected bool Randomized;
            
            protected SquaresRandomNumberGenerator Rng;
            
            protected OptionList? Options;

            protected string? LogicPath;

        #endregion
    #endregion

    #region Public Functions

        public string GetVersionName()
        {
            return "0.7.0";
        }    
        
        /// <summary>
        ///     Throws a <code>ShufflerConfigurationException</code> if state is not valid with a message describing the
        ///     validation failure
        /// </summary>
        public void ValidateState(bool checkIfRandomized = false)
        {
            Logger.Instance.LogInfo("Beginning Shuffler State Validation");

            if (Rom.Instance == null)
                throw new ShufflerConfigurationException("No ROM loaded! You must load a ROM before randomization.");

            if (!RomCrcValid(Rom.Instance))
                throw new ShufflerConfigurationException("ROM does not match the expected CRC for the logic file!");

            if (checkIfRandomized && !Randomized)
                throw new ShufflerConfigurationException(
                    "You must randomize the ROM before saving the ROM or a patch file!");

            Logger.Instance.LogInfo("Shuffler State Validation Succeeded");
            Logger.Instance.SaveLogTransaction();
        }

        public void SetSeed(ulong seed)
        {
            Seed = seed;
            Rng = new SquaresRandomNumberGenerator(SquaresRandomNumberGenerator.DefaultKey, seed);
            Logger.Instance.LogInfo($"Randomization seed set to {seed:X}");
        }

        /// <summary>
        ///     Load the flags that a logic file uses to customize itself
        /// </summary>
        public void LoadOptions(string? logicFile = null)
        {
            Logger.Instance.LogInfo("Loading Logic Options");
            LogicParser.SubParser.ClearOptions();

            string[] logicStrings;

            if (string.IsNullOrEmpty(logicFile))
            {
                // Load default logic if no alternative is specified
                var assembly = Assembly.GetAssembly(typeof(ShufflerBase));
                using (var stream = assembly?.GetManifestResourceStream("RandomizerCore.Resources.default.logic"))
                using (var reader = new StreamReader(stream))
                {
                    var allLocations = reader.ReadToEnd();
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
        
        public int ApplyPatch(string romLocation, string? patchFile = null)
        {
            if (string.IsNullOrEmpty(patchFile))
            {
                // Get directory of MinishRandomizer 
                var assemblyPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                patchFile = assemblyPath + "/Patches/ROM Buildfile.event";
            }

            // Write new patch file to patch folder/extDefinitions.event
            File.WriteAllText(Path.GetDirectoryName(patchFile) + "/extDefinitions.event", GetEventWrites());

            string[] args = { "A", "FE8", "-input:" + patchFile, "-output:" + romLocation };

            Program.CustomOutputStream = null;

            return Program.Main(args);
        }

        public int ApplyPatch(Stream patchedRom, string? patchFile = null)
        {
            if (string.IsNullOrEmpty(patchFile))
            {
                // Get directory of MinishRandomizer 
                var assemblyPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                patchFile = assemblyPath + "/Patches/ROM Buildfile.event";
            }

            // Write new patch file to patch folder/extDefinitions.event
            File.WriteAllText(Path.GetDirectoryName(patchFile) + "/extDefinitions.event", GetEventWrites());

            string[] args = { "A", "FE8", "-input:" + patchFile, "-output:" + "usingAlternateStream" };

            Program.CustomOutputStream = patchedRom;

            return Program.Main(args);
        }

        /// <summary>
        ///     Get the contents of the spoiler log, including playthrough
        /// </summary>
        /// <returns>The contents of the spoiler log</returns>
        public string GetSpoiler()
        {
            var spoilerBuilder = new StringBuilder();
            spoilerBuilder.AppendLine("Spoiler for Minish Cap Randomizer");
            spoilerBuilder.AppendLine($"Seed: {Seed:X}");
            spoilerBuilder.AppendLine(
                $"Version: {ControllerBase.VersionIdentifier} {ControllerBase.RevisionIdentifier}");
            var logicSettings = GetFinalOptions().OnlyLogic();
            spoilerBuilder.AppendLine(
                $"Settings String: {MinifiedSettings.GenerateSettingsString(logicSettings.GetSorted(), logicSettings.GetCrc32())}");

            spoilerBuilder.AppendLine();
            AppendLocationSpoiler(spoilerBuilder);

            spoilerBuilder.AppendLine();
            AppendPlaythroughSpoiler(spoilerBuilder);

            spoilerBuilder.AppendLine();
            AddActualPlaythroughSpoiler(spoilerBuilder);


            return spoilerBuilder.ToString();
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

            var name = LogicParser.SubParser.LogicName ?? fallbackName;
            var version = LogicParser.SubParser.LogicVersion ?? fallbackVersion;

            return name;
        }
    
        public List<LogicOptionBase> GetSortedSettings()
        {
            return LogicParser.SubParser.GetSortedSettings();
        }
    
        public List<LogicOptionBase> GetSortedCosmetics()
        {
            return LogicParser.SubParser.GetSortedCosmetics();
        }
    
        public uint GetLogicOptionsCrc32()
        {
            return LogicParser.SubParser.GetLogicOptionsCrc32();
        }

        public uint GetCosmeticOptionsCrc32()
        {
            return LogicParser.SubParser.GetCosmeticOptionsCrc32();
        }

        public OptionList GetSelectedOptions()
        {
            return new OptionList(LogicParser.SubParser.Options);
        }

        public OptionList GetFinalOptions()
        {
            return Options ??= new OptionList(LogicParser.SubParser.Options);
        }

        public uint GetSettingHash()
        {
            var settingBytes = LogicParser.SubParser.GetSettingBytes();

            return settingBytes.Length > 0 ? settingBytes.Crc32() : 0;
        }

        public uint GetCosmeticsHash()
        {
            var cosmeticBytes = LogicParser.SubParser.GetCosmeticBytes();

            return cosmeticBytes.Length > 0 ? cosmeticBytes.Crc32() : 0;
        }

        public string GetOptionsIdentifier()
        {
            return StringUtil.AsStringHex8((int)GetSettingHash()) + "-" + StringUtil.AsStringHex8((int)GetCosmeticsHash());
        }
        
        /// <summary>
        ///     Get a byte[] of the randomized data
        /// </summary>
        /// <returns>The data of the randomized ROM</returns>
        public byte[] GetRandomizedRom()
        {
            // Create a copy of the ROM data to modify for output
            var outputBytes = new byte[Rom.Instance.RomData.Length];
            Array.Copy(Rom.Instance.RomData, 0, outputBytes, 0, outputBytes.Length);

            using (var ms = new MemoryStream(outputBytes))
            {
                var writer = new Writer(ms);
                foreach (var location in Locations) location.WriteLocation(writer);

                WriteElementPositions(writer);
                UpdateEntrances(writer);
            }

            return outputBytes;
        }
    
    #endregion

    #region Abstract Functions

        #region Public

            public abstract void LoadLocations(string? logicFile = null);

            public abstract void LoadLocationsYaml(string? logicFile = null, string? yamlFileLogic = null,
                string? yamlFileCosmetics = null, bool useGlobalYAML = false);

            public abstract string GetEventWrites();

            public abstract void RandomizeLocations();

        #endregion

    #endregion

    protected ShufflerBase()
    {
        Version = GetVersionName();

        Locations = new List<Location>();

        Music = new List<Item>();
        UnshuffledItems = new List<Item>();

        DungeonEntrances = new List<Item>();
        DungeonConstraints = new List<Item>();
        OverworldConstraints = new List<Item>();

        DungeonPrizes = new List<Item>();
        DungeonMajorItems = new List<Item>();
        DungeonMinorItems = new List<Item>();
        MajorItems = new List<Item>();

        MinorItems = new List<Item>();
        FillerItems = new List<Item>();

        FilledLocations = new List<Location>();

        LogicParser = new Parser.Parser();

        Rng = new SquaresRandomNumberGenerator();
    }

    #region Protected Functions

        protected Item AddItem(Item item)
        {
            var newItem = CheckReplacements(item);

            if (newItem.HasValue) item = newItem.Value;

            switch (item.ShufflePool)
            {
                case ItemPool.Music:
                    Music.Add(item);
                    break;
                case ItemPool.Unshuffled:
                    break;
                case ItemPool.DungeonEntrance:
                    DungeonEntrances.Add(item);
                    break;
                case ItemPool.DungeonConstraint:
                    DungeonConstraints.Add(item);
                    break;
                case ItemPool.OverworldConstraint:
                    OverworldConstraints.Add(item);
                    break;
                case ItemPool.DungeonPrize:
                    DungeonPrizes.Add(item);
                    break;
                case ItemPool.DungeonMajor:
                    DungeonMajorItems.Add(item);
                    break;
                case ItemPool.DungeonMinor:
                    DungeonMinorItems.Add(item);
                    break;
                case ItemPool.Major:
                    MajorItems.Add(item);
                    break;
                case ItemPool.Minor:
                    MinorItems.Add(item);
                    break;
                case ItemPool.Filler:
                    FillerItems.Add(item);
                    break;
                default:
                    MinorItems.Add(item);
                    break;
            }

            return item;
        }

        protected string[] LoadLocationFile(string? logicFile = null)
        {
            string[] locationStrings;

            // Get the logic file as an array of strings that can be parsed
            if (string.IsNullOrEmpty(logicFile))
            {
                // Load default logic if no alternative is specified
                var assembly = Assembly.GetAssembly(typeof(Shuffler));
                using (var stream = assembly?.GetManifestResourceStream("RandomizerCore.Resources.default.logic"))
                using (var reader = new StreamReader(stream))
                {
                    var allLocations = reader.ReadToEnd();
                    // Each line is a different location, split regardless of return form
                    locationStrings = allLocations.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                }
            }
            else
            {
                locationStrings = File.ReadAllLines(logicFile);
            }

            return locationStrings;
        }

        protected Location AddLocation(Location location)
        {
            // All locations are in the master location list
            Locations.Add(location);

            if (!location.Contents.HasValue) return location;

            var newItem = CheckReplacements(location.Contents.Value);

            if (newItem.HasValue) location.SetItem(newItem.Value);


            if (LogicParser.SubParser.LocationTypeOverrides.ContainsKey(location.Contents.Value))
                location.Type = LogicParser.SubParser.LocationTypeOverrides[location.Contents.Value];

            if (location.Type != LocationType.Unshuffled) return location;

            // Unshuffled locations require contents, so add them here
            location.Fill(location.Contents!.Value);
            UnshuffledItems.Add(location.Contents!.Value);

            return location;
        }

        protected void ClearLogic()
        {
            DependencyBase.BeatVaatiDependency = null;
            Location.ShufflerConstraints.Clear();
            Locations.Clear();

            Music.Clear();
            UnshuffledItems.Clear();

            DungeonEntrances.Clear();
            DungeonConstraints.Clear();
            OverworldConstraints.Clear();

            DungeonPrizes.Clear();
            DungeonMajorItems.Clear();
            DungeonMinorItems.Clear();
            MajorItems.Clear();

            MinorItems.Clear();
            FillerItems.Clear();

            FilledLocations.Clear();

            LogicParser.SubParser.ClearTypeOverrides();
            LogicParser.SubParser.ClearIncrementalReplacements();
            LogicParser.SubParser.ClearReplacements();
            LogicParser.SubParser.ClearAmountReplacements();
            LogicParser.SubParser.ClearDefines();
        }

        /// <summary>
        ///     Fill items in locations that are available at the start of the fill.
        ///     Slower than FastFillLocations, but will not place in unavailable locations.
        /// </summary>
        /// <param name="items">The items to fill with</param>
        /// <param name="locations">The locations in which to fill the items</param>
        protected void CheckedFastFillLocations(List<Item> items, List<Location> locations)
        {
            var availableLocations =
                locations.Where(location => location.IsAccessible()).ToList();

            foreach (var item in items)
            {
                var locationIndex = Rng.Next(availableLocations.Count);
                availableLocations[locationIndex].Fill(item);
            }
        }

        /// <summary>
        ///     Fill items in locations without checking logic for speed
        /// </summary>
        /// <param name="items">The items to fill with</param>
        /// <param name="locations">The locations in which to fill the items</param>
        protected void FastFillLocations(List<Item> items, List<Location> locations)
        {
            var nonFillerItems = items.Where(item => item.ShufflePool is not ItemPool.Filler);
            // Don't need to check logic, cause the items being placed do not affect logic
            foreach (var item in nonFillerItems)
            {
                if (locations.Count == 0) return;

                FilledLocations.Add(locations[0]);
                locations[0].Fill(item);
                locations.RemoveAt(0);
            }

            if (locations.Count == 0) return;

            var fillItems = items.Where(item => item.ShufflePool == ItemPool.Filler).ToList();
            var rand = new SquaresRandomNumberGenerator(SquaresRandomNumberGenerator.DefaultKey, Seed);
            while (locations.Count > 0)
            {
                FilledLocations.Add(locations[0]);
                locations[0].Fill(fillItems[rand.Next(fillItems.Count)]);
                locations.RemoveAt(0);
            }
        }

        protected void FastFillAndConsiderItemPlaced(List<Item> items, List<Location> locations)
        {
            foreach (var item in items)
            {
                if (locations.Count == 0) return;

                var l = locations.Where(loc => loc.Dungeons.Contains(item.Dungeon)).ToList();
                if (l.Count == 0) throw new Exception("Invalid logic file! No valid location for constraint found!");
                l.Shuffle(Rng);
                
                FilledLocations.Add(l[0]);
                l[0].Fill(item);
                item.NotifyParentDependencies(true);
                locations.Remove(l[0]);
            }
        }

        protected List<Location> UpdateObtainedItemsFromPlacedLocations()
        {
            var allAvailableLocations = new List<Location>();
            var availableLocations = FilledLocations.Where(location => location.IsAccessible()).ToList();

            while (availableLocations.Count > 0)
            {
                allAvailableLocations.AddRange(availableLocations);

                foreach (var location in availableLocations)
                {
                    FilledLocations.Remove(location);
                    location.Contents!.Value.NotifyParentDependencies(true);
                }

                availableLocations = FilledLocations.Where(location => location.IsAccessible()).ToList();
            }

            return allAvailableLocations;
        }
        
        /// <summary>
        ///     Gets all the available items with a given item set, looping until there are no more items left to get
        /// </summary>
        /// <param name="preAvailableItems">Items that are available from the start</param>
        /// <returns>A list of all the items that are logically accessible</returns>
        protected List<Item> GetAvailableItems(List<Item> preAvailableItems)
        {
            var availableItems = preAvailableItems.ToList();

            availableItems.ForEach(item => item.NotifyParentDependencies(true));

            var filledLocations = Locations.Where(location =>
                location is
                {
                    Filled: true,
                    Type: not LocationType.Helper and not LocationType.Untyped and not LocationType.Inaccessible
                }).ToList();

            int previousSize;

            // Get "spheres" until the next sphere contains no new items
            do
            {
                var accessibleLocations =
                    filledLocations.Where(location => location.IsAccessible()).ToList();
                previousSize = accessibleLocations.Count;

                filledLocations.RemoveAll(location => accessibleLocations.Contains(location));

                var newItems = Location.GetItems(accessibleLocations);
                newItems.ForEach(item => item.NotifyParentDependencies(true));

                availableItems.AddRange(newItems);
            } while (previousSize > 0);

            return availableItems;
        }
        
    #endregion

    #region Private Functions

        private Item? CheckReplacements(Item item)
        {
            if (LogicParser.SubParser.IncrementalReplacements.ContainsKey(item))
            {
                var set = LogicParser.SubParser.IncrementalReplacements[item];
                var replacement = set[0];
                if (replacement.Amount != 0)
                {
                    replacement.Amount -= 1;
                    var newItem = new Item(replacement.Item.Type,
                        (byte)((replacement.Item.SubValue + replacement.Amount) % 256), replacement.Item.Dungeon, false,
                        item.ShufflePool);
                    return newItem;
                }

                if (replacement.Amount == 0)
                {
                    set.RemoveAt(0);
                    if (LogicParser.SubParser.IncrementalReplacements[item].Count == 0)
                    {
                        LogicParser.SubParser.IncrementalReplacements.Remove(item);
                        Logger.Instance.LogInfo($"Removed incremental item, key {item.Type}");
                    }
                }
            }

            if (LogicParser.SubParser.AmountReplacements.ContainsKey(item))
            {
                var set = LogicParser.SubParser.AmountReplacements[item];
                var replacement = set[0];
                if (replacement.Amount != 0)
                {
                    replacement.Amount -= 1;
                    var newItem = replacement.Item;
                    return new Item(newItem.Type, newItem.SubValue, newItem.Dungeon, false, item.ShufflePool);
                }

                if (replacement.Amount == 0)
                {
                    set.RemoveAt(0);
                    if (LogicParser.SubParser.AmountReplacements[item].Count == 0)
                    {
                        LogicParser.SubParser.AmountReplacements.Remove(item);
                        Console.WriteLine("removed key:" + item.Type);
                    }
                }
            }

            if (LogicParser.SubParser.Replacements.ContainsKey(item))
            {
                var chanceSet = LogicParser.SubParser.Replacements[item];
                var number = Rng.Next(chanceSet.TotalChance);
                var val = 0;

                for (var i = 0; i < chanceSet.RandomItems.Count(); i++)
                {
                    val += chanceSet.RandomItems[i].Chance;
                    if (number < val)
                    {
                        var newItem = chanceSet.RandomItems[i].Item;
                        return new Item(newItem.Type, newItem.SubValue, newItem.Dungeon, false, item.ShufflePool);
                    }
                }
            }

            return null;
        }

        private bool RomCrcValid(Rom rom)
        {
            if (LogicParser.SubParser.RomCrc != null)
                return rom.RomData.Crc32() == LogicParser.SubParser.RomCrc;
            return true;
        }

        /// <summary>
        ///     Create list of filled locations and their contents
        /// </summary>
        /// <param name="spoilerBuilder">The running spoiler log builder to append the locations to</param>
        private void AppendLocationSpoiler(StringBuilder spoilerBuilder)
        {
            spoilerBuilder.AppendLine("Location Contents:");
            // Get the locations that have been filled
            var nonNullLocations = Locations.Where(location => location.Contents is not null);

            var filledLocations = nonNullLocations.Where(location =>
                location is
                {
                    Filled: true,
                    Type: not LocationType.Helper and not LocationType.Untyped and not LocationType.Inaccessible
                }).ToList();

            var locationsWithRealItems = filledLocations.Where(location =>
                location.Contents!.Value.Type is not ItemType.Untyped && !location.HideFromSpoilerLog);

            var hackToFilterOutLanternGarbage = locationsWithRealItems.Where(location =>
                location.Contents!.Value.Type != ItemType.Lantern || location.Contents!.Value.SubValue == 0);

            foreach (var location in hackToFilterOutLanternGarbage)
            {
                spoilerBuilder.AppendLine($"{location.Name}: {location.Contents!.Value.Type}");

                AppendSubvalue(spoilerBuilder, location);

                spoilerBuilder.AppendLine();
            }
        }

        /// <summary>
        ///     Create list of items in the order they can logically be collected
        /// </summary>
        /// <param name="spoilerBuilder">The running spoiler log builder to append the playthrough to</param>
        private void AppendPlaythroughSpoiler(StringBuilder spoilerBuilder)
        {
            spoilerBuilder.AppendLine("Spheres:");

            var nonNullLocations = Locations.Where(location => location.Contents is not null);

            var filledLocations = nonNullLocations.Where(location =>
                location is
                {
                    Filled: true,
                    Type: not LocationType.Helper and not LocationType.Untyped and not LocationType.Inaccessible
                }).ToList();

            var availableItems = new List<Item>();

            int previousSize;
            var sphereCounter = 1;

            do
            {
                var accessibleLocations =
                    filledLocations.Where(location => location.IsAccessible()).ToList();
                previousSize = accessibleLocations.Count;

                filledLocations.RemoveAll(location => accessibleLocations.Contains(location));

                var newItems = Location.GetItems(accessibleLocations);
                newItems.ForEach(item => item.NotifyParentDependencies(true));
                availableItems.AddRange(newItems);

                foreach (var location in accessibleLocations)
                {
                    if (location.HideFromSpoilerLog) continue;
                    spoilerBuilder.AppendLine(
                        $"Sphere {sphereCounter}: {location.Contents!.Value.Type} in {location.Name}");

                    AppendSubvalue(spoilerBuilder, location);
                    spoilerBuilder.AppendLine();
                }

                sphereCounter++;
                spoilerBuilder.AppendLine();
            } while (previousSize > 0);
            
            availableItems.ForEach(item => item.NotifyParentDependencies(false));
        }

        private void AddActualPlaythroughSpoiler(StringBuilder builder)
        {
            builder.AppendLine("Playthrough:");

            var nonNullLocations = Locations.Where(location => location.Contents is not null);

            var filledLocations = nonNullLocations.Where(location =>
                location is
                {
                    Filled: true,
                    Type: not LocationType.Helper and not LocationType.Untyped and not LocationType.Inaccessible
                }).ToList();

            var availableItems = new List<Item>();

            int previousSize;
            var sphereCounter = 1;

            do
            {
                if (DependencyBase.BeatVaatiDependency!.DependencyFulfilled())
                {
                    builder.AppendLine($"Sphere {sphereCounter}: {{").AppendLine("\tBeat Vaati").AppendLine("}")
                        .AppendLine();
                    return;
                }

                var accessibleLocations =
                    filledLocations.Where(location => location.IsAccessible()).ToList();
                previousSize = accessibleLocations.Count;

                filledLocations.RemoveAll(location => accessibleLocations.Contains(location));

                var newItems = Location.GetItems(accessibleLocations);
                newItems.ForEach(item => item.NotifyParentDependencies(true));
                availableItems.AddRange(newItems);

                var majorLocations = accessibleLocations.Where(location => Core.MajorItems.IsMajorItem(location.Contents!.Value.Type)).ToList();

                builder.AppendLine($"Sphere {sphereCounter}: {{");

                foreach (var location in majorLocations)
                {
                    if (location.HideFromSpoilerLog) continue;

                    builder.AppendLine($"\t{location.Contents!.Value.Type} in {location.Name}");
                    AppendSubvalue(builder, location);
                    if (majorLocations.Last() != location)
                        builder.AppendLine();
                }

                builder.AppendLine("}");

                sphereCounter++;
                builder.AppendLine();
            } while (previousSize > 0);
            
            availableItems.ForEach(item => item.NotifyParentDependencies(false));
        }

        private void AppendSubvalue(StringBuilder spoilerBuilder, Location location)
        {
            if (!location.Contents.HasValue) return;

            // Display subvalue if relevant
            if (location.Contents.Value.Type == ItemType.Kinstone)
                spoilerBuilder.AppendLine($"\t\tKinstone Type: {location.Contents.Value.Kinstone}");
            else if (location.Contents.Value.Type is ItemType.ProgressiveItem)
                spoilerBuilder.AppendLine($"\t\tItem: {GetProgressiveItemName(location.Contents.Value.SubValue)}");
            else if (location.Contents.Value.Type is ItemType.Bottle)
                spoilerBuilder.AppendLine($"\t\tItem: {GetBottleName(location.Contents.Value.SubValue)}");
            else if (location.Contents.Value.Type is ItemType.Trap)
                spoilerBuilder.AppendLine($"\t\tItem: {GetTrapName(location.Contents.Value.SubValue)}");
            else if (location.Contents.Value.ShufflePool is ItemPool.DungeonEntrance)
                spoilerBuilder.AppendLine($"\t\tDungeon: {GetEntranceNameFromSubvalue(location.Contents.Value.SubValue)}");
            else if (location.Contents.Value.ShufflePool is ItemPool.DungeonMajor ||
                     (location.Contents.Value.SubValue != 0 &&
                      location.Contents.Value.Type is ItemType.SmallKey or ItemType.BigKey or ItemType.Compass
                          or ItemType.DungeonMap))
                spoilerBuilder.AppendLine(
                    $"\t\tDungeon: {GetDungeonNameFromDungeonSubvalue(location.Contents.Value.SubValue)}");
            else if (location.Contents.Value.SubValue != 0)
                spoilerBuilder.AppendLine($"\t\tSubvalue: {location.Contents.Value.SubValue}");

            // Display dungeon contents if relevant
            //if (!string.IsNullOrEmpty(location.Contents.Value.Dungeon))
            //	spoilerBuilder.AppendLine($"\t\tDungeon: {location.Contents.Value.Dungeon}");
        }

        private string GetProgressiveItemName(int subvalue)
        {
            return (ProgressiveItemType)subvalue switch
            {
                ProgressiveItemType.Sword => "Progressive Sword",
                ProgressiveItemType.Bow => "Progressive Bow",
                ProgressiveItemType.Boomerang => "Progressive Boomerang",
                ProgressiveItemType.Shield => "Progressive Shield",
                ProgressiveItemType.SpinAttack => "Progressive Scroll",
                _ => $"{subvalue}"
            };
        }

        private string GetBottleName(int subvalue)
        {
            return (BottleType)subvalue switch
            {
                BottleType.BottleEmpty => "Empty Contents",
                BottleType.BottleButter => "Hylian Butter",
                BottleType.BottleMilk => "Full Milk",
                BottleType.BottleHalfMilk => "Half Milk",
                BottleType.BottleRedPotion => "Red Potion",
                BottleType.BottleBluePotion => "Blue Potion",
                BottleType.BottleWater => "Water",
                BottleType.BottleMineralWater => "Mineral Water",
                BottleType.BottleFairy => "Fairy",
                BottleType.BottlePicolyteRed => "Red Picolyte",
                BottleType.BottlePicolyteOrange => "Orange Picolyte",
                BottleType.BottlePicolyteYellow => "Yellow Picolyte",
                BottleType.BottlePiclolyteGreen => "Green Picolyte",
                BottleType.BottlePicolyteBlue => "Blue Picolyte",
                BottleType.BottlePicolyteWhite => "White Picolyte",
                BottleType.BottleCharmNayru => "Nayru Charm",
                BottleType.BottleCharmFarore => "Farore Charm",
                BottleType.BottleCharmDin => "Din Charm",
                _ => $"{subvalue}"
            };
        }

        private string GetTrapName(int subvalue)
        {
            return (TrapType)subvalue switch
            {
                TrapType.Ice => "Ice",
                TrapType.Fire => "Fire",
                TrapType.Zap => "Electric",
                TrapType.Explosion => "Explosion",
                TrapType.MoneyDrain => "Money Drain",
                TrapType.Stink => "Stink",
                TrapType.Rope => "Rope",
                TrapType.Keese => "Keese",
                TrapType.LikeLike => "LikeLike",
                TrapType.Curse => "Curse",
                _ => $"{subvalue}"
            };
        }

        private string GetEntranceNameFromSubvalue(int subvalue)
        {
            return (DungeonEntranceType)subvalue switch
            {
                DungeonEntranceType.Dws => "Deepwood Shrine",
                DungeonEntranceType.CoF => "Cave of Flames",
                DungeonEntranceType.FoW => "Fortress of Winds",
                DungeonEntranceType.ToD => "Temple of Droplets",
                DungeonEntranceType.Crypt => "Royal Crypt",
                DungeonEntranceType.PoW => "Palace of Winds",
                _ => $"{subvalue}"
            };
        }

        private string GetDungeonNameFromDungeonSubvalue(int subvalue)
        {
            return (DungeonType)subvalue switch
            {
                DungeonType.Dojo => "Dojo",
                DungeonType.Universal => "Universal",
                DungeonType.Dws => "Deepwood Shrine",
                DungeonType.CoF => "Cave of Flames",
                DungeonType.FoW => "Fortress of Winds",
                DungeonType.ToD => "Temple of Droplets",
                DungeonType.PoW => "Palace of Winds",
                DungeonType.Dhc => "Dark Hyrule Castle",
                DungeonType.Crypt => "Royal Crypt",
                _ => $"{subvalue}"
            };
        }
        
        /// <summary>
        ///     Move the elements around in a randomized ROM
        /// </summary>
        /// <param name="w">Writer to write with</param>
        private void WriteElementPositions(Writer w)
        {
            // Write coordinates for each element
            var earthLocation = Locations.First(loc =>
                loc.Contents is not null && loc.Contents.Value.Type == ItemType.EarthElement);
            MoveElement(w, earthLocation);

            var fireLocation =
                Locations.First(loc => loc.Contents is not null && loc.Contents.Value.Type == ItemType.FireElement);
            MoveElement(w, fireLocation);

            var waterLocation = Locations.First(loc =>
                loc.Contents is not null && loc.Contents.Value.Type == ItemType.WaterElement);
            MoveElement(w, waterLocation);

            var windLocation =
                Locations.First(loc => loc.Contents is not null && loc.Contents.Value.Type == ItemType.WindElement);
            MoveElement(w, windLocation);
        }

        /// <summary>
        ///     Moves a single element marker to the location that contains it
        /// </summary>
        /// <param name="w">The writer to write to</param>
        /// <param name="location">The location that contains the element</param>
        private void MoveElement(Writer w, Location prizeLocation)
        {
            // Coordinates for the unzoomed map
            var largeCoords = new byte[2];

            // Coordinates for the zoomed in map
            var smallCoords = new ushort[2];
            (ushort largeAddress, uint smallAdress) coords = (0, 0);
            Location correspondingEntrance;
            switch (prizeLocation.Name)
            {
                case "Deepwood_Prize":
                    correspondingEntrance = Locations.First(location =>
                        location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled &&
                        location.Contents is not null &&
                        (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.Dws);
                    coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                    break;
                case "CoF_Prize":
                    correspondingEntrance = Locations.First(location =>
                        location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled &&
                        location.Contents is not null &&
                        (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.CoF);
                    coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                    break;
                case "Fortress_Prize":
                    correspondingEntrance = Locations.First(location =>
                        location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled &&
                        location.Contents is not null &&
                        (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.FoW);
                    coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                    break;
                case "Droplets_Prize":
                    correspondingEntrance = Locations.First(location =>
                        location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled &&
                        location.Contents is not null &&
                        (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.ToD);
                    coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                    break;
                case "Crypt_Prize":
                    correspondingEntrance = Locations.First(location =>
                        location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled &&
                        location.Contents is not null &&
                        (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.Crypt);
                    coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                    break;
                case "Palace_Prize":
                    correspondingEntrance = Locations.First(location =>
                        location.Type is LocationType.DungeonEntrance or LocationType.Unshuffled &&
                        location.Contents is not null &&
                        (DungeonEntranceType)location.Contents.Value.SubValue is DungeonEntranceType.PoW);
                    coords = GetAddressFromDungeonEntranceName(correspondingEntrance.Name);
                    break;
                default:
                    return;
            }

            largeCoords[0] = (byte)((coords.largeAddress >> 8) & 0xFF);
            largeCoords[1] = (byte)(coords.largeAddress & 0xFF);

            smallCoords[0] = (ushort)((coords.smallAdress >> 16) & 0xFFFF);
            smallCoords[1] = (ushort)(coords.smallAdress & 0xFFFF);

            int largeAddress;
            int smallAddress;

            switch (prizeLocation.Contents!.Value.Type)
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

        private (ushort largeAddress, uint smallAdress) GetAddressFromDungeonEntranceName(string locationName)
        {
            return locationName switch
            {
                "Deepwood_Entrance" => (0xB27A, 0x0D7D0AC8),
                "CoF_Entrance" => (0x3B1B, 0x01E80178),
                "Fortress_Entrance" => (0x4B77, 0x03780A78),
                "Droplets_Entrance" => (0xB54B, 0x0DB80638),
                "Crypt_Entrance" => (0x5A15, 0x04DC0148),
                "Palace_Entrance" => (0xB51B, 0x0D8800E8),
                "DHC_Main_Entrance" => (0x7812, 0x07C800F8),
                "DHC_Side_Entrance" => (0x6D14, 0x06400118),
            };
        }

        private void UpdateEntrances(Writer w)
        {
            var entranceChangedLocations =
                Locations.Where(location => location.Type is LocationType.DungeonEntrance).ToList();

            foreach (var entrance in entranceChangedLocations) UpdateSpecialEntrances(w, entrance);
        }

        /// <summary>
        ///     Updates the following dungeon entrances to point to the correct entrance in dungeon entrance rando and sets the
        ///     correct entrance type based on the dungeon
        ///     Enter ToD from Portal in Lake Hylia
        ///     Leave ToD
        ///     Enter PoW from the Large Tornado at top of Wind Tribe
        ///     Leave PoW
        ///     After Crypt Prize
        ///     After Fortress Prize
        ///     Green warps in: DWS, CoF, FoW, ToD, PoW
        ///     Other entrance shuffle
        /// </summary>
        /// <param name="w"></param>
        /// <param name="location"></param>
        private void UpdateSpecialEntrances(Writer w, Location location)
        {
            //Dungeon exits and green warps
            const uint dwsExit = 0x138172;
            const uint dwsGreenWarp = 0xDF06C;

            const uint cofExit = 0x138352;
            const uint cofGreenWarp = 0xE0F34;

            const uint fowExit = 0x13549A;
            const uint fowGreenWarp = 0xE2308;
            const uint fowAfterElement = 0x13A25E; //FoW has two ways of leaving, one on element, one on green warp

            const uint todExit = 0x13A47A;
            const uint todGreenWarp = 0xE40F4; //Normally warps back into the dungeon, we change it to go outside

            const uint cryptExit = 0x138EAA;
            const uint cryptElementWarp = 0x13A2AE; //Crypt doesn't have a green warp, just a warp after getting the item from King

            const uint powExit = 0x1082A1;
            const uint powGreenWarp = 0xE6A14;

            const uint dhcMainExit = 0x139426; //DHC has no green warp :)
            const uint dhcSideExit = 0x137F52; //DHC has no green warp :)

            if (!location.Contents.HasValue || location.Contents.Value.ShufflePool is not ItemPool.DungeonEntrance) return;

            ushort entranceX;
            ushort entranceY;
            byte entranceLayerOrHeight;
            byte entranceAnimation;
            byte targetArea;
            byte targetRoom;
            byte facingDirection;
            var exitAddress = 0u;
            var greenWarpAddress = 0u;
            var elementWarpAddress = 0u;
            var holeWarpAddress = 0u;
            
            switch ((DungeonEntranceType)location.Contents.Value.SubValue)
            {
                case DungeonEntranceType.Dws:
                    entranceX = 0xA8;
                    entranceY = 0xD8;
                    entranceLayerOrHeight = 0x1;
                    targetArea = 0x48;
                    targetRoom = 0x0B;
                    entranceAnimation = 0x0;
                    facingDirection = 0x0;
                    exitAddress = dwsExit;
                    greenWarpAddress = dwsGreenWarp;
                    break;
                case DungeonEntranceType.CoF:
                    entranceX = 0x88;
                    entranceY = 0xA8;
                    entranceLayerOrHeight = 0x1;
                    targetArea = 0x50;
                    targetRoom = 0x03;
                    entranceAnimation = 0x0;
                    facingDirection = 0x0;
                    exitAddress = cofExit;
                    greenWarpAddress = cofGreenWarp;
                    break;
                case DungeonEntranceType.FoW:
                    entranceX = 0x01D8;
                    entranceY = 0xB0;
                    entranceLayerOrHeight = 0x1;
                    targetArea = 0x18;
                    targetRoom = 0x00;
                    entranceAnimation = 0x0;
                    facingDirection = 0x0;
                    exitAddress = fowExit;
                    greenWarpAddress = fowGreenWarp;
                    elementWarpAddress = fowAfterElement;
                    break;
                case DungeonEntranceType.ToD:
                    entranceX = 0x0108;
                    entranceY = 0xC8;
                    entranceLayerOrHeight = 0x2;
                    targetArea = 0x60;
                    targetRoom = 0x03;
                    entranceAnimation = 0x2;
                    facingDirection = 0x4;
                    exitAddress = todExit;
                    greenWarpAddress = todGreenWarp;
                    break;
                case DungeonEntranceType.Crypt:
                    entranceX = 0x88;
                    entranceY = 0x78;
                    entranceLayerOrHeight = 0x1;
                    targetArea = 0x68;
                    targetRoom = 0x08;
                    entranceAnimation = 0x0;
                    facingDirection = 0x0;
                    exitAddress = cryptExit;
                    elementWarpAddress = cryptElementWarp;
                    break;
                case DungeonEntranceType.PoW:
                    entranceX = 0x268;
                    entranceY = 0x58;
                    entranceLayerOrHeight = 0x1;
                    targetArea = 0x70;
                    targetRoom = 0x31;
                    entranceAnimation = 0x0A;
                    facingDirection = 0x6;
                    holeWarpAddress = powExit;
                    greenWarpAddress = powGreenWarp;
                    break;
                case DungeonEntranceType.DHCMain:
                    entranceX = 0x198;
                    entranceY = 0x1F0;
                    entranceLayerOrHeight = 0x1;
                    targetArea = 0x88;
                    targetRoom = 0x00;
                    entranceAnimation = 0x0;
                    facingDirection = 0x0;
                    exitAddress = dhcMainExit;
                    break;
                case DungeonEntranceType.DHCSide:
                    entranceX = 0x68;
                    entranceY = 0x1A8;
                    entranceLayerOrHeight = 0x1;
                    targetArea = 0x43;
                    targetRoom = 0x00;
                    entranceAnimation = 0x0;
                    facingDirection = 0x0;
                    exitAddress = dhcSideExit;
                    break;
                default:
                    return;
            }

            ITransition transition;

            switch (location.Name)
            {
                case "Deepwood_Entrance":
                    transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.Dws);
                    break;
                case "CoF_Entrance":
                    transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.CoF);
                    break;
                case "Fortress_Entrance":
                    transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.FoW);
                    break;
                case "Droplets_Entrance":
                    transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.ToD);
                    break;
                case "Crypt_Entrance":
                    transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.Crypt);
                    break;
                case "Palace_Entrance":
                    transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.PoW);
                    break;
                case "DHC_Main_Entrance":
                    transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.DHCMain);
                    break;
                case "DHC_Side_Entrance":
                    transition = TransitionFactory.BuildTransitionFromDungeonEntranceType(DungeonEntranceType.DHCSide);
                    break;
                default:
                    return;
            }

            var addressesToWrite = transition.GetTransitionOffsets(targetArea, targetRoom, entranceLayerOrHeight,
                entranceAnimation, entranceX, entranceY, facingDirection, exitAddress, greenWarpAddress, elementWarpAddress,
                holeWarpAddress);

            foreach (var address in addressesToWrite)
            {
                if (address.Value is ushort)
                {
                    w.SetPosition(address.Key);
                    w.WriteUInt16((ushort)address.Value);
                }
                else if (address.Value is byte)
                {
                    w.SetPosition(address.Key);
                    w.WriteByte((byte)address.Value);
                }
            }
        }

        #endregion

}
