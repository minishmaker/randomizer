namespace RandomizerCore.Randomizer.Models;

public struct ChanceItem
{
    public Item item;
    public int chance;

    public ChanceItem(Item item, int chance)
    {
        this.item = item;
        this.chance = chance;
    }
}