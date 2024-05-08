using System.Numerics;
using RandomizerCore.Utilities.Models;

namespace RandomizerCore.Utilities.Util;

public static class BpsPatcher
{
    public enum BpsOperandType
    {
        SourceRead = 0,
        TargetRead = 1,
        SourceCopy = 2,
        TargetCopy = 3
    }

    public static List<byte> ReadBytes(byte[] bytes, int length, ref int offset)
    {
        var newBytes = new List<byte>();
        for (var i = 0; i < length; ++i)
            newBytes.Add(bytes[offset++]);

        return newBytes;
    }

    public static BigInteger ReadVln(byte[] bytes, ref int offset)
    {
        var data = new BigInteger(0);
        var shift = 1;
        while (true)
        {
            var b = bytes[offset++];
            data += (b & 0x7F) * shift;
            if ((b & 0x80) != 0) break;

            shift <<= 7;
            data += shift;
        }

        return data;
    }

    public static void WriteVln(BigInteger data, ref List<byte> outputBytes)
    {
        while (true)
        {
            var b = data & 0x7F;
            data >>= 7;
            if (data == 0)
            {
                outputBytes.Add((byte)(0x80 | b));
                break;
            }

            outputBytes.Add((byte)b);
            --data;
        }
    }

    public static byte[] ApplyPatch(byte[] sourceRom, PatchFile patchFile)
    {
        var patch = Patch.BuildPatchFromContentBytes(patchFile.Content);
        if (sourceRom.Crc32() != patch.SourceChecksum)
            throw new ArgumentException("Hash of provided ROM does not match expected hash!");

        var newRom = new List<byte>(patch.PatchedSize);

        var offset = 0;
        var sourceRelativeOffset = 0;
        var targetRelativeOffset = 0;

        foreach (var action in patch.ExportActions)
            switch (action.OperandType)
            {
                case BpsOperandType.SourceRead:
                    newRom.AddRange(ReadBytes(sourceRom, action.ActionLength, ref offset));
                    break;
                case BpsOperandType.TargetRead:
                    newRom.AddRange(action.Bytes);
                    offset += action.ActionLength;
                    break;
                case BpsOperandType.SourceCopy:
                {
                    sourceRelativeOffset += action.RelativeOffset;
                    var length = action.ActionLength;
                    while (length-- != 0)
                    {
                        newRom.Add(sourceRom[sourceRelativeOffset++]);
                        ++offset;
                    }

                    break;
                }
                case BpsOperandType.TargetCopy:
                {
                    targetRelativeOffset += action.RelativeOffset;
                    var length = action.ActionLength;
                    while (length-- != 0)
                    {
                        newRom.Add(newRom[targetRelativeOffset++]);
                        ++offset;
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

        var newRomBytes = newRom.ToArray();
        if (newRomBytes.Crc32() != patch.PatchedChecksum)
            throw new ArgumentException("Hash of patched ROM does not match expected hash!");

        return newRomBytes;
    }

    public static PatchFile GeneratePatch(byte[] sourceRom, byte[] patchedRom, string? filename = null)
    {
        var patch = new Patch
        {
            SourceSize = sourceRom.Length,
            PatchedSize = patchedRom.Length,
            SourceChecksum = sourceRom.Crc32(),
            PatchedChecksum = patchedRom.Crc32()
        };

        //This uses linear patching by default instead of delta due to ease of implementation, we can change to delta if we want
        var exporterActions = new List<BpsExportAction>();

        var relativeOffset = 0;
        var outputOffset = 0;
        var readLength = 0;

        while (outputOffset < patchedRom.Length)
        {
            var sourceLength = 0;

            for (var i = 0; outputOffset + i < Math.Min(sourceRom.Length, patchedRom.Length); ++i)
            {
                if (sourceRom[outputOffset + i] != patchedRom[outputOffset + i]) break;
                ++sourceLength;
            }

            var relLength = 0;
            for (var i = 1; outputOffset + i < patchedRom.Length; ++i)
            {
                if (patchedRom[outputOffset] != patchedRom[outputOffset + i]) break;
                ++relLength;
            }

            if (relLength >= 4)
            {
                ++readLength;
                ++outputOffset;
                ReadFlush(ref readLength, ref outputOffset, exporterActions, patchedRom);

                var relOffset = outputOffset - 1 - relativeOffset;
                exporterActions.Add(new BpsExportAction
                {
                    OperandType = BpsOperandType.TargetCopy,
                    RelativeOffset = relOffset,
                    ActionLength = relLength
                });
                outputOffset += relLength;
                relativeOffset = outputOffset - 1;
            }
            else if (sourceLength >= 4)
            {
                ReadFlush(ref readLength, ref outputOffset, exporterActions, patchedRom);
                exporterActions.Add(new BpsExportAction
                {
                    OperandType = BpsOperandType.SourceRead,
                    ActionLength = sourceLength
                });
                outputOffset += sourceLength;
            }
            else
            {
                ++readLength;
                ++outputOffset;
            }
        }

        ReadFlush(ref readLength, ref outputOffset, exporterActions, patchedRom);

        patch.ExportActions = exporterActions;

        var sum = exporterActions.Sum(_ => _.ActionLength);

        var patchFile = new PatchFile();

        var patchBytes = patch.CreatePatchData();

        filename ??= $"{Directory.GetCurrentDirectory()}/Patch.bps";

        patchFile.Filename = filename;
        patchFile.Content = patchBytes;
        return patchFile;
    }

    private static void ReadFlush(ref int readLength, ref int outputOffset, List<BpsExportAction> exporterActions,
        byte[] patchedRom)
    {
        if (readLength != 0)
        {
            var action = new BpsExportAction
            {
                OperandType = BpsOperandType.TargetRead,
                Bytes = new List<byte>(),
                ActionLength = readLength
            };
            exporterActions.Add(action);
            var offset = outputOffset - readLength;

            while (readLength != 0)
            {
                action.Bytes.Add(patchedRom[offset++]);
                --readLength;
            }
        }
    }
}
