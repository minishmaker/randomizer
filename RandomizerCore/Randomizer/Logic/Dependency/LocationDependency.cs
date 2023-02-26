using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class LocationDependency : DependencyBase
{
    internal readonly string LocationName;
    private List<DependencyBase> LocationDependencies;

    public LocationDependency(string locationName) : base(true)
    {
        if (locationName == "BeatVaati") 
            BeatVaatiDependency = this;

        LocationName = locationName;
        LocationDependencies = new List<DependencyBase>();
    }

    public override bool DependencyFulfilled(Item? itemToPlace = null)
    {
        if (AlreadyEvaluated) return false;
        
        AlreadyEvaluated = true;

        try
        {
            return SupportsCaching ? Result : LocationDependencies.All(dependency => dependency.DependencyFulfilled(itemToPlace));
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

        Result = newResult && LocationDependencies.All(child => child.Result);

        foreach (var parent in Parents)
            parent.UpdateDependencyResult(Result);

        AlreadyEvaluated = false;
    }

    public override void AddDependencyTargetToDependency(object target)
    {
        //Does nothing
    }

    public override void ExpandRequiredDependencies(List<Location.Location> locations, List<Item> items)
    {
        var location = locations.FirstOrDefault(location => location.Name == LocationName);

        if (location == null) throw new ParserException($"Location of name {LocationName} could not be found!");

        location.RelatedDependencies.Add(this);
        LocationDependencies = location.Dependencies;

        Result = LocationDependencies.All(dep => dep.DependencyFulfilled());

        foreach (var parent in Parents)
            parent.UpdateDependencyResult(Result);

        foreach (var dependency in LocationDependencies)
        {
            dependency.AddParentDependencyToThisDependency(this);

            if (!dependency.SupportsCaching) SupportsCaching = false;
        }
    }
}
