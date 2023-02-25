namespace RandomizerCore.Randomizer.Models;

public struct ChanceItemSet
{
    public List<ChanceItem> RandomItems;
    public int TotalChance;

    public ChanceItemSet(List<ChanceItem> randomItems)
    {
        this.RandomItems = randomItems;
        TotalChance = 0;

        foreach (var randomItem in randomItems) TotalChance += randomItem.Chance;
    }
}
