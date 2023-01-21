using RandomizerCore.Randomizer.Logic.Location;

namespace RandomizerCore.Randomizer.Models;

public class Sphere
{
    public List<Item> Items { get; set; }
    
    public List<Location> Locations { get; set; }
    
    public int SphereNumber { get; set; }
    
    public int TotalShuffledLocations { get; set; }
}