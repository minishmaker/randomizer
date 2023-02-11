using RandomizerCore.Core;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Dependency;
using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Imports;

/*
 * This class defines functions that can be imported with the !import statement in logic.
 * Functions should all fit the following template:
 *   private static bool [DefineName]Import(Location.Location self, Item itemToPlace, List<Item> availableItems, List<Location.Location> allLocations)
 *
 * The list of functions, Dictionary<string, Func<Location.Location, Item, List<Item>, List<Location.Location>, bool>> FunctionValues, should have the string be the name of the imported
 * define, and the Func will be a lambda that calls into the function you wrote.
 *
 * Functions should return true if the condition is met (the item can be placed on the target location), and false if not
 */
public static class LogicImports
{
    public static Dictionary<string, Func<Location.Location, Item, List<Item>, List<Location.Location>, bool>> FunctionValues = new()
    {
        {
            "NON_ELEMENT_DUNGEONS_BARREN",
            NonElementDungeonsBarrenImport
        },
        {
            "NON_ELEMENT_DUNGEONS_NOT_REQUIRED",
            NonElementDungeonsNotRequiredImport
        },
        {
            "VERIFY_LOCATION_IS_ACCESSIBLE",
            VerifyLocationIsAccessibleImport
        },
    };

    private static bool VerifyLocationIsAccessibleImport(Location.Location self, Item itemToPlace, List<Item> availableItems, List<Location.Location> allLocations)
    {
        return self.IsAccessible(availableItems, allLocations, itemToPlace);
    }

    private static bool NonElementDungeonsBarrenImport(Location.Location self, Item itemToPlace, List<Item> availableItems, List<Location.Location> allLocations)
    {
        const string beatVaatiDefineName = "BeatVaati";

        if (itemToPlace.ShufflePool is not ItemPool.Major and not ItemPool.DungeonMajor || self.Dungeons.Count == 0) return true;

        var prizeDungeonForItem = allLocations.Where(location => location.Type == LocationType.DungeonPrize)
            .FirstOrDefault(prize => prize.Dungeons.Any(dungeon => self.Dungeons.Any(dun => dun == dungeon)));

        if (prizeDungeonForItem == null) return true;

        var accessible = prizeDungeonForItem.Contents is
        {
            Type: ItemType.EarthElement or ItemType.FireElement or ItemType.WaterElement or ItemType.WindElement
        } || (itemToPlace.ShufflePool is ItemPool.DungeonMajor && new LocationDependency(beatVaatiDefineName).DependencyFulfilled(availableItems, allLocations));

        if (!accessible && !self.Dependencies.Any(dep => dep is LocationDependency && ((LocationDependency)dep)._requiredLocationName == beatVaatiDefineName))
            self.Dependencies.Add(new LocationDependency(beatVaatiDefineName));

        return accessible;
    }
    
    private static bool NonElementDungeonsNotRequiredImport(Location.Location self, Item itemToPlace, List<Item> availableItems, List<Location.Location> allLocations)
    {
        const string beatVaatiDefineName = "BeatVaati";
        if (itemToPlace.ShufflePool is not ItemPool.Major and not ItemPool.DungeonMajor || self.Dungeons.Count == 0) return true;

        var prizeDungeonForItem = allLocations.Where(location => location.Type == LocationType.DungeonPrize)
            .FirstOrDefault(prize => prize.Dungeons.Any(dungeon => self.Dungeons.Any(dun => dun == dungeon)));

        if (prizeDungeonForItem == null) return true;

        var accessible = prizeDungeonForItem.Contents is
        {
            Type: ItemType.EarthElement or ItemType.FireElement or ItemType.WaterElement or ItemType.WindElement
        } || new LocationDependency(beatVaatiDefineName).DependencyFulfilled(availableItems, allLocations);

        if (!accessible && !self.Dependencies.Any(dep => dep is LocationDependency && ((LocationDependency)dep)._requiredLocationName == beatVaatiDefineName)) 
            self.Dependencies.Add(new LocationDependency(beatVaatiDefineName));

        return accessible;
    }
}
