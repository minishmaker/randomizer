namespace RandomizerCore.Randomizer.Models;

public struct ChanceItem
{
    public Item Item;
    public int Chance;

    public ChanceItem(Item item, int chance)
    {
        this.Item = item;
        this.Chance = chance;
    }
}
