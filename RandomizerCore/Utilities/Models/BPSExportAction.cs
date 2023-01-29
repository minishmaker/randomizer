using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Utilities.Models;

public class BPSExportAction
{
    public BPSPatcher.BPSOperandType OperandType { get; set; }
    
    public int ActionLength { get; set; }
    
    public int RelativeOffset { get; set; }
    
    public List<byte> Bytes { get; set; }
}