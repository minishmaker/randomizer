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
    private readonly List<Item> _dungeonMajorItems;
    private readonly List<Item> _dungeonMinorItems;
    private readonly List<Item> _dungeonConstraints;
    private readonly List<Item> _dungeonPrizes;
    private readonly List<Item> _majorItems;
    private readonly List<Item> _minorItems;
    private readonly List<Item> _niceItems;
    private readonly List<Item> _unshuffledItems;
    private readonly List<Item> _music;
    private Random? _rng;
    private bool _randomized = false;
    
    public int Seed { get; set; } = -1;

    public Shuffler()
    {
        Version = GetVersionName();

        _locations = new List<Location>();
        _dungeonMajorItems = new List<Item>();
        _dungeonMinorItems = new List<Item>();
        _dungeonConstraints = new List<Item>();
        _dungeonPrizes = new List<Item>();
        _majorItems = new List<Item>();
        _niceItems = new List<Item>();
        _minorItems = new List<Item>();
        _unshuffledItems = new List<Item>();
        _music = new List<Item>();
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

        var parsedLocations = _logicParser.ParseLocations(locationStrings, _rng);

        _logicParser.SubParser.DuplicateAmountReplacements();
        _logicParser.SubParser.DuplicateIncrementalReplacements();
        parsedLocations.ForEach(location => { AddLocation(location); });
    }

    public void AddLocation(Location location)
    {
        var replaced = false;
        if (_logicParser.SubParser.IncrementalReplacements.ContainsKey(location.Contents))
        {
            var key = location.Contents;
            var set = _logicParser.SubParser.IncrementalReplacements[key];
            var replacement = set[0];
            if (replacement.amount != 0)
            {
                replacement.amount -= 1;
                var newItem = new Item(replacement.item.Type,
                    (byte)((replacement.item.SubValue + replacement.amount) % 256), replacement.item.Dungeon);
                location.SetItem(newItem);
                replaced = true;
            }

            if (replacement.amount == 0)
            {
                set.RemoveAt(0);
                if (_logicParser.SubParser.IncrementalReplacements[key].Count == 0)
                {
                    _logicParser.SubParser.IncrementalReplacements.Remove(key);
                    Logger.Instance.LogInfo($"Removed incremental item, key {key.Type}");
                }
            }
        }

        if (replaced == false && _logicParser.SubParser.AmountReplacements.ContainsKey(location.Contents))
        {
            var key = location.Contents;
            var set = _logicParser.SubParser.AmountReplacements[key];
            var replacement = set[0];
            if (replacement.amount != 0)
            {
                replacement.amount -= 1;
                location.SetItem(replacement.item);
                replaced = true;
            }

            if (replacement.amount == 0)
            {
                set.RemoveAt(0);
                if (_logicParser.SubParser.AmountReplacements[key].Count == 0)
                {
                    _logicParser.SubParser.AmountReplacements.Remove(key);
                    Console.WriteLine("removed key:" + key.Type);
                }
            }
        }

        if (replaced == false && _logicParser.SubParser.Replacements.ContainsKey(location.Contents))
        {
            var chanceSet = _logicParser.SubParser.Replacements[location.Contents];
            var number = _rng.Next(chanceSet.totalChance);
            var val = 0;

            for (var i = 0; i < chanceSet.randomItems.Count(); i++)
            {
                val += chanceSet.randomItems[i].chance;
                if (number < val)
                {
                    location.SetItem(chanceSet.randomItems[i].item);
                    break;
                }
            }
        }

        if (_logicParser.SubParser.LocationTypeOverrides.ContainsKey(location.Contents))
            location.Type = _logicParser.SubParser.LocationTypeOverrides[location.Contents];

        // All locations are in the master location list
        _locations.Add(location);

        // The type of the containing location determines how the item is handled
        switch (location.Type)
        {
            // These locations are not filled, because they don't reference an item location
            case LocationType.Untyped:
            case LocationType.Helper:
                break;
            // Unshuffled locations are filled by default
            case LocationType.Unshuffled:
                location.Fill(location.Contents);
                _unshuffledItems.Add(location.Contents);
                break;
            // Minor locations are not logically accounted for
            case LocationType.Minor:
                _minorItems.Add(location.Contents);
                break;
            case LocationType.DungeonMinor:
                _dungeonMinorItems.Add(location.Contents);
                break;
            case LocationType.Music:
                _music.Add(location.Contents);
                break;
            // Dungeon items can only be placed within the same dungeon, and are placed first
            case LocationType.DungeonMajor:
                _dungeonMajorItems.Add(location.Contents);
                break;
            case LocationType.DungeonPrize:
                _dungeonPrizes.Add(location.Contents);
                break;
            case LocationType.DungeonConstraint:
                _dungeonConstraints.Add(location.Contents);
                break;
            // Nice items check logic but cannot affect it
            case LocationType.Nice:
                _niceItems.Add(location.Contents);
                break;
            // Major/etc items are fully randomized and check logic
            case LocationType.Major:
            default:
                _majorItems.Add(location.Contents);
                break;
        }
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
        var unplacedItems = _majorItems.ToList();
        var dungeonSpecificItems = _dungeonMajorItems.ToList();
        var allAssumedItems = unplacedItems.Concat(dungeonSpecificItems).Concat(_unshuffledItems).ToList();
        
        //Place music since it doesn't actually affect anything
        var temp = _rng;
        _rng = new Random(Seed);
        FillLocations(_music.ToList(), _locations.Where(location => location.Type == LocationType.Music).ToList());
        _rng = temp;

        var unfilledLocations = _locations.Where(location =>
            !location.Filled && 
            location.Type is not LocationType.Helper and not LocationType.Untyped and not LocationType.Music).ToList();
        unfilledLocations.Shuffle(_rng);
        unplacedItems.Shuffle(_rng);

        //Fill out constraints and prizes before doing anything else
        FillLocations(_dungeonConstraints.ToList(), unfilledLocations, allAssumedItems);
        FillLocations(_dungeonPrizes.ToList(), unfilledLocations, allAssumedItems);

        // Fill dungeon items first so there is room for them all
        FillLocations(dungeonSpecificItems, unfilledLocations, unplacedItems);

        // Fill non-dungeon major items, checking for logic
        unfilledLocations.Shuffle(_rng);
        FillLocations(unplacedItems, unfilledLocations);

        // Get every item that can be logically obtained, to check if the game can be completed
        var finalMajorItems = GetAvailableItems(new List<Item>());

        if (!new LocationDependency("BeatVaati").DependencyFulfilled(finalMajorItems, _locations))
            throw new ShuffleException("Randomization succeeded, but could not beat Vaati!");
        
        // Fill dungeon minor items, do not check logic
        unfilledLocations.Shuffle(_rng);
        FillLocations(_dungeonMinorItems.ToList(), unfilledLocations);

        // Put nice items in locations, logic is checked but not updated
        unfilledLocations.Shuffle(_rng);
        CheckedFastFillLocations(_niceItems, unfilledLocations);

        // Put minor items in locations, not checking logic
        unfilledLocations.Shuffle(_rng);
        FastFillLocations(_minorItems.ToList(), unfilledLocations);

        if (unfilledLocations.Count != 0)
            // All locations should be filled at this point
            throw new ShuffleException($"There are {unfilledLocations.Count} unfilled locations!");

        // Final cache clear
        _locations.ForEach(location => location.InvalidateCache());

        _randomized = true;
    }

    /// <summary>
    ///     Uniformly fills items in locations, checking to make sure the items are logically available.
    /// </summary>
    /// <param name="items">The items to fill with</param>
    /// <param name="locations">The locations to be filled</param>
    /// <param name="assumedItems">The items that are available by default</param>
    /// <returns>A list of the locations that were filled</returns>
    private List<Location> FillLocations(List<Item> items, List<Location> locations, List<Item>? assumedItems = null)
    {
        var filledLocations = new List<Location>();

        if (assumedItems == null) assumedItems = new List<Item>();

        var errorIndexes = new List<int>();
        for ( ; items.Count > 0; )
        {
            // Get a random item from the list and save its index
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

            locations.Remove(availableLocations[locationIndex]);

            filledLocations.Add(availableLocations[locationIndex]);

            // Location caches are no longer valid because available items have changed
            _locations.ForEach(location => location.InvalidateCache());
            
            errorIndexes.Clear();
        }

        return filledLocations;
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
            locations[0].Fill(item);
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
        var filledLocations = _locations.Where(location =>
            location.Filled && location.Type is not LocationType.Helper and not LocationType.Untyped).ToList();

        var locationsWithRealItems = filledLocations.Where(location => location.Contents.Type is not ItemType.Untyped);

        var hackToFilterOutLanternGarbage = locationsWithRealItems.Where(location =>
            location.Contents.Type != ItemType.LanternOff || location.Contents.SubValue == 0);
        
        foreach (var location in hackToFilterOutLanternGarbage)
        {
            spoilerBuilder.AppendLine($"{location.Name}: {location.Contents.Type}");

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

        var filledLocations = _locations.Where(location =>
            location.Filled && location.Type != LocationType.Helper &&
            location.Type != LocationType.Untyped);

        var locationsWithRealItems = filledLocations.Where(location => location.Contents.Type is not ItemType.Untyped);

        var hackToFilterOutLanternGarbage = locationsWithRealItems.Where(location =>
            location.Contents.Type != ItemType.LanternOff || location.Contents.SubValue == 0).ToList();

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
                spoilerBuilder.AppendLine($"Sphere {sphereCounter}: {location.Contents.Type} in {location.Name}");

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
        // Display subvalue if relevant
        if (location.Contents.Type == ItemType.KinstoneX)
            spoilerBuilder.AppendLine($"Kinstone Type: {location.Contents.Kinstone}");
        else if (location.Contents.SubValue != 0) spoilerBuilder.AppendLine($"Subvalue: {location.Contents.SubValue}");

        // Display dungeon contents if relevant
        if (!string.IsNullOrEmpty(location.Contents.Dungeon))
            spoilerBuilder.AppendLine($"Dungeon: {location.Contents.Dungeon}");
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
        var earthLocation = _locations.Where(loc => loc.Contents.Type == ItemType.EarthElement).First();
        MoveElement(w, earthLocation);

        var fireLocation = _locations.Where(loc => loc.Contents.Type == ItemType.FireElement).First();
        MoveElement(w, fireLocation);

        var waterLocation = _locations.Where(loc => loc.Contents.Type == ItemType.WaterElement).First();
        MoveElement(w, waterLocation);

        var windLocation = _locations.Where(loc => loc.Contents.Type == ItemType.WindElement).First();
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
        _locations.Clear();
        _dungeonMajorItems.Clear();
        _dungeonMinorItems.Clear();
        _dungeonConstraints.Clear();
        _unshuffledItems.Clear();
        _dungeonPrizes.Clear();
        _majorItems.Clear();
        _niceItems.Clear();
        _minorItems.Clear();
        _music.Clear();

        _logicParser.SubParser.ClearTypeOverrides();
        _logicParser.SubParser.ClearIncrementalReplacements();
        _logicParser.SubParser.ClearReplacements();
        _logicParser.SubParser.ClearAmountReplacements();
        _logicParser.SubParser.ClearDefines();
        _logicParser.SubParser.AddOptions();
    }
}