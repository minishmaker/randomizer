using System.Text;
using ColorzCore.Parser;

namespace RandomizerCore.Utilities.Models;

public class EventDefine
{
    private readonly string? Replacement;

    public EventDefine(string name)
    {
        Name = name;
    }

    public EventDefine(string name, string replacement)
    {
        Name = name;
        Replacement = replacement;
    }

    public string Name { get; set; }

    public static Definition? Definition => null;

    public void WriteDefineString(StringBuilder builder, string? value = null)
    {
        value ??= Replacement;

        if (string.IsNullOrEmpty(value))
            builder.AppendLine($"#define {Name}");
        else
            builder.AppendLine($"#define {Name} {value}");
    }
}
