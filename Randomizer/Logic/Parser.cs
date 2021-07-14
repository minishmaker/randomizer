using System;
using System.Collections.Generic;
using System.Globalization;
using MinishRandomizer.Core;
using MinishRandomizer.Utilities;

namespace MinishRandomizer.Randomizer.Logic
{
    public class ParserException : Exception
    {
        public ParserException(string message) : base(message) { }
    }

    public class Parser
    {
        public DirectiveParser SubParser;

        public Parser()
        {
            SubParser = new DirectiveParser();
        }

        /// <summary>
        /// Get a list of the dependencies contained within a given logic string. Compound dependencies call this recursively
        /// </summary>
        /// <param name="logic">The logic string to parse</param>
        /// <returns>The list of dependencies contained in the logic</returns>
        public List<Dependency> GetDependencies(string logic)
        {
            List<Dependency> dependencies = new List<Dependency>();


            List<string> subLogic = SplitDependencies(logic);

            foreach (string sequence in subLogic)
            {
                if (string.IsNullOrEmpty(sequence))
                {
                    continue;
                }

                switch (sequence[0])
                {
                    // If the first character of the string is & or |, it's a compound dependency
                    // These are handled recursively, with the first character chopped off
                    case '&':
                        List<Dependency> andList = GetDependencies(sequence.Substring(1));
                        AndDependency andDependency = new AndDependency(andList);
                        dependencies.Add(andDependency);
                        break;
                    case '|':
                        List<Dependency> orList = GetDependencies(sequence.Substring(1));
                        OrDependency orDependency = new OrDependency(orList);
                        dependencies.Add(orDependency);
                        break;
                    case '+':
                        Dictionary<Dependency, int> valueDict = new Dictionary<Dependency, int>();
                        var reqValueString = sequence.Split(',')[0];

                        if (!StringUtil.ParseString(reqValueString.Substring(1), out int reqValue))
                        {
                            throw new ParserException($"Invalid total for counter! {reqValueString.Substring(1)}");
                        }

                        string[] depStrings = sequence.Substring(reqValueString.Length + 1).Split(',');

                        foreach (var depString in depStrings)
                        {
                            string dependencyString = depString;
                            int depValue = 1;
                            string[] values = dependencyString.Split(':');

                            if (values.Length >= 3)
                            {
                                if (!StringUtil.ParseString(values[2], out depValue))
                                {
                                    depValue = 1;
                                }
                                dependencyString = dependencyString.Substring(0, dependencyString.Length - (values[2].Length + 1));
                            }

                            List<Dependency> temp = GetDependencies(dependencyString);

                            valueDict.Add(temp[0], depValue);
                        }
                        dependencies.Add(new CounterDependency(valueDict, reqValue));
                        break;
                    default:
                        string[] splitSequence = sequence.Split(':');
                        string requirement = splitSequence[0];
                        string dungeon = "";
                        int count = 1;

                        if (splitSequence.Length >= 2)
                        {
                            dungeon = splitSequence[1];
                            if (splitSequence.Length >= 3)
                            {
                                if (!StringUtil.ParseString(splitSequence[2], out count))
                                {
                                    throw new ParserException($"Invalid amount on\"{sequence}\"!");
                                }
                            }
                        }

                        string[] dependencyParts = requirement.Split('.');

                        if (dependencyParts.Length < 2)
                        {
                            throw new ParserException($"Invalid logic \"{logic}\"!");
                        }

                        switch (dependencyParts[0])
                        {
                            case "Locations":
                            case "Helpers":
                                LocationDependency locationDependency = new LocationDependency(dependencyParts[1]);
                                dependencies.Add(locationDependency);
                                break;
                            case "Items":
                                Item item = new Item(sequence, " Parse");
                                ItemDependency itemDependency = new ItemDependency(item, count);
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
        /// Split a logic string into the separate dependencies it represents
        /// </summary>
        /// <param name="logic">The logic string to split</param>
        /// <returns>A list of the individual dependencies within the logic</returns>
        public List<string> SplitDependencies(string logic)
        {
            List<string> subLogic = new List<string>();

            int parenCount = 0;
            string subsection = "";

            foreach (char character in logic)
            {
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
                        if (parenCount > 0)
                        {
                            subsection += '(';
                        }

                        // Open parenthesis, so everything until it's closed should be one block
                        parenCount++;
                        break;
                    case ')':
                        // Close parenthesis, so reduce the number of open blocks by 1
                        parenCount--;

                        // Nested parentheses should be in the subsection
                        if (parenCount > 0)
                        {
                            subsection += ')';
                        }
                        break;
                    default:
                        // Not a special case, so it should be added to the current logic subsection
                        subsection += character;
                        break;
                }
            }

            // Add final section to logic
            subLogic.Add(subsection);

            if (parenCount > 0)
            {
                throw new ParserException($"Parentheses could not be parsed correctly in string \"{logic}\"! Make sure none of them are mismatched.");
            }

            return subLogic;
        }

        public Location GetLocation(string locationText)
        {
            // Location format: Type;Name;Address;Large;Logic
            string[] locationParts = locationText.Split(';');

            if (locationParts.Length < 3)
            {
                Console.WriteLine("Too short");
                throw new ParserException($"{locationText} in the logic file lacks required fields!");
            }

            string[] names = locationParts[0].Split(':');
            string name = names[0];

            string dungeon = "";
            // Colon, must have a dungeon specified
            if (names.Length >= 2)
            {
                dungeon = names[1];
            }

            string locationType = locationParts[1];
            if (!Enum.TryParse(locationType, out Location.LocationType type) || type == Location.LocationType.Untyped)
            {
                Console.WriteLine("Invalid type");
                throw new ParserException($"Location \"{name}\" has invalid type \"{locationType}\"!");
            }

            // Get all comma-separated addresses and properly parse them
            string[] addressStrings = locationParts[2].Split(',');
            List<LocationAddress> addresses = new List<LocationAddress>(addressStrings.Length);
            List<EventLocationAddress> defines = new List<EventLocationAddress>();
            try
            {
                foreach (string address in addressStrings)
                {
                    LocationAddress parsedAddress = GetAddressFromString(address);
                    if (parsedAddress is EventLocationAddress)
                    {
                        defines.Add((EventLocationAddress)parsedAddress);
                    }
                    else
                    {
                        addresses.Add(parsedAddress);
                    }

                }
            }
            catch(ParserException error)
            {
                throw new ParserException($"Error at location \"{name}\": {error.Message}");
            }

            string logic = "";
            // Looks like logic is specified
            if (locationParts.Length >= 4)
            {
                logic = locationParts[3];
            }
            List<Dependency> dependencies;
            try
            {
               dependencies = GetDependencies(logic);
            }
            catch (ParserException error)
            {
                throw new ParserException($"Error at location \"{name}\": {error.Message}");
            }

            Item? itemOverride = null;
            // Has enough parts for an extra item
            if (locationParts.Length >= 5 && locationParts[4].Length != 0)
            {
                string[] itemParts = locationParts[4].Split(':');
                string[] subParts = itemParts[0].Split('.');

                if (subParts[0] == "Items")
                {
                    if (Enum.TryParse(subParts[1], out ItemType replacementType))
                    {
                        byte subType = 0;
                        if (subParts.Length >= 3)
                        {
                            if (!StringUtil.ParseString(subParts[2], out subType))
                            {
                                if (Enum.TryParse(subParts[2], out KinstoneType subKinstoneType))
                                {
                                    subType = (byte)subKinstoneType;
                                }
                            }
                        }

                        string itemDungeon = "";

                        if (type == Location.LocationType.Unshuffled)
                        {
                            itemDungeon = dungeon;
                        }

                        if (itemParts.Length >= 2)
                        {
                            itemDungeon = itemParts[1];
                        }


                        itemOverride = new Item(replacementType, subType, itemDungeon);
                    }
                }
                else
                {
                    throw new ParserException($"Location \"{name}\" has invalid item override \"{locationParts[4]}\"!");
                }
            }

            Location location = new Location(type, name, dungeon, addresses, defines, dependencies, itemOverride);

            return location;
        }

        /// <summary>
        /// Turn an address string into a file address
        /// </summary>
        /// <param name="addressString">String representing the address</param>
        /// <returns></returns>
        public LocationAddress GetAddressFromString(string addressString)
        {
            // Either direct address or area-room-chest
            if (addressString == "")
            {
                return new LocationAddress(AddressType.None);
            }

            // Get the types of the address
            string[] addressParts = addressString.Split(':');

            AddressType addressType = AddressType.None;
            if (addressParts.Length > 1)
            {
                for (int i = 1; i < addressParts.Length; i++)
                {
                    if (Enum.TryParse(addressParts[i], out AddressType subType))
                    {
                        // Set the flag(s) indicated by the given type
                        addressType |= subType;
                    }
                    else
                    {
                        throw new ShuffleException($"{addressParts[i]} in address {addressString} is not a valid address type!");
                    }
                }
            }

            // If a byte isn't set, default to both
            if ((addressType & AddressType.FirstByte | addressType & AddressType.SecondByte) == AddressType.None)
            {
                // Set the type of the address to the default
                addressType |= AddressType.BothBytes;
            }

            // If the address is an event define, make it an EventLocationAddress
            if ((addressType & AddressType.Define) == AddressType.Define)
            {
                // The first part refers to the name of the define
                return new EventLocationAddress(addressType, addressParts[0]);
            }

            // The address is a number, so it can simply be parsed
            if (StringUtil.ParseString(addressParts[0], out int address))
            {
                return new LocationAddress(addressType, address);
            }

            // The address is an entity, so it should be parsed as an area-room-entity number
            string[] entityDetails = addressParts[0].Split('-');
            if (entityDetails.Length != 3)
            {
                throw new ParserException($"Entity data \"{addressString}\" does not have a full address!");
            }

            if (!StringUtil.ParseString(entityDetails[0], out int area))
            {
                throw new ParserException($"Entity data \"{addressString}\" has an invalid area index!");
            }

            if (!StringUtil.ParseString(entityDetails[1], out int room))
            {
                throw new ParserException($"Entity data \"{addressString}\" has an invalid room index!");
            }

            if (!StringUtil.ParseString(entityDetails[2], out int chest))
            {
                throw new ParserException($"Entity data \"{addressString}\" has an invalid entity index!");
            }

            int addressValue;

            if ((addressType & AddressType.GroundItem) == AddressType.GroundItem)
            {
                throw new NotImplementedException();
            }
            else
            {
                // Look chest address up in table
                int areaTableAddr = ROM.Instance.reader.ReadAddr(ROM.Instance.headers.AreaMetadataBase + (area << 2));
                int roomTableAddr = ROM.Instance.reader.ReadAddr(areaTableAddr + (room << 2));
                int chestTableAddr = ROM.Instance.reader.ReadAddr(roomTableAddr + 0x0C);

                // Chests are 8 bytes long, and the item is stored 2 bytes in
                addressValue = chestTableAddr + chest * 8 + 0x02;
            }


            return new LocationAddress(addressType, addressValue);
        }

        public List<Location> ParseLocations(string[] lines, Random rng)
        {
            List<Location> outList = new List<Location>();
            foreach (string locationLine in lines)
            {
                // Spaces are ignored, and everything after a # is a comment
                string locationString = locationLine.Split('#')[0].Trim();

                // Empty lines or locations are ignored
                if (string.IsNullOrWhiteSpace(locationString))
                {
                    continue;
                }

                if (!SubParser.ShouldIgnoreLines())
                {
                    // Replace defines between `
                    // Probably a more efficient way to do it, but eh
                    if (locationString.IndexOf("`") != -1)
                    {
                        if (locationString.Contains("`RAND_INT`"))
                        {
                            locationString = locationString.Replace("`RAND_INT`", StringUtil.AsStringHex8(rng.Next()));
                        }
                        locationString = SubParser.ReplaceDefines(locationString);
                    }

                    if (locationString[0] == '!')
                    {
                        // Parse the string as a directive, ignoring preparsed directives
                        if (!SubParser.ParseOnLoad(locationString))
                        {
                            try
                            {
                                SubParser.ParseDirective(locationString);
                            }
                            catch(ParserException error)
                            {
                                throw new ParserException($"Error at directive \"{locationString}\": {error.Message}");
                            } 
                        }

                    }
                    else
                    {
                        // Remove spaces as they're ignored in locations
                        locationString = locationString.Replace(" ", "");
                        locationString = locationString.Replace("\t", "");

                        Location newLocation = GetLocation(locationString);
                        outList.Add(newLocation);
                    }
                }
                else
                {
                    // Only parse directives to check for conditionals
                    if (locationString[0] == '!')
                    {
                        SubParser.ParseDirective(locationString);
                    }
                }
            }

            return outList;
        }

        public void PreParse(string[] lines)
        {
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line[0] == '!')
                {
                    // Remove comments and excess whitespace
                    string trimmedLine = line.Split('#')[0].Trim();

                    if (SubParser.ParseOnLoad(trimmedLine))
                    {
                        SubParser.ParseDirective(trimmedLine);
                    }
                }
            }
        }

        public List<EventDefine> GetEventDefines()
        {
            List<EventDefine> usedDefines = new List<EventDefine>();

            usedDefines.AddRange(SubParser.EventDefines);

            return usedDefines;
        }
    }
}
