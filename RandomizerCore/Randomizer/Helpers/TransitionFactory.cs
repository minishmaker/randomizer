using RandomizerCore.Core;
using RandomizerCore.Randomizer.Exceptions;
using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Helpers
{
    internal static class TransitionFactory
    {
        //Entrance locations
        private const uint _deepwoodEntrance1 = 0x1382C2;
        private const uint _deepwoodEntrance2 = 0x138DCA; //This is the entrance from an unused entrance area for ToD
        private const uint _deepwoodEntrance3 = 0x133742; //This is the entrance from minish woods, used in Firerod
        private const uint _cofEntrance = 0x133F2E;
        private const uint _fowEntrance = 0x133AD6;
        private const uint _todEntrance = 0x13A24A;
        private const uint _cryptEntrance = 0x1346F2;
        private const uint _powEntrance = 0x13A4B6;

        //Dungeon exits and green warps
        private const uint dwsExit = 0x138172;
        private const uint dwsGreenWarp = 0xDF06C;

        private const uint cofExit = 0x138352;
        private const uint cofGreenWarp = 0xE0F34;

        private const uint fowExit = 0x13549A;
        private const uint fowGreenWarp = 0xE2308;
        private const uint fowAfterElement = 0x13A25E; //FoW has two ways of leaving, one on element, one on green warp

        private const uint todExit = 0x13A47A;
        private const uint todGreenWarp = 0xE40F4; //Warps back into the dungeon - we don't actually change this at all

        private const uint cryptExit = 0x138EAA;
        private const uint cryptElementWarp = 0x13A2AE; //Crypt doesn't have a green warp, just a warp after getting the item from King

        private const uint powExit = 0x1082A1;
        private const uint powGreenWarp = 0xE6A14;

        public static ITransition BuildTransitionFromDungeonEntranceType(DungeonEntranceType destinationEntrance) 
        {
            return destinationEntrance switch
            {
                DungeonEntranceType.DWS => new Transition
                {
                    ExitX = 0x78,
                    ExitY = 0x64,
                    EntranceArea = 0x4A,
                    EntranceRoom = 0x0,
                    Height = 0x1,
                    FacingDirection = 0x4,
                    TransitionType = 0x0,
                    EntranceAddress1 = _deepwoodEntrance1,
                    EntranceAddress2 = _deepwoodEntrance2,
                    EntranceAddress3 = _deepwoodEntrance3,
                    GreenWarpExitCoordinate = 0x01C7,
                    ElementGetExitX = 0x78,
                    ElementGetExitY = 0x78,
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
                    EntranceAddress1 = _cofEntrance,
                    GreenWarpExitCoordinate = 0x0246,
                    ElementGetExitX = 0x68,
                    ElementGetExitY = 0x98,
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
                    EntranceAddress1 = _fowEntrance,
                    GreenWarpExitCoordinate = 0x0159,
                    ElementGetExitX = 0x198,
                    ElementGetExitY = 0x68,
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
                    EntranceAddress1 = _todEntrance,
                    GreenWarpExitCoordinate = 0x0692,
                    ElementGetExitX = 0x0128,
                    ElementGetExitY = 0x01A8,
                    EntranceType = DungeonEntranceType.ToD,
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
                    EntranceAddress1 = _cryptEntrance,
                    GreenWarpExitCoordinate = 0x024E,
                    ElementGetExitX = 0xF0,
                    ElementGetExitY = 0xBC,
                },
                DungeonEntranceType.PoW => new ScreenTransition
                {
                    ExitX = 0x78,
                    ExitY = 0x68,
                    EntranceArea = 0x31,
                    EntranceRoom = 0x0,
                    Height = 0x1,
                    FacingDirection = 0x0,
                    TransitionType = 0x2,
                    EntranceAddress1 = _powEntrance,
                    GreenWarpExitCoordinate = 0x01C7,
                    ElementGetExitX = 0x78,
                    ElementGetExitY = 0x78,
                    EntranceType = DungeonEntranceType.PoW,
                },
                _ => throw new ShuffleException("Inavlid dungeon entrance type parsed!")
            };
        }
    }
}
