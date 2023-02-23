namespace RandomizerCore.Randomizer.Models;

public class ItemAmountSet
{
    public int Amount;
    public Item Item;

    public ItemAmountSet(Item item, int amount)
    {
        this.Item = item;
        this.Amount = amount;
    }

    public ItemAmountSet Clone()
    {
        return new ItemAmountSet(Item, Amount);
    }
}
