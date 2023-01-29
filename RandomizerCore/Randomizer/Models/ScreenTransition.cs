using RandomizerCore.Core;

namespace RandomizerCore.Randomizer.Models
{
    internal class ScreenTransition : Transition
    {
        public DungeonEntranceType EntranceType { get; set; }

        public override Dictionary<uint, object> GetTransitionOffsets(byte targetArea, byte targetRoom, byte layerOrHeight, byte animation, ushort entranceX, ushort entranceY, byte facingDirection, uint entranceAddress = 0, uint greenWarpAddress = 0, uint elementGetAddress = 0, uint holeTransitionAddress = 0)
        {
            var addressValuePairs = new Dictionary<uint, object>();

            if (entranceAddress != 0)
            {
                addressValuePairs.Add(entranceAddress, ExitX);
                addressValuePairs.Add(entranceAddress + 0x2, ExitY);
                addressValuePairs.Add(entranceAddress + 0x5, EntranceArea);
                addressValuePairs.Add(entranceAddress + 0x6, EntranceRoom);
                addressValuePairs.Add(entranceAddress + 0x7, Height);
                addressValuePairs.Add(entranceAddress + 0x8, TransitionType);
                addressValuePairs.Add(entranceAddress + 0x9, FacingDirection);
            }

            if (greenWarpAddress != 0)
            {
                addressValuePairs.Add(greenWarpAddress, EntranceArea);
                addressValuePairs.Add(greenWarpAddress + 0x1, EntranceRoom);
                addressValuePairs.Add(greenWarpAddress + 0x8, GreenWarpExitCoordinate);
            }

            if (elementGetAddress != 0)
            {
                addressValuePairs.Add(elementGetAddress, ElementGetExitX);
                addressValuePairs.Add(elementGetAddress + 0x2, ElementGetExitY);
                addressValuePairs.Add(elementGetAddress + 0x5, EntranceArea);
                addressValuePairs.Add(elementGetAddress + 0x6, EntranceRoom);
                addressValuePairs.Add(elementGetAddress + 0x7, Height);
                addressValuePairs.Add(elementGetAddress + 0x8, TransitionType);
                addressValuePairs.Add(elementGetAddress + 0x9, FacingDirection);
            }

            if (holeTransitionAddress != 0)
            {
                addressValuePairs.Add(holeTransitionAddress, EntranceArea);
                addressValuePairs.Add(holeTransitionAddress + 0x1, EntranceRoom);
                addressValuePairs.Add(holeTransitionAddress + 0x2, 1); //Layer is always 1
                addressValuePairs.Add(holeTransitionAddress + 0x3, ElementGetExitX);
                addressValuePairs.Add(holeTransitionAddress + 0x5, ElementGetExitY);
            }

            addressValuePairs.Add(EntranceAddress1, entranceX);
            addressValuePairs.Add(EntranceAddress1 + 0x2, entranceY);
            addressValuePairs.Add(EntranceAddress1 + 0x5, targetArea);
            addressValuePairs.Add(EntranceAddress1 + 0x6, targetRoom);
            addressValuePairs.Add(EntranceAddress1 + 0x7, layerOrHeight);
            addressValuePairs.Add(EntranceAddress1 + 0x8, animation);
            addressValuePairs.Add(EntranceAddress1 + 0x9, facingDirection);

            return addressValuePairs;
        }
    }
}
