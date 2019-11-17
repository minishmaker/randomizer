using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace MinishRandomizer.Utilities
{
    public static class EventUtil
    {
        public static void WriteEvent(Stream input, Stream output, List<EventDefine> defines)
        {
            TextWriter errorStream = Console.Error;
            ColorzCore.IO.Log log = new ColorzCore.IO.Log
            {
                Output = errorStream,
                WarningsAreErrors = false,
                NoColoredTags = false
            };

            Dictionary<string, ColorzCore.Parser.Definition> definitions = GetDefinitions(defines);

            ColorzCore.Program.EAParse("TMC", "Language Raws", ".txt", input, "TMCR", output, log, definitions);
        }

        private static Dictionary<string, ColorzCore.Parser.Definition> GetDefinitions(List<EventDefine> defines)
        {
            Dictionary<string, ColorzCore.Parser.Definition> definitions = new Dictionary<string, ColorzCore.Parser.Definition>();
            foreach (EventDefine define in defines)
            {
                definitions.Add(define.Name, define.Definition);
            }

            return definitions;
        }
    }

    public class EventDefine
    {

        public string Name;
        private string Replacement;

        public ColorzCore.Parser.Definition Definition
        {
            get
            {
                return null;
            }
        }

        public EventDefine(string name)
        {
            Name = name;
        }

        public EventDefine(string name, string replacement)
        {
            Name = name;
            Replacement = replacement;
        }

        public void WriteDefineString(StringBuilder w, string value = null)
        {
            if (value == null)
            {
                value = Replacement;
            }

            if (string.IsNullOrEmpty(value))
            {
                w.AppendLine($"#define {Name}");
            }
            else
            {
                w.AppendLine($"#define {Name} {value}");
            }

        }
    }
}
