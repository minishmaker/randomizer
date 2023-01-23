using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class ItemDependency : DependencyBase
{
    private readonly int _count;
    private readonly Item _requiredItem;

    public ItemDependency(Item item, int count = 1)
    {
        _requiredItem = item;
        _count = count;
    }

    public override bool DependencyFulfilled(List<Item> availableItems, List<Location.Location> locations, Item? itemToPlace = null)
    {
        var counter = 0;
        foreach (var item in availableItems)
            if (item.Type == _requiredItem.Type &&
                (item.SubValue == _requiredItem.SubValue || item.UseAny || _requiredItem.UseAny) &&
                (_requiredItem.Dungeon == "" || _requiredItem.Dungeon == item.Dungeon))
            {
                counter++;
                if (counter >= _count) return true;
            }

        //Console.WriteLine($"Missing {RequiredItem.Type}");

        return false;
    }
}