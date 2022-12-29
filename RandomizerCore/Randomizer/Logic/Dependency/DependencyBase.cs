using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public abstract class DependencyBase
{
    public abstract bool DependencyFulfilled(List<Item> availableItems, List<Location.Location> locations);
}