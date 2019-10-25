using System.Collections.Generic;

namespace MinishRandomizer.Randomizer.Logic
{
    public class Dependency
    {
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

        public AndDependency(List<Dependency> dependencyList)
        {
            AndList = dependencyList;
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

        public OrDependency(List<Dependency> dependencyList)
        {
            OrList = dependencyList;
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

    public class CounterDependency : Dependency
    {
        public Dictionary<Dependency, int> DependencySet;
        public int RequiredCount = 0;
        public CounterDependency(Dictionary<Dependency, int> dependencySet, int reqValue)
        {
            DependencySet = dependencySet;
            RequiredCount = reqValue;
        }

        public override bool DependencyFulfilled(List<Item> availableItems, List<Location> locations)
        {
            var counter = 0;

            foreach (Dependency dependency in DependencySet.Keys)
            {
                if (dependency.DependencyFulfilled(availableItems, locations) == true)
                {
                    var amountFound = 1;

                    if (dependency.GetType() == typeof(ItemDependency))
                    {
                        amountFound = availableItems.FindAll(i => dependency.DependencyFulfilled(new List<Item>(new Item[1] { i }), locations)).Count;
                    }

                    counter += DependencySet[dependency] * amountFound;
                }
            }
            return counter >= RequiredCount;
        }
    }
}
