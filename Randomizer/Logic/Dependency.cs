using MinishRandomizer.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MinishRandomizer.Randomizer.Logic
{
    public class Dependency
    {
        /// <summary>
        /// Get a list of the dependencies contained within a given logic string. Compound dependencies call this recursively
        /// </summary>
        /// <param name="logic">The logic string to parse</param>
        /// <returns>The list of dependencies contained in the logic</returns>
        public static List<Dependency> GetDependencies(string logic)
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
                        AndDependency andDependency = new AndDependency(sequence.Substring(1));
                        dependencies.Add(andDependency);
                        break;
                    case '|':
                        OrDependency orDependency = new OrDependency(sequence.Substring(1));
                        dependencies.Add(orDependency);
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
                                if (!int.TryParse(splitSequence[2], out count))
                                {
                                    count = 1;
                                }
                            }
                        }

                        string[] dependencyParts = requirement.Split('.');

                        if (dependencyParts.Length < 2)
                        {
                            break;
                        }

                        switch (dependencyParts[0])
                        {
                            case "Locations":
                            case "Helpers":
                                LocationDependency locationDependency = new LocationDependency(dependencyParts[1]);
                                dependencies.Add(locationDependency);
                                break;
                            case "Items":
                                if (Enum.TryParse(dependencyParts[1], out ItemType type))
                                {
                                    byte subType = 0;
                                    if (dependencyParts.Length >= 3)
                                    {
                                        if (!byte.TryParse(dependencyParts[2], NumberStyles.HexNumber, null, out subType))
                                        {
                                            if (Enum.TryParse(dependencyParts[2], out KinstoneType subKinstoneType))
                                            {
                                                subType = (byte)subKinstoneType;
                                            }
                                        }
                                    }

                                    ItemDependency itemDependency = new ItemDependency(new Item(type, subType, dungeon), count);
                                    dependencies.Add(itemDependency);
                                }
                                else
                                {
                                    throw new ShuffleException($"Item {dependencyParts[1]} could not be found!");
                                }
                                break;
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
        public static List<string> SplitDependencies(string logic)
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
                throw new ShuffleException($"Parentheses could not be parsed correctly! Make sure none of them are mismatched.");
            }

            return subLogic;
        }

        public virtual bool DependencyFulfilled(List<Item> availableItems, List<Location> locations)
        {
            return false;
        }

    }

    public class ItemDependency : Dependency
    {
        private Item RequiredItem;
        private int Count;
        public ItemDependency(Item item, int count = 1)
        {
            RequiredItem = item;
            Count = count;
        }

        public override bool DependencyFulfilled(List<Item> availableItems, List<Location> locations)
        {
            int counter = 0;
            foreach (Item item in availableItems)
            {
                if (item.Type == RequiredItem.Type && item.SubValue == RequiredItem.SubValue && (RequiredItem.Dungeon == "" || RequiredItem.Dungeon == item.Dungeon))
                {
                    counter++;
                    if (counter >= Count)
                    {
                        return true;
                    }
                }
            }

            //Console.WriteLine($"Missing {RequiredItem.Type}");

            return false;
        }
    }

    public class LocationDependency : Dependency
    {
        private string RequiredLocationName;
        public LocationDependency(string locationName)
        {
            RequiredLocationName = locationName;
        }

        public override bool DependencyFulfilled(List<Item> availableItems, List<Location> locations)
        {
            foreach (Location location in locations)
            {
                if (location.Name == RequiredLocationName)
                {

                    return location.IsAccessible(availableItems, locations);
                }
            }
            throw new ShuffleException($"Could not find location {RequiredLocationName}. You may have an invalid logic file!");
        }
    }

    public class AndDependency : Dependency
    {
        public List<Dependency> AndList;

        public AndDependency(string dependencyText)
        {
            AndList = GetDependencies(dependencyText);
        }

        public override bool DependencyFulfilled(List<Item> availableItems, List<Location> locations)
        {
            foreach (Dependency dependency in AndList)
            {
                if (dependency.DependencyFulfilled(availableItems, locations) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class OrDependency : Dependency
    {
        public List<Dependency> OrList;

        public OrDependency(string dependencyText)
        {
            OrList = GetDependencies(dependencyText);
        }

        public override bool DependencyFulfilled(List<Item> availableItems, List<Location> locations)
        {
            foreach (Dependency dependency in OrList)
            {
                if (dependency.DependencyFulfilled(availableItems, locations) == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
