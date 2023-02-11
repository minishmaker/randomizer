using System.Text.RegularExpressions;
using RandomizerCore.Utilities.Logging;

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
        Logger.Instance.LogInfo($"Define Expression: {name}");
    }

    public bool CanReplace(string input)
    {
        Logger.Instance.LogInfo($"Can replace: {_expression != null}");
        return _expression != null && _expression.IsMatch(input);
    }

    public string Replace(string input)
    {
        return _expression.Replace(input, Replacement);
    }
}