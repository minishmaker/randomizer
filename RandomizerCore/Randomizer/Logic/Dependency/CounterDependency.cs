using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class CounterDependency : DependencyBase
{
    public Dictionary<DependencyBase, int> DependencySet;
    public int RequiredCount;

    public CounterDependency(Dictionary<DependencyBase, int> dependencySet, int reqValue)
    {
        DependencySet = dependencySet;
        RequiredCount = reqValue;
    }

    public override bool DependencyFulfilled(List<Item> availableItems, List<Location.Location> locations, Item? itemToPlace = null)
    {
        var counter = 0;

        foreach (var dependency in DependencySet.Keys)
            if (dependency.DependencyFulfilled(availableItems, locations))
            {
                var amountFound = 1;

                if (dependency.GetType() == typeof(ItemDependency))
                    amountFound = availableItems.FindAll(i =>
                        dependency.DependencyFulfilled(new List<Item>(new Item[1] { i }), locations)).Count;

                counter += DependencySet[dependency] * amountFound;
            }

        return counter >= RequiredCount;
    }
}