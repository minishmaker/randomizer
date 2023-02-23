using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public abstract class DependencyBase
{
    protected readonly List<DependencyBase> Parents;

    protected bool AlreadyEvaluated = false;

    protected DependencyBase(bool supportsCaching)
    {
        Parents = new List<DependencyBase>();
        SupportsCaching = supportsCaching;
        Result = false;
    }

    public static DependencyBase? BeatVaatiDependency { get; set; } = null;

    internal bool Result { get; set; }

    public bool SupportsCaching { get; protected set; }

    public abstract bool DependencyFulfilled(Item? itemToPlace = null);

    public abstract void UpdateDependencyResult(bool newResult);

    public abstract void AddDependencyTargetToDependency(object target);

    public abstract void ExpandRequiredDependencies(List<Location.Location> locations, List<Item> items);

    public void AddParentDependencyToThisDependency(DependencyBase parent)
    {
        Parents.Add(parent);
    }

    protected void RemoveCachingSupportFromParents()
    {
        if (AlreadyEvaluated) 
            return;

        AlreadyEvaluated = true;
        
        foreach (var parent in Parents)
        {
            parent.SupportsCaching = false;
            parent.RemoveCachingSupportFromParents();
        }

        AlreadyEvaluated = false;
    }
}
