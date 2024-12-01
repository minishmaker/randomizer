using ColorzCore;
using ColorzCore.IO;
using ColorzCore.Parser;
using RandomizerCore.Utilities.Models;

namespace RandomizerCore.Utilities.Util;

public static class EventUtil
{
    public static void WriteEvent(Stream input, Stream output, List<EventDefine> defines)
    {
        var errorStream = Console.Error;
        var log = new Log
        {
            Output = errorStream,
            WarningsAreErrors = false,
            NoColoredTags = false
        };

        var definitions = GetDefinitions(defines);

        Program.EaParse("TMC", "Language Raws", ".txt", input, "TMCR", output, log, definitions);
    }

    private static Dictionary<string, Definition>? GetDefinitions(List<EventDefine> defines)
    {
        return defines.ToDictionary(define => define.Name, define => EventDefine.Definition!);
    }
}
