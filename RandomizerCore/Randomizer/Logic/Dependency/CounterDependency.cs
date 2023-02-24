using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class CounterDependency : DependencyBase
{
    public Dictionary<DependencyBase, int> DependencySet;
    public int RequiredCount;
    private int LastAttemptCount;

    public CounterDependency(Dictionary<DependencyBase, int> dependencySet, int reqValue) : base(true)
    {
        DependencySet = dependencySet;
        RequiredCount = reqValue;
    }

    //This type can never have a not item as a dependent
    public override bool DependencyFulfilled(Item? itemToPlace = null)
    {
        return Result;
    }

    public override void UpdateDependencyResult(bool newResult)
    {
        if (AlreadyEvaluated) return;
        
        AlreadyEvaluated = true;
        
        var counter = 0;

        foreach (var dependency in DependencySet.Keys)
            if (dependency.Result)
            {
                var amountFound = 1;

                if (dependency.GetType() == typeof(ItemDependency))
                    amountFound = ((ItemDependency)dependency).ObtainedTargetItemCounter;

                counter += DependencySet[dependency] * amountFound;
            }

        Result = counter >= RequiredCount;

        LastAttemptCount = counter;

        foreach (var parent in Parents)
            parent.UpdateDependencyResult(Result);

        AlreadyEvaluated = false;
    }

    public override void AddDependencyTargetToDependency(object target)
    {
        //does nothing
    }

    public override void ExpandRequiredDependencies(List<Location.Location> locations, List<Item> items)
    {
        if (DependencySet.Keys.Any(key => key.GetType() == typeof(NotItemDependency)))
            throw new ArgumentException("Counters cannot have not item dependencies as requirements!");

        foreach (var child in DependencySet.Keys)
            child.AddParentDependencyToThisDependency(this);
    }
}
