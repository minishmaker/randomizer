using RandomizerCore.Randomizer.Logic.Location;

namespace RandomizerCore.Randomizer.Models;

public class Sphere
{
    public List<Item> Items { get; set; }
    
    public List<Location> Locations { get; set; }
    
    public int SphereNumber { get; set; }
    
    public int TotalShuffledLocations { get; set; }
    
    public int MaxRetryCount { get; set; }
    
    public int CurrentAttemptCount { get; set; }

    public List<Item> PreFilledItemsAddedThisSphere { get; set; }

    public List<Location> PreFilledLocationsAddedThisSphere { get; set; }

    //public List<Item> ElementsPlacedThisSphere { get; set; }

    //public List<Location> ElementLocationsThisSphere { get; set; }

    //public List<Item> DungeonItemsPlacedThisSphere { get; set; }

    //public List<Location> DungeonLocationsPlacedThisSphere { get; set; }
}