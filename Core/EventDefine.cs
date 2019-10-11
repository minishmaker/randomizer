using System.Text;

namespace MinishRandomizer.Core
{
    public class EventDefine
    {
        public string Name;
        private string Replacement;

        public EventDefine(string name)
        {
            Name = name;
        }

        public EventDefine(string name, string replacement)
        {
            Name = name;
            Replacement = replacement;
        }

        public void WriteDefine(StringBuilder w, string value = null)
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
