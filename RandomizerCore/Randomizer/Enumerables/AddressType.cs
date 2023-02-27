namespace RandomizerCore.Randomizer.Enumerables;

[Flags]
public enum AddressType
{
    None = 0,
    FirstByte = 1,
    SecondByte = 2,
    BothBytes = FirstByte | SecondByte,
    GroundItem = 4,
    Define = 8
}
