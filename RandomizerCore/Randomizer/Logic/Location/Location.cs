using System.Text;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Dependency;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.IO;

namespace RandomizerCore.Randomizer.Logic.Location;

public class Location
{
    private readonly List<LocationAddress> Addresses;
    private readonly List<EventLocationAddress> Defines;
    private Item DefaultContents;

    public bool Addressed;

    public List<DependencyBase> Dependencies;
    public List<string> Dungeons;
    public bool Filled;
    public bool HideFromSpoilerLog;
    public string Name;
    internal List<LocationDependency> RelatedDependencies;
    public bool SecondaryAddressed;
    public LocationType Type;

    public Location(LocationType type, string name, List<string> dungeons, List<LocationAddress> addresses,
        List<EventLocationAddress> defines, List<DependencyBase> dependencies, Item? replacementContents = null,
        bool hideFromSpoilerLog = false)
    {
        RelatedDependencies = new List<LocationDependency>();
        Type = type;
        Name = name;
        Dungeons = dungeons;

        // One location can have several addresses
        Addresses = addresses;

        // Because the two have zero overlap, the EventLocationAddresses are separate.
        Defines = defines;

        // Whether it's addressed or not determines what can be placed
        Addressed = IsAddressed();
        SecondaryAddressed = IsSecondaryAddressed();

        Dependencies = dependencies;

        if (replacementContents != null)
        {
            DefaultContents = (Item)replacementContents;
            Contents = DefaultContents;
        }

        Filled = false;
        HideFromSpoilerLog = hideFromSpoilerLog;
    }

    public static List<Func<Location, Item, List<Location>, bool>> ShufflerConstraints { get; } = new();

    public Item? Contents { get; private set; }
    public int RecursionCount { get; private set; }

    public static List<Item> GetItems(List<Location> locations)
    {
        return (from location in locations where location.Contents.HasValue select location.Contents!.Value).ToList();
    }

    public bool IsAddressed()
    {
        foreach (var address in Addresses)
            // If any of the location's addresses validly index the first byte, it can be written to
            if ((address.Type & AddressType.FirstByte) == AddressType.FirstByte && address.Address != 0)
                return true;

        foreach (var define in Defines)
            // If any of the defined addresses isn't 
            if ((define.Type & AddressType.FirstByte) == AddressType.FirstByte &&
                !string.IsNullOrEmpty(define.Define.Name))
                return true;
        return false;
    }

    public bool IsSecondaryAddressed()
    {
        foreach (var address in Addresses)
            // If any of the location's addresses validly index the first byte, it can be written to
            if ((address.Type & AddressType.SecondByte) == AddressType.SecondByte && address.Address != 0)
                return true;

        foreach (var define in Defines)
            // If any of the defined addresses is first byte, it's valid
            if ((define.Type & AddressType.SecondByte) == AddressType.SecondByte &&
                !string.IsNullOrEmpty(define.Define.Name))
                return true;

        // Logger.Instance.LogInfo($"Can't place subvalued items in {Name}");
        return false;
    }

    public void WriteLocation(Writer w)
    {
        if (!Contents.HasValue) return;

        // Write each address to ROM
        foreach (var address in Addresses) address.WriteAddress(w, Contents.Value);
    }

    public void WriteLocationEvent(StringBuilder w)
    {
        if (!Contents.HasValue) return;

        foreach (var define in Defines) define.WriteDefine(w, Contents.Value);
    }

    /// <summary>
    ///     Check if an item can be placed into the location
    /// </summary>
    /// <param name="itemToPlace">The item to check placeability of</param>
    /// <param name="availableItems">The items used for checking accessibility</param>
    /// <param name="locations">The locations used for checking accessibility</param>
    /// <returns>If the item can be placed in this location</returns>
    public bool CanPlace(Item itemToPlace, List<Location> allLocations)
    {
        if (ShufflerConstraints.Any() &&
            !ShufflerConstraints.Contains(Imports.LogicImports.FunctionValues["VERIFY_LOCATION_IS_ACCESSIBLE"]))
            ShufflerConstraints.Clear();

        switch (Type)
        {
            case LocationType.Helper:
            case LocationType.Untyped:
                return false;
        }

        // Unaddressed locations can't contain anything
        if (!Addressed) return false;

        // Without a secondary address, items with subvalues cannot be placed
        if (!SecondaryAddressed && itemToPlace.SubValue != 0) return false;

        // Can only place in correct dungeon
        if (itemToPlace.Dungeon != "" && !Dungeons.Contains(itemToPlace.Dungeon)) return false;

        if (itemToPlace.ShufflePool is ItemPool.DungeonPrize && Type != LocationType.DungeonPrize) return false;

        return ShufflerConstraints.All(constraint => constraint.Invoke(this, itemToPlace, allLocations));
    }

    public bool IsAccessible(Item? itemToPlace = null)
    {
        if (RecursionCount > 0) return false;

        RecursionCount++;

        if (Dependencies.Any(dependency => !dependency.DependencyFulfilled(itemToPlace)))
        {
            RecursionCount--;
            return false;
        }

        RecursionCount--;
        return true;
    }

    public void Fill(Item contents)
    {
        Contents = contents;
        Filled = true;
    }

    public void SetItem(Item contents)
    {
        Contents = contents;
        DefaultContents = contents;
    }

    public void SetDefaultContents()
    {
        Contents = DefaultContents;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var dungeon in Dungeons)
            builder.Append(dungeon).Append(' ');

        return $"{Name}: {builder}";
    }
}
