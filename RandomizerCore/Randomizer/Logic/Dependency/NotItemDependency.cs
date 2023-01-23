using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class NotItemDependency : DependencyBase
{
    private readonly Item _requiredItem;

    public NotItemDependency(Item item)
    {
        _requiredItem = item;
    }

    public override bool DependencyFulfilled(List<Item> availableItems, List<Location.Location> locations, Item? itemToPlace = null)
    {
        return _requiredItem.Equals(itemToPlace);

        //Console.WriteLine($"Missing {RequiredItem.Type}");
    }
}