using System.Text.RegularExpressions;

namespace RandomizerCore.Randomizer.Logic.Defines;

public class LogicDefine
{
    private readonly Regex? _expression;
    public string Name;
    public string Replacement;

    public LogicDefine(string name)
    {
        Name = name;
    }

    public LogicDefine(string name, string replacement)
    {
        Name = name;
        Replacement = replacement;
        _expression = new Regex($"`{Name}`");
        Console.WriteLine(_expression);
    }

    public bool CanReplace(string input)
    {
        Console.WriteLine(_expression != null);
        return _expression != null && _expression.IsMatch(input);
    }

    public string Replace(string input)
    {
        return _expression.Replace(input, Replacement);
    }
}