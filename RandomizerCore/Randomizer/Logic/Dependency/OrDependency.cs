using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class OrDependency : DependencyBase
{
    public List<DependencyBase> OrList;

    public OrDependency(List<DependencyBase> dependencyList)
    {
        OrList = dependencyList;
    }

    public override bool DependencyFulfilled(List<Item> availableItems, List<Location.Location> locations)
    {
        foreach (var dependency in OrList)
            if (dependency.DependencyFulfilled(availableItems, locations))
                return true;
        
        return false;
    }
}