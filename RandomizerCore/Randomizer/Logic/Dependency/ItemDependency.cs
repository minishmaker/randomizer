using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Dependency;

public class ItemDependency : DependencyBase
{
    private readonly List<Item> ChildItems;
    private readonly int Count;
    private readonly Item RequiredItem;

    public ItemDependency(Item item, int count = 1) : base(true)
    {
        RequiredItem = item;
        Count = count;
        ObtainedTargetItemCounter = 0;
        ChildItems = new List<Item>();
    }

    internal int ObtainedTargetItemCounter { get; private set; }

    public override bool DependencyFulfilled(Item? itemToPlace = null)
    {
        return Result;
    }

    public override void UpdateDependencyResult(bool newResult)
    {
        if (AlreadyEvaluated) return;

        AlreadyEvaluated = true;
        
        ObtainedTargetItemCounter += newResult ? 1 : -1;

        Result = ObtainedTargetItemCounter >= Count;

        foreach (var parent in Parents)
            parent.UpdateDependencyResult(Result);

        AlreadyEvaluated = false;
    }

    public override void AddDependencyTargetToDependency(object target)
    {
        if (target.GetType() != typeof(Item)) return;

        var i = (Item)target;

        i.AddParentDependency(this);
        ChildItems.Add(i);
    }

    public override void ExpandRequiredDependencies(List<Location.Location> locations, List<Item> items)
    {
        foreach (var item in items)
            if (item.Type == RequiredItem.Type &&
                (item.SubValue == RequiredItem.SubValue || item.UseAny || RequiredItem.UseAny) &&
                (RequiredItem.Dungeon == "" || RequiredItem.Dungeon == item.Dungeon))
            {
                item.AddParentDependency(this);
                ChildItems.Add(item);
            }
    }
}
