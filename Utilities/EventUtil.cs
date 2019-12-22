using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace MinishRandomizer.Utilities
{
    public static class EventUtil
    {
        public static Dictionary<string, ColorzCore.Parser.Definition> GetDefinitions(List<EventDefine> defines)
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
                return Replacement == null ? new ColorzCore.Parser.Definition() : ColorzCore.Program.CreateDefinition(Replacement);
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
    }
}
