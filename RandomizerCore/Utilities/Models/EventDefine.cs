using System.Text;
using ColorzCore.Parser;

namespace RandomizerCore.Utilities.Models;

public class EventDefine
{
    public string Name { get; set; }
    
    private readonly string? _replacement;

    public EventDefine(string name)
    {
        Name = name;
    }

    public EventDefine(string name, string replacement)
    {
        Name = name;
        _replacement = replacement;
    }

    public static Definition? Definition => null;

    public void WriteDefineString(StringBuilder builder, string? value = null)
    {
        value ??= _replacement;

        if (string.IsNullOrEmpty(value))
            builder.AppendLine($"#define {Name}");
        else
            builder.AppendLine($"#define {Name} {value}");
    }
}