using MinishRandomizer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace MinishRandomizer.Randomizer
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

            logic = logic.Replace(" ", "");

            // Match: comma, capture and match anything between 
            string regexPattern = @"(?:,|^)\(([&|](?:[^()]|(?<p>\()|(?<-p>\)))+)\)(?:,|$)|,";
            string[] subLogic = Regex.Split(logic, regexPattern);
            foreach (string sequence in subLogic)
            {
                if (string.IsNullOrEmpty(sequence))
                {
                    continue;
                }


                switch(sequence[0])
                {
                    // If the first character of the string is & or |, it's a compound dependency
                    case '&':
                        AndDependency andDependency = new AndDependency(sequence.Substring(1));
                        dependencies.Add(andDependency);
                        break;
                    case '|':
                        OrDependency orDependency = new OrDependency(sequence.Substring(1));
                        dependencies.Add(orDependency);
                        break;
                    default:
                        string[] dependencyParts = sequence.Split('.');
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

                                    ItemDependency itemDependency = new ItemDependency(new Item(type, subType));
                                    dependencies.Add(itemDependency);
                                }
                                break;
                        }
                        break;
                }
            }

            return dependencies;
        }

        public virtual bool DependencyFulfilled(List<Item> availableItems, List<Location> locations)
        {
            return false;
        }

    }

    public class ItemDependency : Dependency
    {
        private Item RequiredItem;
        public ItemDependency(Item item)
        {
            RequiredItem = item;
        }

        public override bool DependencyFulfilled(List<Item> availableItems, List<Location> locations)
        {
            foreach (Item item in availableItems)
            {
                if (item.Type == RequiredItem.Type && item.SubValue == RequiredItem.SubValue)
                {
                    Console.WriteLine($"Has: {item.Type.ToString()} Needs: {RequiredItem.Type.ToString()}");
                    return true;
                }
            }

            Console.WriteLine($"Inccessible because: {RequiredItem.Type.ToString()} is not available!");
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
            Console.WriteLine($"Could not find location: {RequiredLocationName}");
            return false;
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
