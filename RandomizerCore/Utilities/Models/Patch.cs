using RandomizerCore.Utilities.Extensions;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Utilities.Models;

public class Patch
{
    //Specified in the BPS File Format
    public static string BpsMagicString = "BPS1";

    public int SourceSize { get; set; }

    public int PatchedSize { get; set; }

    public List<BpsExportAction>? ExportActions { get; set; }

    public uint SourceChecksum { get; set; }

    public uint PatchedChecksum { get; set; }

    public uint PatchChecksum { get; set; }

    public static Patch BuildPatchFromContentBytes(byte[] content)
    {
        var patch = new Patch();
        var offset = 4; //Ignore the magic string at the beginning

        patch.SourceSize = (int)BpsPatcher.ReadVln(content, ref offset);
        patch.PatchedSize = (int)BpsPatcher.ReadVln(content, ref offset);

        var metadataLength = (int)BpsPatcher.ReadVln(content, ref offset);
        offset += metadataLength; //Ignore metadata

        var checksumBeginOffset = content.Length - 12;

        var actions = new List<BpsExportAction>();

        while (offset < checksumBeginOffset)
        {
            var data = (int)BpsPatcher.ReadVln(content, ref offset);
            var action = new BpsExportAction
            {
                OperandType = (BpsPatcher.BpsOperandType)(data & 0x3),
                ActionLength = (data >> 2) + 1
            };

            switch (action.OperandType)
            {
                case BpsPatcher.BpsOperandType.TargetRead:
                    action.Bytes = BpsPatcher.ReadBytes(content, action.ActionLength, ref offset);
                    break;
                case BpsPatcher.BpsOperandType.SourceCopy or BpsPatcher.BpsOperandType.TargetCopy:
                    var relOffset = BpsPatcher.ReadVln(content, ref offset);
                    action.RelativeOffset = (int)(((relOffset & 1) != 0 ? -1 : 1) * (relOffset >> 1));
                    break;
            }

            actions.Add(action);
        }

        patch.ExportActions = actions;

        patch.SourceChecksum = content.ByteArrayToUintLe(offset);
        offset += 4;
        patch.PatchedChecksum = content.ByteArrayToUintLe(offset);
        offset += 4;
        patch.PatchChecksum = content.ByteArrayToUintLe(offset);

        return patch;
    }

    public byte[] CreatePatchData()
    {
        //Encode the magic string first
        var data = new List<byte>();
        data.AddRange(BpsMagicString.ToCharArray().Select(c => (byte)c));

        BpsPatcher.WriteVln(SourceSize, ref data);
        BpsPatcher.WriteVln(PatchedSize, ref data);
        data.Add(0x80); //We don't include any metadata in the patch, so we encode 0

        foreach (var action in ExportActions!)
        {
            BpsPatcher.WriteVln(((action.ActionLength - 1) << 2) + (int)action.OperandType, ref data);
            switch (action.OperandType)
            {
                case BpsPatcher.BpsOperandType.TargetRead:
                    data.AddRange(action.Bytes!);
                    break;
                case BpsPatcher.BpsOperandType.SourceCopy or BpsPatcher.BpsOperandType.TargetCopy:
                    BpsPatcher.WriteVln((Math.Abs(action.RelativeOffset) << 1) +
                                        (action.RelativeOffset < 0 ? 1 : 0), ref data);
                    break;
            }
        }

        data.AddRange(SourceChecksum.UintToByteArrayLe());
        data.AddRange(PatchedChecksum.UintToByteArrayLe());

        PatchChecksum = data.ToArray().Crc32(); //Create checksum of the rest of the patch
        data.AddRange(PatchChecksum.UintToByteArrayLe());

        return data.ToArray();
    }
}
