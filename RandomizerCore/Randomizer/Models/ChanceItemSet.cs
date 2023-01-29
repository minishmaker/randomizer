namespace RandomizerCore.Randomizer.Models;

public struct ChanceItemSet
{
    public List<ChanceItem> randomItems;
    public int totalChance;

    public ChanceItemSet(List<ChanceItem> randomItems)
    {
        this.randomItems = randomItems;
        totalChance = 0;

        foreach (var randomItem in randomItems) totalChance += randomItem.chance;
    }
}