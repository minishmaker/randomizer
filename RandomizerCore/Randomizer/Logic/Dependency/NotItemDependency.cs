using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class NotItemDependency : DependencyBase
{
    private readonly Item ExcludedItem;

    public NotItemDependency(Item item) : base(false)
    {
        ExcludedItem = item;
    }

    public override bool DependencyFulfilled(Item? itemToPlace = null)
    {
        Result = !ExcludedItem.EqualsIgnoreShufflePool(itemToPlace);
        return Result;
    }

    public override void UpdateDependencyResult(bool newResult)
    {
        //does nothing
    }

    public override void AddDependencyTargetToDependency(object target)
    {
        //does nothing
    }

    public override void ExpandRequiredDependencies(List<Location.Location> locations, List<Item> items)
    {
        RemoveCachingSupportFromParents();
    }
}
