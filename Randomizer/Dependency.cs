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
                        
                        break;
                }
            }

            return dependencies;
        }

        private Item RequiredItem;

        public Dependency()
        {

        }

        public Dependency(string dependencyText)
        {
            string[] dependencyParts = dependencyText.Split('.');
            switch(dependencyParts[0].ToLower())
            {
                case "locations":

                    break;
                case "items":
                    if (Enum.TryParse(dependencyParts[1], out ItemType type))
                    {
                        byte subType = 0;
                        if (dependencyParts.Length >= 2)
                        {
                            if (!byte.TryParse(dependencyParts[2], NumberStyles.HexNumber, null, out subType))
                            {
                                if (Enum.TryParse(dependencyParts[2], out KinstoneType subKinstoneType))
                                {
                                    subType = (byte)subKinstoneType;
                                }
                            }
                        }
                        RequiredItem = new Item(type, subType);
                    }
                    else
                    {
                        RequiredItem = new Item(ItemType.Untyped, 0);
                    }
                    break;
            }
        }

        public virtual bool DependencyFulfilled(List<Item> unplacedItems)
        {
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

        override public bool DependencyFulfilled(List<Item> unplacedItems)
        {
            foreach (Dependency dependency in AndList)
            {
                if (dependency.DependencyFulfilled(unplacedItems) == false)
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

        override public bool DependencyFulfilled(List<Item> unplacedItems)
        {
            foreach (Dependency dependency in OrList)
            {
                if (dependency.DependencyFulfilled(unplacedItems) == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
