using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class AndDependency : DependencyBase
{
    public List<DependencyBase> AndList;

    public AndDependency(List<DependencyBase> dependencyList)
    {
        AndList = dependencyList;
    }

    public override bool DependencyFulfilled(List<Item> availableItems, List<Location.Location> locations)
    {
        foreach (var dependency in AndList)
            if (dependency.DependencyFulfilled(availableItems, locations) == false)
                return false;
        
        return true;
    }
}