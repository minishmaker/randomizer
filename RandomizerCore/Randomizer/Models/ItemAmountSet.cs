namespace RandomizerCore.Randomizer.Models;

public class ItemAmountSet
{
    public int amount;
    public Item item;

    public ItemAmountSet(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public ItemAmountSet Clone()
    {
        return new ItemAmountSet(item, amount);
    }
}