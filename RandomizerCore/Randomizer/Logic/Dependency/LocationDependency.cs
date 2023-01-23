using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;


public class LocationDependency : DependencyBase
{
    private readonly string _requiredLocationName;

    public LocationDependency(string locationName)
    {
        _requiredLocationName = locationName;
    }

    public override bool DependencyFulfilled(List<Item> availableItems, List<Location.Location> locations, Item? itemToPlace = null)
    {
        foreach (var location in locations)
            if (location.Name == _requiredLocationName)
                return location.IsAccessible(availableItems, locations, itemToPlace);
        
        throw new ShuffleException(
            $"Could not find location {_requiredLocationName}. You may have an invalid logic file!");
    }
}