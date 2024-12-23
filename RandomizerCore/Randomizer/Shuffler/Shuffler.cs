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
        LogicPath = logicFile;

        // Reset everything to allow rerandomization
        ClearLogic();

        // Set option defines
        LogicParser.SubParser.AddOptions();

        var locationStrings = LoadLocationFile(logicFile);
        
        var time = DateTime.Now;
        Logger.Instance.BeginLogTransaction();
        var locationAndItems = LogicParser.ParseLocationsAndItems(locationStrings, Rng);
        Logger.Instance.SaveLogTransaction();

        LogicParser.SubParser.DuplicateAmountReplacements();
        LogicParser.SubParser.DuplicateIncrementalReplacements();

        var collectedLocations = locationAndItems.locations.Select(AddLocation).ToList();
        var collectedItems = locationAndItems.items.Concat(locationAndItems.locations.Where(loc => loc.Type is LocationType.Unshuffled && loc.Contents.HasValue).Select(loc => loc.Contents!.Value)).Select(AddItem).ToList();

        var distinctDeps = LogicParser.Dependencies.Distinct().ToList();

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
        unfilledLocations.AddRange(FillLocationsUniform(MajorItems,
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
        unfilledLocations.Shuffle(Rng);
        Logger.Instance.LogInfo($"Placing Minors");
        FastFillLocations(MinorItems.Concat(FillerItems).ToList(), unfilledLocations);
        
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Inaccessible)
            ? locationGroups.First(group => group.Key == LocationType.Inaccessible).ToList()
            : new List<Location>();
        unfilledLocations.AddRange(nextLocationGroup);
        unfilledLocations = unfilledLocations.Distinct().ToList();
        unfilledLocations.Shuffle(Rng);
        Logger.Instance.LogInfo($"Filling all remaining locations");
        FastFillLocations(FillerItems.ToList(), unfilledLocations);

        // Get every item that can be logically obtained, to check if the game can be completed
        //var finalMajorItems = GetAvailableItems(new List<Item>());
        var finalFilledLocations = UpdateObtainedItemsFromPlacedLocations();

        if (Location.ShufflerConstraints.Any() && !DependencyBase.BeatVaatiDependency!.DependencyFulfilled())
            throw new ShuffleException("Randomization succeeded, but could not beat Vaati!");
        
        finalFilledLocations.ForEach(location => location.Contents!.Value.NotifyParentDependencies(false));
        FilledLocations.AddRange(finalFilledLocations);

        var diff = DateTime.Now - time;
        Logger.Instance.SaveLogTransaction();
        Logger.Instance.LogInfo($"Timing Benchmark - Shuffling with seed {Seed:X} and settings {MinifiedSettings.GenerateSettingsString(GetSortedSettings(), GetLogicOptionsCrc32())} took {diff.Seconds}.{diff.Milliseconds} seconds!");
        Logger.Instance.SaveLogTransaction(true);
        
        Randomized = true;
    }

    protected (List<IGrouping<LocationType, Location>> locationGroups, List<Location> unfilledLocations) PlaceImportantItems()
    {
        var locationGroups = Locations.GroupBy(location => location.Type).ToList();
        //We now do randomization in phases, following the ordering of items in <code>LocationType</code>
        //Make it so randomized music doesn't affect randomization
        var temp = Rng;
        Rng = new SquaresRandomNumberGenerator(SquaresRandomNumberGenerator.DefaultKey, Seed);

        var nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Music)
            ? locationGroups.First(group => group.Key == LocationType.Music).ToList()
            : new List<Location>();

        nextLocationGroup.Shuffle(Rng);
        Logger.Instance.LogInfo("Placing Music");
        FastFillLocations(Music, nextLocationGroup);

        Rng = temp;

        FilledLocations = new List<Location>();

        var majorsAndEntrances = MajorItems.Concat(DungeonEntrances).ToList();
        var majorsAndPrizesAndDungeon = MajorItems.Concat(DungeonPrizes).Concat(DungeonMajorItems).Concat(UnshuffledItems).ToList();

        //Shuffle dungeon entrances
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.DungeonEntrance)
            ? locationGroups.First(group => group.Key == LocationType.DungeonEntrance).ToList()
            : new List<Location>();
        nextLocationGroup.Shuffle(Rng);
        Logger.Instance.LogInfo("Placing Entrances");
        if (DungeonEntrances.Any(ent => ent.SubValue is 7 or 8))
        {
            var dhc = DungeonEntrances.First(ent => ent.SubValue is 7 or 8);
            DungeonEntrances.Remove(dhc);
            var extraEntrances = FillLocationsFrontToBack(new List<Item> { dhc }, nextLocationGroup, majorsAndPrizesAndDungeon);
            FillLocationsFrontToBack(DungeonEntrances, extraEntrances, majorsAndPrizesAndDungeon);
        }
        else
            FillLocationsFrontToBack(DungeonEntrances, nextLocationGroup, majorsAndPrizesAndDungeon);

        //Add all unfilled items to the available pool
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Unshuffled)
            ? locationGroups.First(group => group.Key == LocationType.Unshuffled).ToList()
            : new List<Location>();
        FilledLocations.AddRange(nextLocationGroup);

        //Grab all items that we need to beat the seed
        var allItems = MajorItems.Concat(DungeonMajorItems).ToList();
        var allItemsAndEntrances = MajorItems.Concat(DungeonMajorItems).Concat(DungeonEntrances).ToList();

        //Like entrances, constraints shouldn't check logic when placing
        //Shuffle constraints
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.DungeonConstraint)
            ? locationGroups.First(group => group.Key == LocationType.DungeonConstraint).ToList()
            : new List<Location>();
        nextLocationGroup.Shuffle(Rng);
        Logger.Instance.LogInfo($"Placing Dungeon Constraints");
        FastFillConstraints(DungeonConstraints, nextLocationGroup);
        
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.OverworldConstraint)
            ? locationGroups.First(group => group.Key == LocationType.OverworldConstraint).ToList()
            : new List<Location>();
        nextLocationGroup.Shuffle(Rng);
        Logger.Instance.LogInfo($"Placing Overworld Constraints");
        FastFillConstraints(OverworldConstraints, nextLocationGroup);

        // //Shuffle prizes
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.DungeonPrize)
            ? locationGroups.First(group => group.Key == LocationType.DungeonPrize).ToList()
            : new List<Location>();
        Logger.Instance.LogInfo($"Placing Dungeon Prizes");
        var unfilledPrizeLocations = FillLocationsFrontToBack(DungeonPrizes, nextLocationGroup, allItems);

        //Shuffle dungeon majors
        nextLocationGroup = locationGroups.Any(group => group.Key == LocationType.Dungeon)
            ? locationGroups.First(group => group.Key == LocationType.Dungeon).ToList()
            : new List<Location>();
        nextLocationGroup.AddRange(unfilledPrizeLocations);
        Logger.Instance.LogInfo($"Placing Dungeon Majors");
        var unfilledLocations = FillLocationsFrontToBack(DungeonMajorItems, nextLocationGroup, majorsAndEntrances);

        //Shuffle dungeon minors
        Logger.Instance.LogInfo($"Placing Dungeon Minors");
        unfilledLocations.AddRange(FillLocationsFrontToBack(DungeonMinorItems,
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
        if ( items.Count == 0 ) Logger.Instance.LogInfo($"Nothing to place");
        for (; items.Count > 0;)
        {
            // Get a random item from the list and save its index
            var usingFallback = false;
            var itemIndex = Rng.Next(items.Count);
            while (errorIndexes.Contains(itemIndex))
                itemIndex = Rng.Next(items.Count);

            var item = items[itemIndex];

            // Take item out of pool
            items.RemoveAt(itemIndex);
            
            filledLocations.AddRange(UpdateObtainedItemsFromPlacedLocations());

            // Find locations that are available for placing the item
            var availableLocations = locations.Where(location => location.CanPlace(item, Locations))
                .ToList();

            if (availableLocations.Count == 0)
            {
                availableLocations = fallbackLocations
                    .Where(location => location.CanPlace(item, Locations)).ToList();
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
                        $"Failed to place {items.Count} items, last attempt: {item.Type}! Subvalue: {StringUtil.AsStringHex2(item.SubValue)}, Dungeon: {item.Dungeon}");

                continue;
            }

            var locationIndex = Rng.Next(availableLocations.Count);

            availableLocations[locationIndex].Fill(item);
            Logger.Instance.LogInfo(
                $"Placed {item.Type} subtype {StringUtil.AsStringHex2(item.SubValue)} at {availableLocations[locationIndex].Name} from {availableLocations.Count} locations, with {items.Count} items remaining");

            if (usingFallback) fallbackLocations.Remove(availableLocations[locationIndex]);
            else locations.Remove(availableLocations[locationIndex]);

            item.NotifyParentDependencies(true);

            filledLocations.Add(availableLocations[locationIndex]);

            if (items.Count == 0) Logger.Instance.LogInfo($"All {item.ShufflePool} placed");

            errorIndexes.Clear();
        }

        assumedItems.ForEach(item => item.NotifyParentDependencies(false));
        
        FilledLocations.AddRange(filledLocations);
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
            var itemIndex = Rng.Next(items.Count);
            while (errorIndexes.Contains(itemIndex))
                itemIndex = Rng.Next(items.Count);

            var item = items[itemIndex];

            // Take item out of pool
            items.RemoveAt(itemIndex);
            item.NotifyParentDependencies(false);
            
            var tempFilledLocations = UpdateObtainedItemsFromPlacedLocations();

            // Find locations that are available for placing the item
            var availableLocations = locations.Where(location => location.CanPlace(item, Locations))
                .ToList();

            if (availableLocations.Count == 0)
            {
                availableLocations = fallbackLocations
                    .Where(location => location.CanPlace(item, Locations)).ToList();
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

            var locationIndex = Rng.Next(availableLocations.Count);

            availableLocations[locationIndex].Fill(item);
            Logger.Instance.LogInfo(
                $"Placed {item.Type.ToString()} subtype {StringUtil.AsStringHex2(item.SubValue)} at {availableLocations[locationIndex].Name} from {availableLocations.Count} locations, with {items.Count} items remaining");

            if (usingFallback) fallbackLocations.Remove(availableLocations[locationIndex]);
            else locations.Remove(availableLocations[locationIndex]);

            FilledLocations.Add(availableLocations[locationIndex]);
            FilledLocations.AddRange(tempFilledLocations);
            tempFilledLocations.ForEach(location => location.Contents!.Value.NotifyParentDependencies(false));

            if (items.Count == 0) Logger.Instance.LogInfo($"All {item.ShufflePool} placed");

            errorIndexes.Clear();
        }

        assumedItems.ForEach(item => item.NotifyParentDependencies(false));
        
        FilledLocations.AddRange(filledLocations);
        filledLocations.ForEach(location => location.Contents!.Value.NotifyParentDependencies(false));

        return locations.Concat(fallbackLocations).ToList();
    }

    public override string GetEventWrites()
    {
        var eventBuilder = new StringBuilder();

        foreach (var location in Locations) location.WriteLocationEvent(eventBuilder);

        foreach (var define in LogicParser.GetEventDefines()) define.WriteDefineString(eventBuilder);

        var seedValues = new byte[8];
        seedValues[0] = (byte)((Seed >> 00) & 0xFF);
        seedValues[1] = (byte)((Seed >> 08) & 0xFF);
        seedValues[2] = (byte)((Seed >> 16) & 0xFF);
        seedValues[3] = (byte)((Seed >> 24) & 0xFF);
        seedValues[4] = (byte)((Seed >> 32) & 0xFF);
        seedValues[5] = (byte)((Seed >> 40) & 0xFF);
        seedValues[6] = (byte)((Seed >> 48) & 0xFF);
        seedValues[7] = (byte)((Seed >> 56) & 0xFF);
        var crc = (int)seedValues.Crc32();

        eventBuilder.AppendLine("#define seedHashed 0x" + StringUtil.AsStringHex8(crc));
        eventBuilder.AppendLine("#define settingHash 0x" + StringUtil.AsStringHex8((int)GetFinalOptions().OnlyLogic().GetHash()));

        return eventBuilder.ToString();
    }
}
