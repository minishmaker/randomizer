using RandomizerCore.Utilities.Extensions;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Utilities.Models;

public class Patch
{
    //Specified in the BPS File Format
    public static string BPSMagicString = "BPS1";

    public int SourceSize { get; set; }
    
    public int PatchedSize { get; set; }
    
    public List<BPSExportAction> ExportActions { get; set; }
    
    public uint SourceChecksum { get; set; }
    
    public uint PatchedChecksum { get; set; }
    
    public uint PatchChecksum { get; set; }

    public static Patch BuildPatchFromContentBytes(byte[] content)
    {
        var patch = new Patch();
        var offset = 4; //Ignore the magic string at the beginning
        
        patch.SourceSize = (int)BPSPatcher.ReadVLN(content, ref offset);
        patch.PatchedSize = (int)BPSPatcher.ReadVLN(content, ref offset);
        
        var metadataLength = (int)BPSPatcher.ReadVLN(content, ref offset);
        offset += metadataLength; //Ignore metadata
        
        var checksumBeginOffset = content.Length - 12;

        var actions = new List<BPSExportAction>();

        while (offset < checksumBeginOffset)
        {
            var data = (int)BPSPatcher.ReadVLN(content, ref offset);
            var action = new BPSExportAction
            {
                OperandType = (BPSPatcher.BPSOperandType)(data & 0x3),
                ActionLength = (data >> 2) + 1,
            };
            
            switch (action.OperandType)
            {
                case BPSPatcher.BPSOperandType.TargetRead:
                    action.Bytes = BPSPatcher.ReadBytes(content, action.ActionLength, ref offset);
                    break;
                case BPSPatcher.BPSOperandType.SourceCopy or BPSPatcher.BPSOperandType.TargetCopy:
                    var relOffset = BPSPatcher.ReadVLN(content, ref offset);
                    action.RelativeOffset = (int)(((relOffset & 1) != 0 ? -1 : 1) * (relOffset >> 1));
                    break;
            }
            
            actions.Add(action);
        }

        patch.ExportActions = actions;
        
        patch.SourceChecksum = content.ByteArrayToUintLE(ref offset);
        offset += 4;
        patch.PatchedChecksum = content.ByteArrayToUintLE(ref offset);
        offset += 4;
        patch.PatchChecksum = content.ByteArrayToUintLE(ref offset);
        
        return patch;
    }
    
    public byte[] CreatePatchData()
    {
        //Encode the magic string first
        var data = new List<byte>();
        data.AddRange(BPSMagicString.ToCharArray().Select(c => (byte)c));
        
        BPSPatcher.WriteVLN(SourceSize, ref data);
        BPSPatcher.WriteVLN(PatchedSize, ref data);
        data.Add(0x80); //We don't include any metadata in the patch, so we encode 0

        foreach (var action in ExportActions)
        {
            BPSPatcher.WriteVLN(((action.ActionLength - 1) << 2) + (int)action.OperandType, ref data);
            switch (action.OperandType)
            {
                case BPSPatcher.BPSOperandType.TargetRead:
                    data.AddRange(action.Bytes);
                    break;
                case BPSPatcher.BPSOperandType.SourceCopy or BPSPatcher.BPSOperandType.TargetCopy:
                    BPSPatcher.WriteVLN((Math.Abs(action.RelativeOffset) << 1) + 
                        (action.RelativeOffset < 0 ? 1 : 0), ref data);
                    break;
            }
        }
        
        data.AddRange(SourceChecksum.UintToByteArrayLE());
        data.AddRange(PatchedChecksum.UintToByteArrayLE());

        PatchChecksum = data.ToArray().Crc32(); //Create checksum of the rest of the patch
        data.AddRange(PatchChecksum.UintToByteArrayLE());

        return data.ToArray();
    }
}