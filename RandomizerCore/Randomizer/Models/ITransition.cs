using RandomizerCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomizerCore.Randomizer.Models
{
    internal interface ITransition
    {
        /// <summary>
        /// Builds the dictionary of addresses and values for the given transition.
        /// </summary>
        /// <param name="targetArea"></param>
        /// <param name="targetRoom"></param>
        /// <param name="layerOrHeight"></param>
        /// <param name="animation"></param>
        /// <param name="entranceX"></param>
        /// <param name="entranceY"></param>
        /// <param name="facingDirection"></param>
        /// <param name="entranceAddress">The address to write exit data to. Address should start at "Destination X"</param>
        /// <param name="greenWarpAddress">The address to write green warp data to. Address should start at the entity offset in ROM + 0x7C</param>
        /// <param name="elementGetAddress">The address to write exit data to. Address should start at "Destination X"</param>
        /// <param name="holeTransitionAddress">The address to write exit data to for falling down a hole. Address should start at "unk_01"</param>
        /// <returns></returns>
        Dictionary<uint, object> GetTransitionOffsets(byte targetArea, byte targetRoom, byte layerOrHeight, byte animation, ushort entranceX, ushort entranceY, byte facingDirection, uint entranceAddress = 0, uint greenWarpAddress = 0, uint elementGetAddress = 0, uint holeTransitionAddress = 0);
    }
}
