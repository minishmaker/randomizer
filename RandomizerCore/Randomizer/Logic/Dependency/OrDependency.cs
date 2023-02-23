using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class OrDependency : DependencyBase
{
    private readonly List<DependencyBase> ChildDependencies;

    public OrDependency(List<DependencyBase> dependencyList) : base(true)
    {
        ChildDependencies = dependencyList;

        foreach (var child in ChildDependencies)
            child.AddParentDependencyToThisDependency(this);
    }

    public override bool DependencyFulfilled(Item? itemToPlace = null)
    {
        if (AlreadyEvaluated) return false;
        
        AlreadyEvaluated = true;

        try
        {
            return SupportsCaching ? Result : ChildDependencies.Any(child => child.DependencyFulfilled(itemToPlace));
        }
        finally
        {
            AlreadyEvaluated = false;
        }
    }

    public override void UpdateDependencyResult(bool newResult)
    {
        if (!SupportsCaching || AlreadyEvaluated) return;

        AlreadyEvaluated = true;

        Result = newResult || ChildDependencies.Any(child => child.Result);

        foreach (var parent in Parents)
            parent.UpdateDependencyResult(Result);

        AlreadyEvaluated = false;
    }

    public override void AddDependencyTargetToDependency(object target)
    {
        if (target.GetType() != typeof(DependencyBase) && target.GetType().BaseType != typeof(DependencyBase)) return;

        var dep = (DependencyBase)target;

        dep.AddParentDependencyToThisDependency(this);
        ChildDependencies.Add(dep);
        UpdateDependencyResult(dep.Result);
    }

    public override void ExpandRequiredDependencies(List<Location.Location> locations, List<Item> items)
    {
        //does nothing
    }
}
