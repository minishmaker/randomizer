using System.Text.RegularExpressions;
using RandomizerCore.Utilities.Logging;

namespace RandomizerCore.Randomizer.Logic.Defines;

public class LogicDefine
{
    private readonly Regex? Expression;
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
        Expression = new Regex($"`{Name}`");
        // Logger.Instance.LogInfo($"Define Expression: {name}");
    }

    public bool CanReplace(string input)
    {
        // Logger.Instance.LogInfo($"Can replace: {Expression != null}");
        return Expression != null && Expression.IsMatch(input);
    }

    public string Replace(string input)
    {
        return Expression.Replace(input, Replacement);
    }
}
