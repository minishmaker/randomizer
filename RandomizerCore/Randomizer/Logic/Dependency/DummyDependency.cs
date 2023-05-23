using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class DummyDependency : DependencyBase
{
    public DummyDependency() : base(true)
    {
    }

    public override bool DependencyFulfilled(Item? itemToPlace = null)
    {
        return true;
    }

    public override void UpdateDependencyResult(bool newResult)
    {
    }

    public override void AddDependencyTargetToDependency(object target)
    {
    }

    public override void ExpandRequiredDependencies(List<Location.Location> locations, List<Item> items)
    {
    }
}
