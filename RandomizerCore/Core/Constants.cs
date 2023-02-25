namespace RandomizerCore.Core;

public enum RegionVersion
{
    Eu,
    Jp,
    Us,
    None
}

public struct HeaderData
{
    public int MapHeaderBase;

    // Name on this is gonna have to change sometime
    public int AreaMetadataBase;

    //added for now, change names to whatever you want
    public int TileOffset;
    public int PaletteSetTableLoc;
    public int Chunk0TableLoc;
    public int Area1Chunk0TableLoc;
    public int Chunk1TableLoc;
    public int Chunk2TableLoc;
    public int SwapBase;
    public int PaletteChangeBase;
    public int Area1SwapBase;
    public int GlobalTileSetTableLoc;
    public int GfxSourceBase;
    public int GlobalMetaTileSetTableLoc;
    public int GlobalTileDataTableLoc;

    public HeaderData(int map, int area, int tileOffset, int paletteSetTableLoc, int c0TableLoc, int a1C0TableLoc,
        int c1TableLoc, int c2TableLoc, int swapBase, int paletteChangeBase, int area1SwapBase,
        int globalTileSetTableLoc, int gfxSourceBase, int globalMetaTileSetTableLoc, int globalTileDataTableLoc)
    {
        MapHeaderBase = map;
        AreaMetadataBase = area;
        this.TileOffset = tileOffset;
        this.PaletteSetTableLoc = paletteSetTableLoc;
        Chunk0TableLoc =
            c0TableLoc; //looks like each next chunkTable is a 0x16 further (eu and us), not adding because of possible jp
        Area1Chunk0TableLoc = a1C0TableLoc;
        Chunk1TableLoc = c1TableLoc; //c0+ 0x16
        Chunk2TableLoc = c2TableLoc; //c0+ 0x32
        this.SwapBase = swapBase;
        this.PaletteChangeBase = paletteChangeBase;
        this.Area1SwapBase = area1SwapBase; //above -0x140?
        this.GlobalTileSetTableLoc = globalTileSetTableLoc;
        this.GfxSourceBase = gfxSourceBase;
        this.GlobalMetaTileSetTableLoc = globalMetaTileSetTableLoc;
        this.GlobalTileDataTableLoc = globalTileDataTableLoc;
    }
}

public enum TileEntityType
{
    None = 0x00,
    TestA = 0x01,
    Chest = 0x02,
    BigChest = 0x03,
    TestB = 0x04,
    TestC = 0x05
}

public enum DungeonEntranceType
{
    Dws = 0x01,
    CoF = 0x02,
    FoW = 0x03,
    ToD = 0x04,
    Crypt = 0x05,
    PoW = 0x06
}

public enum DungeonType
{
    Dojo = 00,
    Dws = 24,
    CoF = 25,
    FoW = 26,
    ToD = 27,
    PoW = 28,
    Dhc = 29,
    Crypt = 30
}

public enum ProgressiveItemType
{
    Sword = 0x0,
    Bow = 0x1,
    Boomerang = 0x2,
    Shield = 0x3,
    SpinAttack = 0x4
}

public enum KinstoneType
{
    UnTyped,

    GoldenCloudTops = 0x65, //YellowTornadoProng
    YellowTornadoSpike = 0x66,
    YellowTornadoChaotic = 0x67,
    //68 and 69 are repeats of above

    GoldenSwamp = 0x6A, //YellowTotemProng
    YellowTotemWave = 0x6B,
    YellowTotemZigZag = 0x6C,

    GoldenFalls = 0x6D, //YellowCrown

    RedW = 0x6E, //RedSpike
    RedV = 0x6F, //RedCrack
    RedE = 0x70, //RedProng

    BlueL = 0x71,
    BlueS = 0x72,

    GreenC = 0x73, //GreenSpike
    GreenG = 0x74, //GreenSquare
    GreenP = 0x75 //GreenSplit
}

public enum ItemType
{
    Untyped,
    SmithSword = 0x01,
    GreenSword = 0x02,
    RedSword = 0x03,
    BlueSword = 0x04,

    ProgressiveItem = 0x05,

    FourSword = 0x06,
    Bombs = 0x07,
    RemoteBombs = 0x08,
    Bow = 0x09,
    LightArrow = 0x0A,
    Boomerang = 0x0B,
    MagicBoomerang = 0x0C,
    Shield = 0x0D,
    MirrorShield = 0x0E,
    Lantern = 0x0F, //LanternOff

    GustJar = 0x11,
    PacciCane = 0x12,
    MoleMitts = 0x13,
    RocsCape = 0x14,
    PegasusBoots = 0x15,
    Firerod = 0x16,
    Ocarina = 0x17,
    GreenClock = 0x18, //GreenOrb
    BlueClock = 0x19, //BlueOrb
    RedClock = 0x1A, //RedOrb
    Trap = 0x1B,
    Bottle1 = 0x1C,
    Bottle2 = 0x1D,
    Bottle3 = 0x1E,
    Bottle = 0x1F, //Bottle4
    BottleEmpty = 0x20,
    BottleButter = 0x21,
    BottleMilk = 0x22,
    BottleHalfMilk = 0x23,
    BottleRedPotion = 0x24,
    BottleBluePotion = 0x25,
    BottleWater = 0x26,
    BottleMineralWater = 0x27,
    BottleFairy = 0x28,
    BottlePicolyteRed = 0x29,
    BottlePicolyteOrange = 0x2A,
    BottlePicolyteYellow = 0x2B,
    BottlePiclolyteGreen = 0x2C,
    BottlePicolyteBlue = 0x2D,
    BottlePicolyteWhite = 0x2E,
    BottleCharmNayru = 0x2F,
    BottleCharmFarore = 0x30,
    BottleCharmDin = 0x31,


    SmithSwordQuest = 0x34,
    BrokenPicoriBlade = 0x35,
    DogFoodBottle = 0x36,
    LonLonKey = 0x37,
    WakeUpMushroom = 0x38,
    RedBook = 0x39, //HyruleanBestiary
    GreenBook = 0x3A, //PicoriLegend
    BlueBook = 0x3B, //MaskHistory
    GraveyardKey = 0x3C,
    TingleTrophy = 0x3D,
    CarlovMedal = 0x3E,
    Shells = 0x3F, //ShellsX
    EarthElement = 0x40,
    FireElement = 0x41,
    WaterElement = 0x42,
    WindElement = 0x43,
    GripRing = 0x44,
    PowerBracelets = 0x45,
    Flippers = 0x46,
    HyruleMap = 0x47,
    SpinAttack = 0x48,
    RollAttack = 0x49,
    DashAttack = 0x4A,
    RockBreaker = 0x4B,
    SwordBeam = 0x4C,
    GreatSpin = 0x4D,
    DownThrust = 0x4E,
    PerilBeam = 0x4F,
    DungeonMap = 0x50,
    Compass = 0x51,
    BigKey = 0x52,
    SmallKey = 0x53,
    Rupee1 = 0x54,
    Rupee5 = 0x55,
    Rupee20 = 0x56,
    Rupee50 = 0x57,
    Rupee100 = 0x58,
    Rupee200 = 0x59,

    JabberNut = 0x5B,
    Kinstone = 0x5C, //KinstoneX
    Bombs5 = 0x5D,
    Arrows5 = 0x5E,
    SmallHeart = 0x5F,
    Fairy = 0x60,
    Shells30 = 0x61,
    HeartContainer = 0x62,
    PieceOfHeart = 0x63,
    Wallet = 0x64,
    BombBag = 0x65,
    LargeQuiver = 0x66,
    Figurine = 0x67, //KinstoneBag
    Brioche = 0x68,
    Croissant = 0x69,
    PieSlice = 0x6A,
    CakeSlice = 0x6B,
    Bombs10 = 0x6C,
    Bombs30 = 0x6D,
    Arrows10 = 0x6E,
    Arrows30 = 0x6F,
    ArrowButterfly = 0x70,
    DigButterfly = 0x71,
    SwimButterfly = 0x72,
    FastSpin = 0x73,
    FastSplit = 0x74,
    LongSpin = 0x75,


    Entrance = 0xF0,
    Music = 0xF1,
    ToDKeys = 0xF2,
    Fusion = 0xF3
}

public class Header
{
    // Will fill out when relevant, only need EU for now
    private readonly HeaderData[] HeaderTable =
    {
        //             MAP     , ENTITY?,	TILEOFFSET	PALETTESET	CHUNK0		CHUNK0AREA1	CHUNK1		CHUNK2		SWAP		PALETTECHANGE	AREA1SWAP	TILESET		GFXSOURCE	METATILE	TILEDATA
        new(0x11D95C, 0x0D4828, 0x5A23D0, 0xFED88, 0x107AEC, 0x1077AC, 0x107B02, 0x107B18, 0x107B5C, 0x107940, 0x107800,
            0x101BC8, 0x323FEC, 0x1027F8, 0x1070E4),
        new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
        new(0x11E214, 0x0D50FC, 0x5A2E80, 0xFF850, 0x108398, 0x108050, 0x1083AE, 0x1083C4, 0x108408, 0x1081E4, 0x1080A4,
            0x10246C, 0x324AE4, 0x10309C, 0x107988)
    };

    public HeaderData GetHeaderAddresses(RegionVersion region)
    {
        return HeaderTable[(int)region];
    }
}
