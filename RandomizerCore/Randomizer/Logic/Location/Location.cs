using System.Text;
using RandomizerCore.Core;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Dependency;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.IO;
using RandomizerCore.Utilities.Logging;

namespace RandomizerCore.Randomizer.Logic.Location;

public class Location
{
    public bool Addressed;
    private readonly List<LocationAddress> _addresses;
    private bool? _availableCache;
    private Item _defaultContents;
    private readonly List<EventLocationAddress> _defines;

    public List<DependencyBase> Dependencies;
    public string Dungeon;
    public bool Filled;
    public string Name;
    public bool SecondaryAddressed;
    public LocationType Type;

    public Location(LocationType type, string name, string dungeon, List<LocationAddress> addresses,
        List<EventLocationAddress> defines, List<DependencyBase> dependencies, Item? replacementContents = null)
    {
        Type = type;
        Name = name;
        Dungeon = dungeon;

        // One location can have several addresses
        _addresses = addresses;

        // Because the two have zero overlap, the EventLocationAddresses are separate.
        _defines = defines;

        // Whether it's addressed or not determines what can be placed
        Addressed = IsAddressed();
        SecondaryAddressed = IsSecondaryAddressed();

        Dependencies = dependencies;

        if (replacementContents == null)
        {
            // Need to get the item from the ROM
            _defaultContents = GetItemContents();
            Contents = _defaultContents;
        }
        else
        {
            // The item is specified by the logic rather than the ROM
            _defaultContents = (Item)replacementContents;
            Contents = _defaultContents;
        }

        Filled = false;
        _availableCache = null;
    }

    public Item Contents { get; private set; }
    public int RecursionCount { get; private set; }

    public static List<Item> GetItems(List<Location> locations)
    {
        var items = new List<Item>();

        foreach (var location in locations) items.Add(location.Contents);

        return items;
    }

    public bool IsAddressed()
    {
        foreach (var address in _addresses)
            // If any of the location's addresses validly index the first byte, it can be written to
            if ((address.Type & AddressType.FirstByte) == AddressType.FirstByte && address.Address != 0)
                return true;

        foreach (var define in _defines)
            // If any of the defined addresses isn't 
            if ((define.Type & AddressType.FirstByte) == AddressType.FirstByte &&
                !string.IsNullOrEmpty(define.Define.Name))
                return true;
        return false;
    }

    public bool IsSecondaryAddressed()
    {
        foreach (var address in _addresses)
            // If any of the location's addresses validly index the first byte, it can be written to
            if ((address.Type & AddressType.SecondByte) == AddressType.SecondByte && address.Address != 0)
                return true;

        foreach (var define in _defines)
            // If any of the defined addresses is first byte, it's valid
            if ((define.Type & AddressType.SecondByte) == AddressType.SecondByte &&
                !string.IsNullOrEmpty(define.Define.Name))
                return true;

        Logger.Instance.LogInfo($"Can't place subvalued items in {Name}");
        return false;
    }

    public void WriteLocation(Writer w)
    {
        // Write each address to ROM
        foreach (var address in _addresses) address.WriteAddress(w, Contents);
    }

    public void WriteLocationEvent(StringBuilder w)
    {
        foreach (var define in _defines) define.WriteDefine(w, Contents);
    }

    /// <summary>
    ///     Read the item from the ROM
    /// </summary>
    /// <returns>The item contained at the address</returns>
    public Item GetItemContents()
    {
        var type = ItemType.Untyped;
        byte subType = 0;

        // Read all the addresses, taking the last of each byte type
        foreach (var address in _addresses) address.ReadAddress(Rom.Instance.reader, ref type, ref subType);

        // If the contents of the address aren't defined/are untyped, it's probably broken
        if (type == ItemType.Untyped && Type != LocationType.Helper && Type != LocationType.Unshuffled)
            Logger.Instance.LogWarning($"Untyped contents in {Name}! Addresses may be bad");

        // Dungeon items get the Dungeon part defined
        if (Type == LocationType.DungeonItem)
            return new Item(type, subType, Dungeon);
        return new Item(type, subType);
    }

    /// <summary>
    ///     Check if an item can be placed into the location
    /// </summary>
    /// <param name="itemToPlace">The item to check placability of</param>
    /// <param name="availableItems">The items used for checking accessibility</param>
    /// <param name="locations">The locations used for checkign accessiblility</param>
    /// <returns>If the item can be placed in this location</returns>
    public bool CanPlace(Item itemToPlace, List<Item> availableItems, List<Location> locations)
    {
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
        if (itemToPlace.Dungeon != "")
            if (itemToPlace.Dungeon != Dungeon)
                return false;

        return IsAccessible(availableItems, locations);
    }

    public bool IsAccessible(List<Item> availableItems, List<Location> locations)
    {
        if (RecursionCount > 0) return false;

        RecursionCount++;

        if (_availableCache != null)
        {
            RecursionCount--;
            return (bool)_availableCache;
        }

        foreach (var dependency in Dependencies)
            if (!dependency.DependencyFulfilled(availableItems, locations))
            {
                _availableCache = false;
                RecursionCount--;
                return false;
            }

        _availableCache = true;
        RecursionCount--;
        return true;
    }

    public void InvalidateCache()
    {
        _availableCache = null;
    }

    public void Fill(Item contents)
    {
        Contents = contents;
        Filled = true;
    }

    public void SetItem(Item contents)
    {
        Contents = contents;
        _defaultContents = contents;
    }

    public void SetDefaultContents()
    {
        Contents = _defaultContents;
    }

    public override string ToString()
    {
        return Name + ":" + Dungeon;
    }
}