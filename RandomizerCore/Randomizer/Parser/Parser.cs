using RandomizerCore.Core;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Logic.Dependency;
using RandomizerCore.Randomizer.Logic.Location;
using RandomizerCore.Randomizer.Models;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Models;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer.Parser;

public class Parser
{
    public DirectiveParser SubParser;

    public Parser()
    {
        SubParser = new DirectiveParser();
    }

    /// <summary>
    ///     Get a list of the dependencies contained within a given logic string. Compound dependencies call this recursively
    /// </summary>
    /// <param name="logic">The logic string to parse</param>
    /// <returns>The list of dependencies contained in the logic</returns>
    public List<DependencyBase> GetDependencies(string logic)
    {
        var dependencies = new List<DependencyBase>();


        var subLogic = SplitDependencies(logic);

        foreach (var sequence in subLogic)
        {
            if (string.IsNullOrEmpty(sequence)) continue;

            switch (sequence[0])
            {
                // If the first character of the string is & or |, it's a compound dependency
                // These are handled recursively, with the first character chopped off
                case '&':
                    var andList = GetDependencies(sequence.Substring(1));
                    var andDependency = new AndDependency(andList);
                    dependencies.Add(andDependency);
                    break;
                case '|':
                    var orList = GetDependencies(sequence.Substring(1));
                    var orDependency = new OrDependency(orList);
                    dependencies.Add(orDependency);
                    break;
                case '~':
                    var parts = sequence.Substring(1).Split('.');
                    if (parts.Length < 3)
                        throw new ParserException("Item in not dependency has the wrong number of parts!");
                    if (!Enum.TryParse(parts[1], out ItemType type))
                        throw new ParserException("Could not parse item in not dependency!");
                    if (!byte.TryParse(parts[2], out var subValue))
                        throw new ParserException("Could not parse item in not dependency!");
                    
                    dependencies.Add(new NotItemDependency(new Item(type, subValue)));
                    break;
                case '+':
                    var valueDict = new Dictionary<DependencyBase, int>();
                    var reqValueString = sequence.Split(',')[0];

                    if (!StringUtil.ParseString(reqValueString.Substring(1), out int reqValue))
                        throw new ParserException($"Invalid total for counter! {reqValueString.Substring(1)}");

                    var depStrings = sequence.Substring(reqValueString.Length + 1).Split(',');

                    foreach (var depString in depStrings)
                    {
                        var dependencyString = depString;
                        var depValue = 1;
                        var values = dependencyString.Split(':');

                        if (values.Length >= 3)
                        {
                            if (!StringUtil.ParseString(values[2], out depValue)) depValue = 1;
                            dependencyString =
                                dependencyString.Substring(0, dependencyString.Length - (values[2].Length + 1));
                        }

                        var temp = GetDependencies(dependencyString);

                        valueDict.Add(temp[0], depValue);
                    }

                    dependencies.Add(new CounterDependency(valueDict, reqValue));
                    break;
                default:
                    var splitSequence = sequence.Split(':');
                    var requirement = splitSequence[0];
                    var dependencyParts = requirement.Split('.');
                    
                    // var dungeon = "";
                    var count = 1;

                    if (splitSequence.Length >= 2 && dependencyParts[0] == "Items")
                    {
                        // dungeon = splitSequence[1];
                        if (!StringUtil.ParseString(splitSequence[1], out count))
                            throw new ParserException($"Invalid amount on\"{sequence}\"!");
                    }

                    if (dependencyParts.Length < 2) throw new ParserException($"Invalid logic \"{logic}\"!");

                    switch (dependencyParts[0])
                    {
                        case "Locations":
                        case "Helpers":
                            var locationDependency = new LocationDependency(dependencyParts[1]);
                            dependencies.Add(locationDependency);
                            break;
                        case "Items":
                            var item = new Item(sequence, " Parse");
                            var itemDependency = new ItemDependency(item, count);
                            dependencies.Add(itemDependency);

                            break;
                        default:
                            throw new ParserException($"\"{dependencyParts[0]}\" is not a valid dependency type!");
                    }

                    break;
            }
        }

        return dependencies;
    }

    /// <summary>
    ///     Split a logic string into the separate dependencies it represents
    /// </summary>
    /// <param name="logic">The logic string to split</param>
    /// <returns>A list of the individual dependencies within the logic</returns>
    public List<string> SplitDependencies(string logic)
    {
        var subLogic = new List<string>();

        var parenCount = 0;
        var subsection = "";

        foreach (var character in logic)
            switch (character)
            {
                case ',':
                    if (parenCount == 0)
                    {
                        // Not within parentheses, should start a new logic string
                        subLogic.Add(subsection);
                        subsection = "";
                    }
                    else
                    {
                        // Comma is within parentheses, so it will be parsed as a compound dependency
                        subsection += ',';
                    }

                    break;
                case '(':
                    // Nested parentheses should be in the subsection
                    if (parenCount > 0) subsection += '(';

                    // Open parenthesis, so everything until it's closed should be one block
                    parenCount++;
                    break;
                case ')':
                    // Close parenthesis, so reduce the number of open blocks by 1
                    parenCount--;

                    // Nested parentheses should be in the subsection
                    if (parenCount > 0) subsection += ')';
                    break;
                default:
                    // Not a special case, so it should be added to the current logic subsection
                    subsection += character;
                    break;
            }

        // Add final section to logic
        subLogic.Add(subsection);

        if (parenCount > 0)
            throw new ParserException(
                $"Parentheses could not be parsed correctly in string \"{logic}\"! Make sure none of them are mismatched.");

        return subLogic;
    }

    public List<Item> GetItems(string itemText)
    {
        var allItemParts = itemText.Split(';');
        var itemParts = allItemParts[0].Split(':');
        var subParts = itemParts[0].Split('.');

        if (allItemParts.Length is < 2 or > 3)
            throw new ParserException("Invalid item! Item does not have the correct number of parameters!");

        if (!Enum.TryParse(subParts[1], out ItemType replacementType))
            throw new ParserException("Item has invalid item type!");
        
        byte subType = 0;
        if (subParts.Length >= 3)
            if (!StringUtil.ParseString(subParts[2], out subType))
                if (Enum.TryParse(subParts[2], out KinstoneType subKinstoneType))
                    subType = (byte)subKinstoneType;

        var amount = itemParts.Length > 1 ? int.Parse(itemParts[1]) : 1;

        if (!Enum.TryParse(allItemParts[1], out ItemPool itemShufflePool))
            throw new ParserException("Item has invalid shuffle pool!");

        var items = new List<Item>();

        var dungeon = "";
        
        if (itemShufflePool is ItemPool.DungeonMajor && allItemParts.Length < 3)
            throw new ParserException("Dungeon item is missing dungeon name!");
        
        if (allItemParts.Length >= 3)
            dungeon = allItemParts[2];

        while (amount-- > 0)
        {
            items.Add(new Item(replacementType, subType, dungeon, shufflePool: itemShufflePool));
        }

        return items;
    }

    public Location GetLocation(string locationText)
    {
        // Location format: Type;Name;Address;Large;Logic;[Optional]Item
        var locationParts = locationText.Split(';');

        if (locationParts.Length < 3)
        {
            Console.WriteLine("Too short");
            throw new ParserException($"{locationText} in the logic file lacks required fields!");
        }

        var names = locationParts[0].Split(':');
        var name = names[0];

        var dungeons = new List<string>();
        // Colon, must have at least one dungeon specified
        if (names.Length >= 2) dungeons.AddRange(names.Skip(1));

        var locationType = locationParts[1];
        if (!Enum.TryParse(locationType, out LocationType type) || type == LocationType.Untyped)
        {
            Console.WriteLine("Invalid type");
            throw new ParserException($"Location \"{name}\" has invalid type \"{locationType}\"!");
        }

        // Get all comma-separated addresses and properly parse them
        var addressStrings = locationParts[2].Split(',');
        var addresses = new List<LocationAddress>(addressStrings.Length);
        var defines = new List<EventLocationAddress>();
        try
        {
            foreach (var address in addressStrings)
            {
                var parsedAddress = GetAddressFromString(address);
                if (parsedAddress is EventLocationAddress locationAddress)
                    defines.Add(locationAddress);
                else
                    addresses.Add(parsedAddress);
            }
        }
        catch (ParserException error)
        {
            throw new ParserException($"Error at location \"{name}\": {error.Message}");
        }

        var logic = "";
        // Looks like logic is specified
        if (locationParts.Length >= 4) logic = locationParts[3];
        List<DependencyBase> dependencies;
        try
        {
            dependencies = GetDependencies(logic);
        }
        catch (ParserException error)
        {
            throw new ParserException($"Error at location \"{name}\": {error.Message}");
        }

        Item? itemOverride = null;

        if (type == LocationType.Unshuffled)
        {
            if (!(locationParts.Length >= 5) || locationParts[4].Length == 0)
                throw new ParserException($"Unshuffled location missing an item to place there!");
            
            var itemParts = locationParts[4].Split(':');
            var subParts = itemParts[0].Split('.');

            if (subParts[0] == "Items")
            {
                if (Enum.TryParse(subParts[1], out ItemType replacementType))
                {
                    byte subType = 0;
                    if (subParts.Length >= 3)
                        if (!StringUtil.ParseString(subParts[2], out subType))
                            if (Enum.TryParse(subParts[2], out KinstoneType subKinstoneType))
                                subType = (byte)subKinstoneType;

                    itemOverride = new Item(replacementType, subType);
                }
            }
            else
            {
                throw new ParserException($"Location \"{name}\" has invalid item override \"{locationParts[4]}\"!");
            }
        }

        var location = new Location(type, name, dungeons, addresses, defines, dependencies, itemOverride);

        return location;
    }

    /// <summary>
    ///     Turn an address string into a file address
    /// </summary>
    /// <param name="addressString">String representing the address</param>
    /// <returns></returns>
    public LocationAddress GetAddressFromString(string addressString)
    {
        // Either direct address or area-room-chest
        if (addressString == "") return new LocationAddress(AddressType.None);

        // Get the types of the address
        var addressParts = addressString.Split(':');

        var addressType = AddressType.None;
        if (addressParts.Length > 1)
            for (var i = 1; i < addressParts.Length; i++)
                if (Enum.TryParse(addressParts[i], out AddressType subType))
                    // Set the flag(s) indicated by the given type
                    addressType |= subType;
                else
                    throw new ShuffleException(
                        $"{addressParts[i]} in address {addressString} is not a valid address type!");

        // If a byte isn't set, default to both
        if (((addressType & AddressType.FirstByte) | (addressType & AddressType.SecondByte)) == AddressType.None)
            // Set the type of the address to the default
            addressType |= AddressType.BothBytes;

        // If the address is an event define, make it an EventLocationAddress
        if ((addressType & AddressType.Define) == AddressType.Define)
            // The first part refers to the name of the define
            return new EventLocationAddress(addressType, addressParts[0]);

        // The address is a number, so it can simply be parsed
        if (StringUtil.ParseString(addressParts[0], out int address)) return new LocationAddress(addressType, address);

        // The address is an entity, so it should be parsed as an area-room-entity number
        var entityDetails = addressParts[0].Split('-');
        if (entityDetails.Length != 3)
            throw new ParserException($"Entity data \"{addressString}\" does not have a full address!");

        if (!StringUtil.ParseString(entityDetails[0], out int area))
            throw new ParserException($"Entity data \"{addressString}\" has an invalid area index!");

        if (!StringUtil.ParseString(entityDetails[1], out int room))
            throw new ParserException($"Entity data \"{addressString}\" has an invalid room index!");

        if (!StringUtil.ParseString(entityDetails[2], out int chest))
            throw new ParserException($"Entity data \"{addressString}\" has an invalid entity index!");

        int addressValue;

        if ((addressType & AddressType.GroundItem) == AddressType.GroundItem)
        {
            throw new NotImplementedException();
        }

        // Look chest address up in table
        var areaTableAddr = Rom.Instance.reader.ReadAddr(Rom.Instance.Headers.AreaMetadataBase + (area << 2));
        var roomTableAddr = Rom.Instance.reader.ReadAddr(areaTableAddr + (room << 2));
        var chestTableAddr = Rom.Instance.reader.ReadAddr(roomTableAddr + 0x0C);

        // Chests are 8 bytes long, and the item is stored 2 bytes in
        addressValue = chestTableAddr + chest * 8 + 0x02;


        return new LocationAddress(addressType, addressValue);
    }

    public (List<Location> locations, List<Item> items) ParseLocationsAndItems(string[] lines, Random rng)
    {
        Logger.Instance.BeginLogTransaction();
        var locations = new List<Location>();
        var items = new List<Item>();
        for (var index = 0; index < lines.Length; ++index)
        {
            var line = lines[index];
            try
            {
                // Spaces are ignored, and everything after a # is a comment
                var trimmedLine = line.Split('#')[0].Trim();

                // Empty lines or locations are ignored
                if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

                if (!SubParser.ShouldIgnoreLines())
                {
                    // Replace defines between `
                    // Probably a more efficient way to do it, but eh
                    if (trimmedLine.IndexOf("`", StringComparison.Ordinal) != -1)
                    {
                        if (trimmedLine.Contains("`RAND_INT`"))
                            trimmedLine = trimmedLine.Replace("`RAND_INT`", StringUtil.AsStringHex8(rng.Next()));
                        trimmedLine = SubParser.ReplaceDefines(trimmedLine);
                    }

                    if (trimmedLine[0] == '!')
                    {
                        // Parse the string as a directive, ignoring preparsed directives
                        if (!SubParser.ParseOnLoad(trimmedLine))
                                SubParser.ParseDirective(trimmedLine);
                    }
                    else
                    {
                        // Remove spaces as they're ignored in locations
                        trimmedLine = trimmedLine.Replace(" ", "");
                        trimmedLine = trimmedLine.Replace("\t", "");

                        if (trimmedLine.Split('.')[0] == "Items")
                        {
                            //It is an item, parse it as one
                            var newItem = GetItems(trimmedLine);
                            items.AddRange(newItem);
                        }
                        else
                        {
                            //It is a location, parse it as one
                            var newLocation = GetLocation(trimmedLine);
                            locations.Add(newLocation);
                        }
                    }

                }
                else
                {
                    // Only parse directives to check for conditionals
                    if (trimmedLine[0] == '!') SubParser.ParseDirective(trimmedLine);
                }

                Logger.Instance.SaveLogTransaction();
            }
            catch (Exception e)
            {
                Logger.Instance.LogError($"Failed to parse line {index + 1}!");
                Logger.Instance.LogException(e);
                throw new ParserException(
                    $"Error at line \"{index + 1}\", directive \"{line}\": {e.Message}");
            }
        }

        return (locations, items);
    }

    public void PreParse(string[] lines)
    {
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line[0] == '!')
            {
                // Remove comments and excess whitespace
                var trimmedLine = line.Split('#')[0].Trim();

                if (SubParser.ParseOnLoad(trimmedLine)) SubParser.ParseDirective(trimmedLine);
            }
        }
    }

    public List<EventDefine> GetEventDefines()
    {
        var usedDefines = new List<EventDefine>();

        usedDefines.AddRange(SubParser.EventDefines);

        return usedDefines;
    }
}
