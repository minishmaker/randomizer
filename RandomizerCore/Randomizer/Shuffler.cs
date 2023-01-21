using System.Diagnostics;
using System.Reflection;
using System.Text;
using ColorzCore;
using RandomizerCore.Core;
using RandomizerCore.Properties;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Logic.Dependency;
using RandomizerCore.Randomizer.Logic.Location;
using RandomizerCore.Randomizer.Logic.Options;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.Extensions;
using RandomizerCore.Utilities.IO;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer;

internal class Shuffler
{
    public readonly string Version;

    //private List<Location> StartingLocations;
    private readonly List<Location> _locations;
    private readonly Parser.Parser _logicParser;
    private string? _logicPath;
    //Item lists are sorted in the order they are processed
    private readonly List<Item> _music;
    private readonly List<Item> _unshuffledItems;
    private readonly List<Item> _dungeonEntrances;
    private readonly List<Item> _dungeonConstraints;
    private readonly List<Item> _overworldConstraints;
    private readonly List<Item> _dungeonPrizes;
    private readonly List<Item> _dungeonMajorItems;
    private readonly List<Item> _majorItems;
    // private readonly List<Item> _dungeonMinorItems;
    private readonly List<Item> _minorItems;
    private readonly List<Item> _fillerItems;
    // private readonly List<Item> _niceItems;
    private Random? _rng;
    private bool _randomized = false;
    
    public int Seed { get; set; } = -1;

    public Shuffler()
    {
        Version = GetVersionName();

        _locations = new List<Location>();
        
        _music = new List<Item>();
        _unshuffledItems = new List<Item>();

        _dungeonEntrances = new List<Item>();
        _dungeonConstraints = new List<Item>();
        _overworldConstraints = new List<Item>();
        
        _dungeonPrizes = new List<Item>();
        _dungeonMajorItems = new List<Item>();
        _majorItems = new List<Item>();
        
        // _dungeonMinorItems = new List<Item>();
        _minorItems = new List<Item>();
        _fillerItems = new List<Item>();
        // _niceItems = new List<Item>();
        
        _logicParser = new Parser.Parser();
    }

    /// <summary>
    /// Throws a <code>ShufflerConfigurationException</code> if state is not valid with a message describing the
    /// validation failure
    /// </summary>
    public void ValidateState(bool checkIfRandomized = false)
    {
        Logger.Instance.LogInfo("Beginning Shuffler State Validation");
        
        if (Rom.Instance == null)
            throw new ShufflerConfigurationException("No ROM loaded! You must load a ROM before randomization.");
        
        if (Seed < 0)
            throw new ShufflerConfigurationException($"Supplied Seed is invalid! Seeds must be a number from 0 to {int.MaxValue}");
        
        if (checkIfRandomized && !_randomized)
            throw new ShufflerConfigurationException("You must randomize the ROM before saving the ROM or a patch file!");
        
        Logger.Instance.LogInfo("Shuffler State Validation Succeeded");
        Logger.Instance.SaveLogTransaction();
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
        if (_logicPath != null)
        {
            fallbackName = Path.GetFileNameWithoutExtension(_logicPath);
            fallbackVersion = File.GetLastWriteTime(_logicPath).ToShortDateString();
        }
        else
        {
            fallbackName = "Default";
            fallbackVersion = Version;
        }

        var name = _logicParser.SubParser.LogicName ?? fallbackName;
        var version = _logicParser.SubParser.LogicVersion ?? fallbackVersion;

        return name;
    }

    public void SetSeed(int seed)
    {
        Seed = seed;
        _rng = new Random(seed);
        Logger.Instance.LogInfo($"Randomization seed set to {seed}");
    }

    public List<LogicOptionBase> GetOptions()
    {
        return _logicParser.SubParser.Options;
    }

    public uint GetSettingHash()
    {
        var settingBytes = _logicParser.SubParser.GetSettingBytes();

        return settingBytes.Length > 0 ? settingBytes.Crc32() : 0;
    }

    public uint GetCosmeticsHash()
    {
        var cosmeticBytes = _logicParser.SubParser.GetCosmeticBytes();

        return cosmeticBytes.Length > 0 ? cosmeticBytes.Crc32() : 0;
    }

    public string GetOptionsIdentifier()
    {
        return StringUtil.AsStringHex8((int)GetSettingHash()) + "-" + StringUtil.AsStringHex8((int)GetCosmeticsHash());
    }

    /// <summary>
    ///     Load the flags that a logic file uses to customize itself
    /// </summary>
    public void LoadOptions(string? logicFile = null)
    {
        Logger.Instance.LogInfo("Loading Logic Options");
        _logicParser.SubParser.ClearOptions();

        string[] logicStrings;

        if (logicFile == null)
        {
            // Load default logic if no alternative is specified
            // var assembly = Assembly.GetExecutingAssembly();
            var assembly = Assembly.GetAssembly(typeof(Shuffler));
            using (var stream = assembly.GetManifestResourceStream("RandomizerCore.Resources.default.logic"))
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

        _logicParser.PreParse(logicStrings);
    }

    public bool RomCrcValid(Rom rom)
    {
        if (_logicParser.SubParser.RomCrc != null)
            return rom.romData.Crc32() == _logicParser.SubParser.RomCrc;
        return true;
    }

    /// <summary>
    ///     Reads the list of locations from a file, or the default logic if none is specified
    /// </summary>
    /// <param name="logicFile">The file to read locations from</param>
    public void LoadLocations(string? logicFile = null)
    {
        // Change the logic file path to match
        _logicPath = logicFile;

        // Reset everything to allow rerandomization
        ClearLogic();

        string[] locationStrings;

        // Get the logic file as an array of strings that can be parsed
        if (logicFile == null)
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

        var locationAndItems = _logicParser.ParseLocationsAndItems(locationStrings, _rng);

        _logicParser.SubParser.DuplicateAmountReplacements();
        _logicParser.SubParser.DuplicateIncrementalReplacements();
        locationAndItems.locations.ForEach(AddLocation);
        locationAndItems.items.ForEach(AddItem);
    }

    public void AddItem(Item item)
    {
        var newItem = CheckReplacements(item);

        if (newItem.HasValue) item = newItem.Value;
        
        switch (item.ShufflePool)
        {
            case ItemPool.Music:
                _music.Add(item);
                break;
            case ItemPool.Unshuffled:
                break;
            case ItemPool.DungeonEntrance:
                _dungeonEntrances.Add(item);
                break;
            case ItemPool.DungeonConstraint:
                _dungeonConstraints.Add(item);
                break;
            case ItemPool.OverworldConstraint:
                _overworldConstraints.Add(item);
                break;
            case ItemPool.DungeonPrize:
                _dungeonPrizes.Add(item);
                break;
            case ItemPool.DungeonMajor:
                _dungeonMajorItems.Add(item);
                break;
            case ItemPool.Major:
                _majorItems.Add(item);
                break;
            // case ItemPool.DungeonMinor:
            //     _dungeonMinorItems.Add(item);
            //     break;
            case ItemPool.Minor:
                _minorItems.Add(item);
                break;
            case ItemPool.Filler:
                _fillerItems.Add(item);
                break;
            default:
                _minorItems.Add(item);
                break;
        }
    }

    public void AddLocation(Location location)
    {
        // All locations are in the master location list
        _locations.Add(location);

        if (!location.Contents.HasValue) return;
        
        var newItem = CheckReplacements(location.Contents.Value);
        
        if (newItem.HasValue) location.SetItem(newItem.Value);


        if (_logicParser.SubParser.LocationTypeOverrides.ContainsKey(location.Contents.Value))
            location.Type = _logicParser.SubParser.LocationTypeOverrides[location.Contents.Value];

        if (location.Type != LocationType.Unshuffled) return;
        
        // Unshuffled locations are filled by default
        // Unshuffled locations require contents, so add them here
        location.Fill(location.Contents!.Value);
        _unshuffledItems.Add(location.Contents!.Value);

        // The type of the containing location determines how the item is handled
        // switch (location.Type)
        // {
        //     // These locations are not filled, because they don't reference an item location
        //     case LocationType.Untyped:
        //     case LocationType.Helper:
        //         break;
        //     // Minor locations are not logically accounted for
        //     case LocationType.Minor:
        //         _minorItems.Add(location.Contents);
        //         break;
        //     case LocationType.DungeonMinor:
        //         _dungeonMinorItems.Add(location.Contents);
        //         break;
        //     case LocationType.Music:
        //         _music.Add(location.Contents);
        //         break;
        //     // Dungeon items can only be placed within the same dungeon, and are placed first
        //     case LocationType.DungeonMajor:
        //         _dungeonMajorItems.Add(location.Contents);
        //         break;
        //     case LocationType.DungeonPrize:
        //         _dungeonPrizes.Add(location.Contents);
        //         break;
        //     case LocationType.DungeonConstraint:
        //         _dungeonConstraints.Add(location.Contents);
        //         break;
        //     // Nice items check logic but cannot affect it
        //     case LocationType.Nice:
        //         _niceItems.Add(location.Contents);
        //         break;
        //     // Major/etc items are fully randomized and check logic
        //     case LocationType.Major:
        //     default:
        //         _majorItems.Add(location.Contents);
        //         break;
        // }
    }

    private Item? CheckReplacements(Item item)
    {
        if (_logicParser.SubParser.IncrementalReplacements.ContainsKey(item))
        {
            var set = _logicParser.SubParser.IncrementalReplacements[item];
            var replacement = set[0];
            if (replacement.amount != 0)
            {
                replacement.amount -= 1;
                var newItem = new Item(replacement.item.Type,
                    (byte)((replacement.item.SubValue + replacement.amount) % 256), replacement.item.Dungeon);
                return newItem;
            }

            if (replacement.amount == 0)
            {
                set.RemoveAt(0);
                if (_logicParser.SubParser.IncrementalReplacements[item].Count == 0)
                {
                    _logicParser.SubParser.IncrementalReplacements.Remove(item);
                    Logger.Instance.LogInfo($"Removed incremental item, key {item.Type}");
                }
            }
        }

        if (_logicParser.SubParser.AmountReplacements.ContainsKey(item))
        {
            var set = _logicParser.SubParser.AmountReplacements[item];
            var replacement = set[0];
            if (replacement.amount != 0)
            {
                replacement.amount -= 1;
                return replacement.item;
            }

            if (replacement.amount == 0)
            {
                set.RemoveAt(0);
                if (_logicParser.SubParser.AmountReplacements[item].Count == 0)
                {
                    _logicParser.SubParser.AmountReplacements.Remove(item);
                    Console.WriteLine("removed key:" + item.Type);
                }
            }
        }

        if (_logicParser.SubParser.Replacements.ContainsKey(item))
        {
            var chanceSet = _logicParser.SubParser.Replacements[item];
            var number = _rng.Next(chanceSet.totalChance);
            var val = 0;

            for (var i = 0; i < chanceSet.randomItems.Count(); i++)
            {
                val += chanceSet.randomItems[i].chance;
                if (number < val)
                {
                    return chanceSet.randomItems[i].item;
                }
            }
        }

        return null;
    }

    public void ApplyPatch(string romLocation, string? patchFile = null)
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

        Program.Main(args);
    }

    public void ApplyPatch(Stream patchedRom, string? patchFile = null)
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

        Program.Main(args);
    }

    /// <summary>
    ///     Shuffles all locations, ensuring the game is beatable within the logic and all Major/Nice items are reachable.
    /// </summary>
    public void RandomizeLocations()
    {
        var locationGroups = _locations.GroupBy(location => location.Type).ToList();
        //We now do randomization in phases, following the ordering of items in <code>LocationType</code>
        
        //Make it so randomized music doesn't affect randomization
        var temp = _rng;
        _rng = new Random(Seed);
        FillLocations(_music, 
            locationGroups.First(group => group.Key == LocationType.Music).ToList(), null);
        
        _rng = temp;
        
        //Shuffle dungeon entrances
        FastFillLocations(_dungeonEntrances,
            locationGroups.First(group => group.Key == LocationType.DungeonEntrance).ToList());
        
        //Grab all items that we need to beat the seed
        var allItems = _majorItems.Concat(_dungeonMajorItems).Concat(_unshuffledItems).ToList();
        
        //Shuffle constraints
        FillLocations(_dungeonConstraints,
            locationGroups.First(group => group.Key == LocationType.DungeonConstraint).ToList(), allItems);
        FillLocations(_overworldConstraints,
            locationGroups.First(group => group.Key == LocationType.OverworldConstraint).ToList(), allItems);
        
        //Shuffle prizes
        var unfilledLocations = FillLocations(_dungeonPrizes,
            locationGroups.First(group => group.Key == LocationType.DungeonPrize).ToList(), allItems);
        
        //For dungeon majors, assume we have all majors and unshuffled items
        var allMajorsAndUnshuffled = _majorItems.Concat(_unshuffledItems).ToList();
        
        //Shuffle dungeon majors
        unfilledLocations.AddRange(FillLocations(_dungeonMajorItems,
            locationGroups.First(group => group.Key == LocationType.Dungeon).ToList(),
            allMajorsAndUnshuffled,
            unfilledLocations));
        
        //Now that all dungeon items are placed, we add all the rest of the locations to the pool of unfilled locations
        unfilledLocations.AddRange(locationGroups.First(group => group.Key == LocationType.Any));
        
        //Shuffle all other majors, do not assume any items are already obtained anymore
        unfilledLocations.AddRange(FillLocations(_majorItems,
            locationGroups.First(group => group.Key == LocationType.Major).ToList(),
            null,
            unfilledLocations));
        
        // var unfilledLocations = _locations.Where(location =>
        //     !location.Filled && 
        //     location.Type is not LocationType.Helper and not LocationType.Untyped and not LocationType.Music).ToList();
        //
        // unfilledLocations.Shuffle(_rng);
        // unplacedItems.Shuffle(_rng);
        //
        // //Fill out constraints and prizes before doing anything else
        // FillLocations(_dungeonConstraints.ToList(), 
        //     _locations.Where(location => location.Type == LocationType.DungeonConstraint).ToList(), allAssumedItems);
        // FillLocations(_dungeonPrizes.ToList(),
        //     _locations.Where(location => location.Type == LocationType.DungeonPrize).ToList(), allAssumedItems);
        //
        // // Fill dungeon items first so there is room for them all
        // FillLocations(dungeonSpecificItems, unfilledLocations, unplacedItems);
        //
        // // Fill non-dungeon major items, checking for logic
        // unfilledLocations.Shuffle(_rng);
        // FillLocations(unplacedItems, unfilledLocations);

        // Get every item that can be logically obtained, to check if the game can be completed
        var finalMajorItems = GetAvailableItems(new List<Item>());

        if (!new LocationDependency("BeatVaati").DependencyFulfilled(finalMajorItems, _locations))
            throw new ShuffleException("Randomization succeeded, but could not beat Vaati!");
        
        // FastFillLocations(_dungeonMinorItems.ToList());
        
        // Fill dungeon minor items, do not check logic
        // unfilledLocations.Shuffle(_rng);
        // FillLocations(_dungeonMinorItems.ToList(), unfilledLocations);
        //
        // // Put nice items in locations, logic is checked but not updated
        // unfilledLocations.Shuffle(_rng);
        // // CheckedFastFillLocations(_niceItems, unfilledLocations);
        //
        // // Put minor items in locations, not checking logic
        // unfilledLocations.Shuffle(_rng);
        FastFillLocations(_minorItems.ToList(), unfilledLocations);

        // Final cache clear
        _locations.ForEach(location => location.InvalidateCache());

        _randomized = true;
    }

    private List<Location> FillLocations(List<Item> allShuffledItems, List<Location> allPlaceableLocations)
    {
        var spheres = new List<Sphere>();

        var sphereNumber = 0;
        var obtainedItems = new List<Item>();

        var canBeatVaati = false;

        var locationsAvailableThisSphere =
            allPlaceableLocations.Where(location => location.IsAccessible(obtainedItems, _locations)).ToList();
        
        var shuffledLocationsThisSphere = locationsAvailableThisSphere
            .Where(location => location.Type is not LocationType.Unshuffled).ToList();
        var unshuffledLocationsThisSphere =
            locationsAvailableThisSphere.Where(location => location.Type is LocationType.Unshuffled).ToList();

        var sphere = new Sphere
        {
            Locations = locationsAvailableThisSphere,
            TotalShuffledLocations = shuffledLocationsThisSphere.Count,
            SphereNumber = sphereNumber,
        };

        var unshuffledItemsThisSphere =
            unshuffledLocationsThisSphere.Select(location => location.Contents!.Value).ToList();
        obtainedItems.AddRange(unshuffledItemsThisSphere);

        foreach (var location in locationsAvailableThisSphere)
        {
            allPlaceableLocations.Remove(location);
        }

        var maxRetries = shuffledLocationsThisSphere.Count == 0 ? 0 : allShuffledItems.Count / shuffledLocationsThisSphere.Count;
        var retryCount = 0;

        while (//!allShuffledItems.Any(item => item.ShufflePool is not ItemPool.Major and not ItemPool.DungeonMajor) ||
               !canBeatVaati)
        {
            shuffledLocationsThisSphere.Shuffle(_rng);

            var availableMajorItems = allShuffledItems.Where(item => item.ShufflePool is ItemPool.Major or ItemPool.DungeonMajor).ToList();
            var placedItemsThisSphere = new List<Item>();
            
            var filledLocationThisSphere = false;
            for (var i = 0; i < shuffledLocationsThisSphere.Count; )
            {
                var location = shuffledLocationsThisSphere[0];
                
                var item = filledLocationThisSphere
                    ? allShuffledItems[_rng.Next(allShuffledItems.Count)]
                    : availableMajorItems[_rng.Next(availableMajorItems.Count)];

                if (!location.CanPlace(item, obtainedItems, _locations))
                {
                    shuffledLocationsThisSphere.Shuffle(_rng);
                    continue;
                }
                
                location.SetItem(item);
                shuffledLocationsThisSphere.Remove(location);
                allShuffledItems.Remove(item);
                placedItemsThisSphere.Add(item);
                filledLocationThisSphere = true;
            }
            
            //The available items has changed, invalidate cache and re-run
            _locations.ForEach(location => location.InvalidateCache());
            var locationsAvailableNextSphere = allPlaceableLocations.Where(location => location.IsAccessible(obtainedItems.Concat(placedItemsThisSphere).ToList(), _locations)).ToList();

            if (locationsAvailableNextSphere.Count == 0)
            {
                retryCount++;
                if (retryCount > maxRetries)
                {
                    if (sphereNumber == 0)
                        throw new ShuffleException("Could not place any items in sphere 0 that advanced logic!");

                    sphereNumber--;

                    var lastSphere = spheres[sphereNumber];
                    allShuffledItems.AddRange(lastSphere.Items);
                    
                    foreach (var item in lastSphere.Items)
                        obtainedItems.Remove(item);

                    while (lastSphere.TotalShuffledLocations == 0)
                    {
                        spheres.Remove(lastSphere);
                        sphereNumber--;
                        allPlaceableLocations.AddRange(lastSphere.Locations);
                        if (sphereNumber < 0)
                            throw new ShuffleException("Could not place any items in sphere 0 that advanced logic!");
                        lastSphere = spheres[sphereNumber];
                        allShuffledItems.AddRange(lastSphere.Items);
                        
                        foreach (var item in lastSphere.Items)
                            obtainedItems.Remove(item);
                    }

                    locationsAvailableThisSphere = lastSphere.Locations;
        
                    shuffledLocationsThisSphere = locationsAvailableThisSphere
                        .Where(location => location.Type is not LocationType.Unshuffled).ToList();
                    unshuffledLocationsThisSphere =
                        locationsAvailableThisSphere.Where(location => location.Type is LocationType.Unshuffled).ToList();

                    sphere = new Sphere
                    {
                        Locations = locationsAvailableThisSphere,
                        TotalShuffledLocations = shuffledLocationsThisSphere.Count,
                        SphereNumber = sphereNumber,
                    };
                    
                    unshuffledItemsThisSphere =
                        unshuffledLocationsThisSphere.Select(location => location.Contents!.Value).ToList();
                    obtainedItems.AddRange(unshuffledItemsThisSphere);
                    
                    maxRetries = shuffledLocationsThisSphere.Count == 0 ? 0 : allShuffledItems.Count / shuffledLocationsThisSphere.Count;
                    retryCount = 0;
                    continue;
                }
                
                shuffledLocationsThisSphere = locationsAvailableThisSphere
                    .Where(location => location.Type is not LocationType.Unshuffled).ToList();

                allShuffledItems.AddRange(placedItemsThisSphere);
                placedItemsThisSphere = new List<Item>();
                
                continue;
            }

            sphereNumber++;

            obtainedItems.AddRange(placedItemsThisSphere);
            sphere.Items = placedItemsThisSphere.Concat(unshuffledItemsThisSphere).ToList();
            
            spheres.Add(sphere);

            canBeatVaati = new LocationDependency("BeatVaati").DependencyFulfilled(obtainedItems, _locations);

            locationsAvailableThisSphere = locationsAvailableNextSphere;
            
            shuffledLocationsThisSphere = locationsAvailableThisSphere
                .Where(location => location.Type is not LocationType.Unshuffled).ToList();
            unshuffledLocationsThisSphere =
                locationsAvailableThisSphere.Where(location => location.Type is LocationType.Unshuffled).ToList();

            sphere = new Sphere
            {
                Locations = locationsAvailableThisSphere,
                TotalShuffledLocations = shuffledLocationsThisSphere.Count,
                SphereNumber = sphereNumber,
            };
            
            unshuffledItemsThisSphere =
                unshuffledLocationsThisSphere.Select(location => location.Contents!.Value).ToList();
            obtainedItems.AddRange(unshuffledItemsThisSphere);

            foreach (var location in locationsAvailableThisSphere)
            {
                allPlaceableLocations.Remove(location);
            }

            maxRetries = shuffledLocationsThisSphere.Count == 0 ? 0 : allShuffledItems.Count / shuffledLocationsThisSphere.Count;
            retryCount = 0;

        }

        var remainingLocations = FillLocations(allShuffledItems, allPlaceableLocations, null);

        return remainingLocations;
    }

    /// <summary>
    ///     Uniformly fills items in locations, checking to make sure the items are logically available.
    /// </summary>
    /// <param name="items">The items to fill with</param>
    /// <param name="locations">The locations to be filled</param>
    /// <param name="assumedItems">The items that are available by default</param>
    /// <returns>A list of the locations that were filled</returns>
    private List<Location> FillLocations(List<Item> items, List<Location> locations, List<Item>? assumedItems = null, List<Location>? fallbackLocations = null)
    {
        var filledLocations = new List<Location>();

        if (fallbackLocations == null) fallbackLocations = new List<Location>();

        if (assumedItems == null) assumedItems = new List<Item>();

        var errorIndexes = new List<int>();
        for ( ; items.Count > 0; )
        {
            // Get a random item from the list and save its index
            var usingFallback = false;
            var itemIndex = _rng.Next(items.Count);
            while (errorIndexes.Contains(itemIndex))
                itemIndex = _rng.Next(items.Count);
            
            var item = items[itemIndex];

            // Take item out of pool
            items.RemoveAt(itemIndex);

            var availableItems = GetAvailableItems(items.Concat(assumedItems).ToList());

            // Find locations that are available for placing the item
            var availableLocations = locations.Where(location => location.CanPlace(item, availableItems, _locations))
                .ToList();

            if (availableLocations.Count == 0)
            {
                availableLocations = fallbackLocations
                    .Where(location => location.CanPlace(item, availableItems, _locations)).ToList();
                usingFallback = true;
            }

            if (availableLocations.Count <= 0)
            {
                errorIndexes.Add(itemIndex);
                Logger.Instance.LogInfo($"Error Count: {errorIndexes.Count}");
                Logger.Instance.LogInfo($"Could not place {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");
                items.Insert(itemIndex, item);
                if (errorIndexes.Count == items.Count)
                {
                    // The filler broke
                    throw new ShuffleException($"Could not place {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");
                }

                continue;
            }

            var locationIndex = _rng.Next(availableLocations.Count);

            availableLocations[locationIndex].Fill(item);
            Logger.Instance.LogInfo(
                $"Placed {item.Type.ToString()} subtype {StringUtil.AsStringHex2(item.SubValue)} at {availableLocations[locationIndex].Name} with {items.Count} items remaining");

            if (usingFallback) fallbackLocations.Remove(availableLocations[locationIndex]);
            else locations.Remove(availableLocations[locationIndex]);

            filledLocations.Add(availableLocations[locationIndex]);

            // Location caches are no longer valid because available items have changed
            _locations.ForEach(location => location.InvalidateCache());
            
            errorIndexes.Clear();
        }

        return locations.Concat(fallbackLocations).ToList();
    }

    /// <summary>
    ///     Fill items in locations that are available at the start of the fill.
    ///     Slower than FastFillLocations, but will not place in unavailable locations.
    /// </summary>
    /// <param name="items">The items to fill with</param>
    /// <param name="locations">The locations in which to fill the items</param>
    private void CheckedFastFillLocations(List<Item> items, List<Location> locations)
    {
        var finalMajorItems = GetAvailableItems(new List<Item>());
        var availableLocations =
            locations.Where(location => location.IsAccessible(finalMajorItems, _locations)).ToList();

        foreach (var item in items)
        {
            var locationIndex = _rng.Next(0, availableLocations.Count);
            availableLocations[locationIndex].Fill(item);
        }
    }

    /// <summary>
    ///     Fill items in locations without checking logic for speed
    /// </summary>
    /// <param name="items">The items to fill with</param>
    /// <param name="locations">The locations in which to fill the items</param>
    private void FastFillLocations(List<Item> items, List<Location> locations)
    {
        // Don't need to check logic, cause the items being placed do not affect logic
        foreach (var item in items)
        {
            if (locations.Count == 0) return;
            
            locations[0].Fill(item);
            locations.RemoveAt(0);
        }

        if (locations.Count == 0) return;
        
        var fillItems = items.Where(item => item.ShufflePool == ItemPool.Filler).ToList();
        var rand = new Random(Seed);
        while (locations.Count > 0)
        {
            locations[0].Fill(fillItems[rand.Next(items.Count)]);
            locations.RemoveAt(0);
        }
    }


    /// <summary>
    ///     Gets all the available items with a given item set, looping until there are no more items left to get
    /// </summary>
    /// <param name="preAvailableItems">Items that are available from the start</param>
    /// <returns>A list of all the items that are logically accessible</returns>
    private List<Item> GetAvailableItems(List<Item> preAvailableItems)
    {
        var availableItems = preAvailableItems.ToList();

        var filledLocations = _locations.Where(location =>
            location.Filled && location.Type != LocationType.Helper &&
            location.Type != LocationType.Untyped).ToList();

        int previousSize;

        // Get "spheres" until the next sphere contains no new items
        do
        {
            var accessibleLocations =
                filledLocations.Where(location => location.IsAccessible(availableItems, _locations)).ToList();
            previousSize = accessibleLocations.Count;

            filledLocations.RemoveAll(location => accessibleLocations.Contains(location));

            var newItems = Location.GetItems(accessibleLocations);

            availableItems.AddRange(newItems);

            // Cache is invalidated between each sphere to make sure things work out
            _locations.ForEach(location => location.InvalidateCache());
        } while (previousSize > 0);

        return availableItems;
    }

    /// <summary>
    ///     Get a byte[] of the randomized data
    /// </summary>
    /// <returns>The data of the randomized ROM</returns>
    public byte[] GetRandomizedRom()
    {
        // Create a copy of the ROM data to modify for output
        var outputBytes = new byte[Rom.Instance.romData.Length];
        Array.Copy(Rom.Instance.romData, 0, outputBytes, 0, outputBytes.Length);

        using (var ms = new MemoryStream(outputBytes))
        {
            var writer = new Writer(ms);
            foreach (var location in _locations) location.WriteLocation(writer);

            WriteElementPositions(writer);
        }

        return outputBytes;
    }

    /// <summary>
    ///     Get the contents of the spoiler log, including playthrough
    /// </summary>
    /// <returns>The contents of the spoiler log</returns>
    public string GetSpoiler()
    {
        var spoilerBuilder = new StringBuilder();
        spoilerBuilder.AppendLine("Spoiler for Minish Cap Randomizer");
        spoilerBuilder.AppendLine($"Seed: {Seed}");

        spoilerBuilder.AppendLine();
        AppendLocationSpoiler(spoilerBuilder);

        spoilerBuilder.AppendLine();
        AppendPlaythroughSpoiler(spoilerBuilder);


        return spoilerBuilder.ToString();
    }

    /// <summary>
    ///     Create list of filled locations and their contents
    /// </summary>
    /// <param name="spoilerBuilder">The running spoiler log builder to append the locations to</param>
    private void AppendLocationSpoiler(StringBuilder spoilerBuilder)
    {
        spoilerBuilder.AppendLine("Location Contents:");
        // Get the locations that have been filled
        var nonNullLocations = _locations.Where(location => location.Contents is not null);
        
        var filledLocations = nonNullLocations.Where(location => 
            location.Filled && location.Type is not LocationType.Helper and not LocationType.Untyped).ToList();

        var locationsWithRealItems = filledLocations.Where(location => location.Contents!.Value.Type is not ItemType.Untyped);

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
        spoilerBuilder.AppendLine("Playthrough:");

        var nonNullLocations = _locations.Where(location => location.Contents is not null);
        
        var filledLocations = nonNullLocations.Where(location =>
            location.Filled && location.Type != LocationType.Helper &&
            location.Type != LocationType.Untyped);

        var locationsWithRealItems = filledLocations.Where(location => location.Contents!.Value.Type is not ItemType.Untyped);

        var hackToFilterOutLanternGarbage = locationsWithRealItems.Where(location =>
            location.Contents!.Value.Type != ItemType.Lantern || location.Contents!.Value.SubValue == 0).ToList();

        var availableItems = new List<Item>();

        int previousSize;
        var sphereCounter = 1;

        do
        {
            var accessibleLocations =
                hackToFilterOutLanternGarbage.Where(location => location.IsAccessible(availableItems, _locations)).ToList();
            previousSize = accessibleLocations.Count;

            hackToFilterOutLanternGarbage.RemoveAll(location => accessibleLocations.Contains(location));

            var newItems = Location.GetItems(accessibleLocations);
            availableItems.AddRange(newItems);

            foreach (var location in accessibleLocations)
            {
                spoilerBuilder.AppendLine($"Sphere {sphereCounter}: {location.Contents!.Value.Type} in {location.Name}");

                AppendSubvalue(spoilerBuilder, location);
                spoilerBuilder.AppendLine();
            }

            sphereCounter++;
            spoilerBuilder.AppendLine();

            // Evaluating for different items, so cache is invalidated now
            _locations.ForEach(location => location.InvalidateCache());
        } while (previousSize > 0);
    }

    private void AppendSubvalue(StringBuilder spoilerBuilder, Location location)
    {
        if (!location.Contents.HasValue) return;
        
        // Display subvalue if relevant
        if (location.Contents.Value.Type == ItemType.Kinstone)
            spoilerBuilder.AppendLine($"Kinstone Type: {location.Contents.Value.Kinstone}");
        else if (location.Contents.Value.SubValue != 0) spoilerBuilder.AppendLine($"Subvalue: {location.Contents.Value.SubValue}"); //TODO: Display dungeon names

        // Display dungeon contents if relevant
        if (!string.IsNullOrEmpty(location.Contents.Value.Dungeon))
            spoilerBuilder.AppendLine($"Dungeon: {location.Contents.Value.Dungeon}");
    }

    public string GetEventWrites()
    {
        var eventBuilder = new StringBuilder();

        foreach (var location in _locations) location.WriteLocationEvent(eventBuilder);

        foreach (var define in _logicParser.GetEventDefines()) define.WriteDefineString(eventBuilder);

        var seedValues = new byte[4];
        seedValues[0] = (byte)((Seed >> 00) & 0xFF);
        seedValues[1] = (byte)((Seed >> 08) & 0xFF);
        seedValues[2] = (byte)((Seed >> 16) & 0xFF);
        seedValues[3] = (byte)((Seed >> 24) & 0xFF);

        eventBuilder.AppendLine("#define seedHashed 0x" + StringUtil.AsStringHex8((int)CrcUtil.Crc32(seedValues, 4)));
        eventBuilder.AppendLine("#define settingHash 0x" + StringUtil.AsStringHex8((int)GetSettingHash()));

        return eventBuilder.ToString();
    }

    /// <summary>
    ///     Move the elements around in a randomized ROM
    /// </summary>
    /// <param name="w">Writer to write with</param>
    private void WriteElementPositions(Writer w)
    {
        // Write coordinates for each element
        var earthLocation = _locations.First(loc => loc.Contents is not null && loc.Contents.Value.Type == ItemType.EarthElement);
        MoveElement(w, earthLocation);

        var fireLocation = _locations.First(loc => loc.Contents is not null && loc.Contents.Value.Type == ItemType.FireElement);
        MoveElement(w, fireLocation);

        var waterLocation = _locations.First(loc => loc.Contents is not null && loc.Contents.Value.Type == ItemType.WaterElement);
        MoveElement(w, waterLocation);

        var windLocation = _locations.First(loc => loc.Contents is not null && loc.Contents.Value.Type == ItemType.WindElement);
        MoveElement(w, windLocation);
    }

    /// <summary>
    ///     Moves a single element marker to the location that contains it
    /// </summary>
    /// <param name="w">The writer to write to</param>
    /// <param name="location">The location that contains the element</param>
    private void MoveElement(Writer w, Location location)
    {
        // Coordinates for the unzoomed map
        var largeCoords = new byte[2];

        // Coordinates for the zoomed in map
        var smallCoords = new ushort[2];
        switch (location.Name)
        {
            case "Deepwood_Prize":
                largeCoords[0] = 0xB2;
                largeCoords[1] = 0x7A;

                smallCoords[0] = 0x0D7D;
                smallCoords[1] = 0x0AC8;
                break;
            case "CoF_Prize":
                largeCoords[0] = 0x3B;
                largeCoords[1] = 0x1B;

                smallCoords[0] = 0x01E8;
                smallCoords[1] = 0x0178;
                break;
            case "Fortress_Prize":
                largeCoords[0] = 0x4B;
                largeCoords[1] = 0x77;

                smallCoords[0] = 0x0378;
                smallCoords[1] = 0x0A78;
                break;
            case "Droplets_Prize":
                largeCoords[0] = 0xB5;
                largeCoords[1] = 0x4B;

                smallCoords[0] = 0x0DB8;
                smallCoords[1] = 0x0638;
                break;
            case "Crypt_Prize":
                largeCoords[0] = 0x5A;
                largeCoords[1] = 0x15;

                smallCoords[0] = 0x04DC;
                smallCoords[1] = 0x0148;
                break;
            case "Palace_Prize":
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

        switch (location.Contents!.Value.Type)
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
        _locations.Clear();
        
        _music.Clear();
        _unshuffledItems.Clear();
        
        _dungeonEntrances.Clear();
        _dungeonConstraints.Clear();
        _overworldConstraints.Clear();
        
        _dungeonPrizes.Clear();
        _dungeonMajorItems.Clear();
        _majorItems.Clear();
        
        // _dungeonMinorItems.Clear();
        _minorItems.Clear();
        _fillerItems.Clear();
        // _niceItems.Clear();

        _logicParser.SubParser.ClearTypeOverrides();
        _logicParser.SubParser.ClearIncrementalReplacements();
        _logicParser.SubParser.ClearReplacements();
        _logicParser.SubParser.ClearAmountReplacements();
        _logicParser.SubParser.ClearDefines();
        _logicParser.SubParser.AddOptions();
    }
}