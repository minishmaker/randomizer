using System.Text;
using RandomizerCore.Random;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Logic.Dependency;
using RandomizerCore.Randomizer.Logic.Location;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.Extensions;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Models;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer.Shuffler;

internal class Shuffler : ShufflerBase
{

    public Shuffler()
    { }

    public override void LoadLocations(string? logicFile = null)
    {
        // Change the logic file path to match
        _logicPath = logicFile;

        // Reset everything to allow rerandomization
        ClearLogic();

        // Set option defines
        _logicParser.SubParser.AddOptions();

        var locationStrings = LoadLocationFile(logicFile);
        
        var time = DateTime.Now;
        var locationAndItems = LogicParser.ParseLocationsAndItems(locationStrings, Rng);

        _logicParser.SubParser.DuplicateAmountReplacements();
        _logicParser.SubParser.DuplicateIncrementalReplacements();

        var collectedLocations = locationAndItems.locations.Select(AddLocation).ToList();
        var collectedItems = locationAndItems.items.Concat(locationAndItems.locations.Where(loc => loc.Type is LocationType.Unshuffled && loc.Contents.HasValue).Select(loc => loc.Contents!.Value)).Select(AddItem).ToList();

        var distinctDeps = _logicParser.Dependencies.Distinct().ToList();

        foreach (var itemDep in distinctDeps.Where(dep => dep.GetType() == typeof(ItemDependency)))
            itemDep.ExpandRequiredDependencies(collectedLocations, collectedItems);
        foreach (var countDep in distinctDeps.Where(dep => dep.GetType() == typeof(CounterDependency)))
            countDep.ExpandRequiredDependencies(collectedLocations, collectedItems);
        foreach (var locDep in distinctDeps.Where(dep => dep.GetType() == typeof(LocationDependency)))
            locDep.ExpandRequiredDependencies(collectedLocations, collectedItems);
        foreach (var itemDep in distinctDeps.Where(dep => dep.GetType() == typeof(NotItemDependency)))
            itemDep.ExpandRequiredDependencies(collectedLocations, collectedItems);

        var diff = DateTime.Now - time;
        Logger.Instance.BeginLogTransaction();
        Logger.Instance.LogInfo($"Timing Benchmark - Parsing logic file took {diff.Seconds}.{diff.Milliseconds} seconds!");
        Logger.Instance.SaveLogTransaction(true);
    }

    public override void LoadLocationsYaml(string? logicFile = null, string? yamlFileLogic = null, string? yamlFileCosmetics = null,
        bool useGlobalYAML = false) => throw new NotImplementedException();

    /// <summary>
    ///     Shuffles all locations, ensuring the game is beatable within the logic and all Major/Nice items are reachable.
    /// </summary>
    public override void RandomizeLocations()
    {
        var time = DateTime.Now;

        var groupsAndLocations = PlaceImportantItems();

        var locationGroups = groupsAndLocations.locationGroups;
        var unfilledLocations = groupsAndLocations.unfilledLocations;
        
        //Now that all dungeon items are placed, we add all the rest of the locations to the pool of unfilled locations
        var nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Any)
            ? locationGroups.First(group => group.Key == LocationType.Any).ToList()
            : new List<Location>();
        unfilledLocations.AddRange(nextLocationGroup);
        unfilledLocations = unfilledLocations.Distinct().ToList();

        //Shuffle all other majors, do not assume any items are already obtained anymore
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Major)
            ? locationGroups.First(group => group.Key == LocationType.Major).ToList()
            : new List<Location>();
        unfilledLocations.AddRange(FillLocationsUniform(_majorItems,
            nextLocationGroup,
            null,
            unfilledLocations));
        unfilledLocations = unfilledLocations.Distinct().ToList();

        // Put minor and filler items in remaining locations locations, not checking logic
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Minor)
            ? locationGroups.First(group => group.Key == LocationType.Minor).ToList()
            : new List<Location>();
        unfilledLocations.AddRange(nextLocationGroup);
        unfilledLocations = unfilledLocations.Distinct().ToList();
        unfilledLocations.Shuffle(_rng);
        FastFillLocations(_minorItems.Concat(_fillerItems).ToList(), unfilledLocations);
        
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Inaccessible)
            ? locationGroups.First(group => group.Key == LocationType.Inaccessible).ToList()
            : new List<Location>();
        unfilledLocations.AddRange(nextLocationGroup);
        unfilledLocations = unfilledLocations.Distinct().ToList();
        unfilledLocations.Shuffle(_rng);
        FastFillLocations(_fillerItems.ToList(), unfilledLocations);

        // Get every item that can be logically obtained, to check if the game can be completed
        //var finalMajorItems = GetAvailableItems(new List<Item>());
        var finalFilledLocations = UpdateObtainedItemsFromPlacedLocations();

        if (!DependencyBase.BeatVaatiDependency!.DependencyFulfilled())
            throw new ShuffleException("Randomization succeeded, but could not beat Vaati!");
        
        finalFilledLocations.ForEach(location => location.Contents!.Value.NotifyParentDependencies(false));
        _filledLocations.AddRange(finalFilledLocations);

        var diff = DateTime.Now - time;
        Logger.Instance.BeginLogTransaction();
        Logger.Instance.LogInfo($"Timing Benchmark - Shuffling with seed {Seed:X} and settings {MinifiedSettings.GenerateSettingsString(GetSortedSettings(), GetLogicOptionsCrc32())} took {diff.Seconds}.{diff.Milliseconds} seconds!");
        Logger.Instance.SaveLogTransaction(true);
        
        _randomized = true;
    }

    protected (List<IGrouping<LocationType, Location>> locationGroups, List<Location> unfilledLocations) PlaceImportantItems()
    {
                var locationGroups = _locations.GroupBy(location => location.Type).ToList();
        //We now do randomization in phases, following the ordering of items in <code>LocationType</code>
        //Make it so randomized music doesn't affect randomization
        var temp = _rng;
        _rng = new SquaresRandomNumberGenerator(SquaresRandomNumberGenerator.DefaultKey, Seed);

        var nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Music)
            ? locationGroups.First(group => group.Key == LocationType.Music).ToList()
            : new List<Location>();

        nextLocationGroup.Shuffle(_rng);
        FastFillLocations(_music, nextLocationGroup);

        _rng = temp;

        _filledLocations = new List<Location>();

        var majorsAndEntrances = _majorItems.Concat(_dungeonEntrances).ToList();

        //Shuffle dungeon entrances
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.DungeonEntrance)
            ? locationGroups.First(group => group.Key == LocationType.DungeonEntrance).ToList()
            : new List<Location>();
        nextLocationGroup.Shuffle(_rng);
        FastFillLocations(_dungeonEntrances, nextLocationGroup);

        //Add all unfilled items to the available pool
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Unshuffled)
            ? locationGroups.First(group => group.Key == LocationType.Unshuffled).ToList()
            : new List<Location>();
        _filledLocations.AddRange(nextLocationGroup);

        //Grab all items that we need to beat the seed
        var allItems = _majorItems.Concat(_dungeonMajorItems).ToList();
        var allItemsAndEntrances = _majorItems.Concat(_dungeonMajorItems).Concat(_dungeonEntrances).ToList();

        //Like entrances, constraints shouldn't check logic when placing
        //Shuffle constraints
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.DungeonConstraint)
            ? locationGroups.First(group => group.Key == LocationType.DungeonConstraint).ToList()
            : new List<Location>();
        nextLocationGroup.Shuffle(_rng);
        FastFillAndConsiderItemPlaced(_dungeonConstraints, nextLocationGroup);
        
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.OverworldConstraint)
            ? locationGroups.First(group => group.Key == LocationType.OverworldConstraint).ToList()
            : new List<Location>();
        nextLocationGroup.Shuffle(_rng);
        FastFillAndConsiderItemPlaced(_overworldConstraints, nextLocationGroup);

        // //Shuffle prizes
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.DungeonPrize)
            ? locationGroups.First(group => group.Key == LocationType.DungeonPrize).ToList()
            : new List<Location>();
        var unfilledLocations = FillLocationsFrontToBack(_dungeonPrizes, nextLocationGroup, allItems);

        //Shuffle dungeon majors
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Dungeon)
            ? locationGroups.First(group => group.Key == LocationType.Dungeon).ToList()
            : new List<Location>();
        unfilledLocations.AddRange(FillLocationsFrontToBack(_dungeonMajorItems,
            nextLocationGroup,
            majorsAndEntrances,
            unfilledLocations));

        //Shuffle dungeon minors
        unfilledLocations.AddRange(FillLocationsFrontToBack(_dungeonMinorItems,
            unfilledLocations,
            allItemsAndEntrances));

        unfilledLocations = unfilledLocations.Distinct().ToList();

        return (locationGroups, unfilledLocations);
    }

    /// <summary>
    ///     Fills in locations from front to back and makes sure they are accessible, meant to be used for dungeon items or for
    ///     linear progression seeds. Adding an item to the pool does not invalidate the available items
    /// </summary>
    /// <param name="items">The items to fill with</param>
    /// <param name="locations">The locations to be filled</param>
    /// <param name="assumedItems">The items that are available by default</param>
    /// <returns>A list of the locations that were filled</returns>
    private List<Location> FillLocationsFrontToBack(List<Item> items, List<Location> locations,
        List<Item>? assumedItems = null, List<Location>? fallbackLocations = null)
    {
        var filledLocations = new List<Location>();

        if (fallbackLocations == null) fallbackLocations = new List<Location>();

        if (assumedItems == null) assumedItems = new List<Item>();

        assumedItems.ForEach(item => item.NotifyParentDependencies(true));

        var errorIndexes = new List<int>();
        for (; items.Count > 0;)
        {
            // Get a random item from the list and save its index
            var usingFallback = false;
            var itemIndex = _rng.Next(items.Count);
            while (errorIndexes.Contains(itemIndex))
                itemIndex = _rng.Next(items.Count);

            var item = items[itemIndex];

            // Take item out of pool
            items.RemoveAt(itemIndex);
            
            filledLocations.AddRange(UpdateObtainedItemsFromPlacedLocations());

            // Find locations that are available for placing the item
            var availableLocations = locations.Where(location => location.CanPlace(item, _locations))
                .ToList();

            if (availableLocations.Count == 0)
            {
                availableLocations = fallbackLocations
                    .Where(location => location.CanPlace(item, _locations)).ToList();
                usingFallback = true;
            }

            if (availableLocations.Count <= 0)
            {
                errorIndexes.Add(itemIndex);
                Logger.Instance.LogInfo($"Error Count: {errorIndexes.Count}");
                Logger.Instance.LogInfo(
                    $"Could not place {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");
                items.Insert(itemIndex, item);
                if (errorIndexes.Count == items.Count)
                    // The filler broke
                    throw new ShuffleException(
                        $"Could not place {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");

                continue;
            }

            var locationIndex = _rng.Next(availableLocations.Count);

            availableLocations[locationIndex].Fill(item);
            Logger.Instance.LogInfo(
                $"Placed {item.Type} subtype {StringUtil.AsStringHex2(item.SubValue)} at {availableLocations[locationIndex].Name} with {items.Count} items remaining");

            if (usingFallback) fallbackLocations.Remove(availableLocations[locationIndex]);
            else locations.Remove(availableLocations[locationIndex]);
            
            item.NotifyParentDependencies(true);

            filledLocations.Add(availableLocations[locationIndex]);

            errorIndexes.Clear();
        }

        assumedItems.ForEach(item => item.NotifyParentDependencies(false));
        
        _filledLocations.AddRange(filledLocations);
        filledLocations.ForEach(location => location.Contents!.Value.NotifyParentDependencies(false));

        return locations.Concat(fallbackLocations).ToList();
    }

    /// <summary>
    ///     Uniformly fills items in locations, checking to make sure the items are logically available. Adding an item to the pool invalidates items that were previously available
    /// </summary>
    /// <param name="items">The items to fill with</param>
    /// <param name="locations">The locations to be filled</param>
    /// <param name="assumedItems">The items that are available by default</param>
    /// <returns>A list of the locations that were filled</returns>
    private List<Location> FillLocationsUniform(List<Item> items, List<Location> locations,
        List<Item>? assumedItems = null, List<Location>? fallbackLocations = null)
    {
        var filledLocations = new List<Location>();

        if (fallbackLocations == null) fallbackLocations = new List<Location>();

        if (assumedItems == null) assumedItems = new List<Item>();

        assumedItems.ForEach(item => item.NotifyParentDependencies(true));

        foreach (var item in items)
            item.NotifyParentDependencies(true);

        var errorIndexes = new List<int>();
        for (; items.Count > 0;)
        {
            // Get a random item from the list and save its index
            var usingFallback = false;
            var itemIndex = _rng.Next(items.Count);
            while (errorIndexes.Contains(itemIndex))
                itemIndex = _rng.Next(items.Count);

            var item = items[itemIndex];

            // Take item out of pool
            items.RemoveAt(itemIndex);
            item.NotifyParentDependencies(false);
            
            var tempFilledLocations = UpdateObtainedItemsFromPlacedLocations();

            // Find locations that are available for placing the item
            var availableLocations = locations.Where(location => location.CanPlace(item, _locations))
                .ToList();

            if (availableLocations.Count == 0)
            {
                availableLocations = fallbackLocations
                    .Where(location => location.CanPlace(item, _locations)).ToList();
                usingFallback = true;
            }

            if (availableLocations.Count <= 0)
            {
                errorIndexes.Add(itemIndex);
                Logger.Instance.LogInfo($"Error Count: {errorIndexes.Count}");
                Logger.Instance.LogInfo(
                    $"Could not place {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");
                items.Insert(itemIndex, item);
                item.NotifyParentDependencies(true);
                if (errorIndexes.Count == items.Count)
                    // The filler broke
                    throw new ShuffleException(
                        $"Could not place {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");

                continue;
            }

            var locationIndex = _rng.Next(availableLocations.Count);

            availableLocations[locationIndex].Fill(item);
            Logger.Instance.LogInfo(
                $"Placed {item.Type.ToString()} subtype {StringUtil.AsStringHex2(item.SubValue)} at {availableLocations[locationIndex].Name} with {items.Count} items remaining");

            if (usingFallback) fallbackLocations.Remove(availableLocations[locationIndex]);
            else locations.Remove(availableLocations[locationIndex]);
            
            _filledLocations.Add(availableLocations[locationIndex]);
            _filledLocations.AddRange(tempFilledLocations);
            tempFilledLocations.ForEach(location => location.Contents!.Value.NotifyParentDependencies(false));

            errorIndexes.Clear();
        }

        assumedItems.ForEach(item => item.NotifyParentDependencies(false));
        
        _filledLocations.AddRange(filledLocations);
        filledLocations.ForEach(location => location.Contents!.Value.NotifyParentDependencies(false));

        return locations.Concat(fallbackLocations).ToList();
    }

    public override string GetEventWrites()
    {
        var eventBuilder = new StringBuilder();

        foreach (var location in _locations) location.WriteLocationEvent(eventBuilder);

        foreach (var define in _logicParser.GetEventDefines()) define.WriteDefineString(eventBuilder);

        var seedValues = new byte[8];
        seedValues[0] = (byte)((Seed >> 00) & 0xFF);
        seedValues[1] = (byte)((Seed >> 08) & 0xFF);
        seedValues[2] = (byte)((Seed >> 16) & 0xFF);
        seedValues[3] = (byte)((Seed >> 24) & 0xFF);
        seedValues[4] = (byte)((Seed >> 32) & 0xFF);
        seedValues[5] = (byte)((Seed >> 40) & 0xFF);
        seedValues[6] = (byte)((Seed >> 48) & 0xFF);
        seedValues[7] = (byte)((Seed >> 56) & 0xFF);
        var crc = (int)CrcUtil.Crc32(seedValues, 8);

        eventBuilder.AppendLine("#define seedHashed 0x" + StringUtil.AsStringHex8(crc));
        eventBuilder.AppendLine("#define settingHash 0x" + StringUtil.AsStringHex8((int)GetFinalOptions().OnlyLogic().GetHash()));

        return eventBuilder.ToString();
    }
}
