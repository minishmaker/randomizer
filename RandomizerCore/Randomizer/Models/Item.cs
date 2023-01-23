using RandomizerCore.Core;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Logic;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer.Models;

public readonly struct Item
{
    public readonly ItemType Type;
    public readonly KinstoneType Kinstone;
    public readonly ItemPool ShufflePool;
    public readonly byte SubValue;
    public readonly string Dungeon;
    public readonly bool UseAny;

    public Item(string data, string commandScope = "", ItemPool shufflePool = ItemPool.Unshuffled)
    {
        var dataChunks = data.Split(':');
        var itemData = dataChunks[0].Split('.');
        if (itemData[0].TrimStart(' ').TrimEnd(' ') != "Items")
            throw new ParserException(
                $"{commandScope}: \"{data}\" is not an item, make sure it has \"Items.\" prepended");
        if (!Enum.TryParse(itemData[1], out Type))
            throw new ParserException($"{commandScope}: \"{data}\" has an invalid itemType");

        UseAny = false;
        SubValue = 0;
        if (itemData.Length >= 3)
        {
            if (itemData[2] == "*")
            {
                UseAny = true;
            }
            else if (!StringUtil.ParseString(itemData[2], out SubValue))
            {
                if (Enum.TryParse(itemData[2], out Kinstone))
                    SubValue = (byte)Kinstone;
                else
                    throw new ParserException($"{commandScope}: \"{data}\" has an invalid itemSub");
            }
        }

        Dungeon = "";
        // if (dataChunks.Length > 1) Dungeon = dataChunks[1];

        if (Type == ItemType.Kinstone)
            Kinstone = (KinstoneType)SubValue;
        else
            Kinstone = KinstoneType.UnTyped;

        ShufflePool = shufflePool;
    }

    public Item(ItemType type, byte subValue, string dungeon = "", bool useAny = false, ItemPool shufflePool = ItemPool.Unshuffled)
    {
        Type = type;
        SubValue = subValue;
        UseAny = useAny;
        if (type == ItemType.Kinstone)
            Kinstone = (KinstoneType)subValue;
        else
            Kinstone = KinstoneType.UnTyped;

        Dungeon = dungeon;

        ShufflePool = shufflePool;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;
        var asItem = (Item)obj;
        return asItem.Type == Type && (asItem.SubValue == SubValue || asItem.UseAny || UseAny);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return Type + "." + SubValue + ":" + Dungeon;
    }
}