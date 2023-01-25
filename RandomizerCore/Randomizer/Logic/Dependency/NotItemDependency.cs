using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class NotItemDependency : DependencyBase
{
    private readonly Item _excludedItem;

    public NotItemDependency(Item item)
    {
        _excludedItem = item;
    }

    public override bool DependencyFulfilled(List<Item> availableItems, List<Location.Location> locations, Item? itemToPlace = null)
    {
        return !_excludedItem.EqualsIgnoreShufflePool(itemToPlace);

        //Console.WriteLine($"Missing {RequiredItem.Type}");
    }
}