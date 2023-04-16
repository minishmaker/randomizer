using RandomizerCore.Core;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Helpers;

internal static class TransitionFactory
{
    //Entrance locations
    private const uint DeepwoodEntrance1 = 0x1382C2;
    private const uint DeepwoodEntrance2 = 0x138DCA; //This is the entrance from an unused entrance area for ToD
    private const uint DeepwoodEntrance3 = 0x133742; //This is the entrance from minish woods, used in Firerod
    private const uint CofEntrance = 0x133F2E;
    private const uint FowEntrance = 0x133AD6;
    private const uint TodEntrance = 0x13A24A;
    private const uint CryptEntrance = 0x1346F2;
    private const uint PowEntrance = 0x13A4B6;
    private const uint DhcMainEntrance = 0x13460E;
    private const uint DhcSideEntrance1 = 0x1345BE; //This is pre-DHC activated
    private const uint DhcSideEntrance2 = 0x13464A; //This is post-DHC activated

    //Dungeon exits and green warps
    private const uint DwsExit = 0x138172;
    private const uint DwsGreenWarp = 0xDF06C;

    private const uint CofExit = 0x138352;
    private const uint CofGreenWarp = 0xE0F34;

    private const uint FowExit = 0x13549A;
    private const uint FowGreenWarp = 0xE2308;
    private const uint FowAfterElement = 0x13A25E; //FoW has two ways of leaving, one on element, one on green warp

    private const uint TodExit = 0x13A47A;
    private const uint TodGreenWarp = 0xE40F4; //Warps back into the dungeon - we don't actually change this at all

    private const uint CryptExit = 0x138EAA;

    private const uint
        CryptElementWarp = 0x13A2AE; //Crypt doesn't have a green warp, just a warp after getting the item from King

    private const uint PowExit = 0x1082A1;
    private const uint PowGreenWarp = 0xE6A14;


    public static ITransition BuildTransitionFromDungeonEntranceType(DungeonEntranceType destinationEntrance)
    {
        return destinationEntrance switch
        {
            DungeonEntranceType.Dws => new Transition
            {
                ExitX = 0x78,
                ExitY = 0x64,
                EntranceArea = 0x4A,
                EntranceRoom = 0x0,
                Height = 0x1,
                FacingDirection = 0x4,
                TransitionType = 0x0,
                EntranceAddress1 = DeepwoodEntrance1,
                EntranceAddress2 = DeepwoodEntrance2,
                EntranceAddress3 = DeepwoodEntrance3,
                GreenWarpExitCoordinate = 0x01C7,
                ElementGetExitX = 0x78,
                ElementGetExitY = 0x78
            },
            DungeonEntranceType.CoF => new Transition
            {
                ExitX = 0x68,
                ExitY = 0x88,
                EntranceArea = 0x06,
                EntranceRoom = 0x02,
                Height = 0x1,
                FacingDirection = 0x4,
                TransitionType = 0x0,
                EntranceAddress1 = CofEntrance,
                GreenWarpExitCoordinate = 0x0246,
                ElementGetExitX = 0x68,
                ElementGetExitY = 0x98
            },
            DungeonEntranceType.FoW => new Transition
            {
                ExitX = 0x0198,
                ExitY = 0x28,
                EntranceArea = 0x05,
                EntranceRoom = 0x04,
                Height = 0x1,
                FacingDirection = 0x4,
                TransitionType = 0x0,
                EntranceAddress1 = FowEntrance,
                GreenWarpExitCoordinate = 0x0159,
                ElementGetExitX = 0x198,
                ElementGetExitY = 0x68
            },
            DungeonEntranceType.ToD => new ScreenTransition
            {
                ExitX = 0x0128,
                ExitY = 0x0188,
                EntranceArea = 0x0B,
                EntranceRoom = 0x0,
                Height = 0x1,
                FacingDirection = 0x4,
                TransitionType = 0x9,
                EntranceAddress1 = TodEntrance,
                GreenWarpExitCoordinate = 0x0692,
                ElementGetExitX = 0x0128,
                ElementGetExitY = 0x01A8,
                EntranceType = DungeonEntranceType.ToD
            },
            DungeonEntranceType.Crypt => new Transition
            {
                ExitX = 0xF0,
                ExitY = 0x3C,
                EntranceArea = 0x09,
                EntranceRoom = 0x0,
                Height = 0x1,
                FacingDirection = 0x4,
                TransitionType = 0x0,
                EntranceAddress1 = CryptEntrance,
                GreenWarpExitCoordinate = 0x024E,
                ElementGetExitX = 0xE8,
                ElementGetExitY = 0x98
            },
            DungeonEntranceType.PoW => new ScreenTransition
            {
                ExitX = 0x78,
                ExitY = 0x68,
                EntranceArea = 0x31,
                EntranceRoom = 0x0,
                Height = 0x1,
                FacingDirection = 0x4,
                TransitionType = 0x2,
                EntranceAddress1 = PowEntrance,
                GreenWarpExitCoordinate = 0x01C7,
                ElementGetExitX = 0x78,
                ElementGetExitY = 0x78,
                EntranceType = DungeonEntranceType.PoW
            },
            DungeonEntranceType.DHCMain => new Transition
            {
                ExitX = 0x1F8,
                ExitY = 0x38,
                EntranceArea = 0x07,
                EntranceRoom = 0x00,
                Height = 0x1,
                FacingDirection = 0x4,
                TransitionType = 0x0,
                EntranceAddress1 = DhcMainEntrance,
                GreenWarpExitCoordinate = 0x00DF,
                ElementGetExitX = 0x1F8,
                ElementGetExitY = 0x38
            },
            DungeonEntranceType.DHCSide => new Transition
            {
                ExitX = 0x68,
                ExitY = 0x58,
                EntranceArea = 0x07,
                EntranceRoom = 0x00,
                Height = 0x1,
                FacingDirection = 0x0,
                TransitionType = 0x0,
                EntranceAddress1 = DhcSideEntrance1,
                EntranceAddress2 = DhcSideEntrance2,
                GreenWarpExitCoordinate = 0x0146,
                ElementGetExitX = 0x68,
                ElementGetExitY = 0x58
            },
            _ => throw new ShuffleException("Inavlid dungeon entrance type parsed!")
        };
    }
}
