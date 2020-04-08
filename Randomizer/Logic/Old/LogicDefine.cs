using System;
using System.Text.RegularExpressions;

namespace MinishRandomizer.Randomizer.Logic
{
    public class LogicDefine
    {
        public string Name;
        public string Replacement;
        private Regex Expression;

        public LogicDefine(string name)
        {
            Name = name;
        }

        public LogicDefine(string name, string replacement)
        {
            Name = name;
            Replacement = replacement;
            Expression = new Regex($"`{Name}`");
            Console.WriteLine(Expression);
        }

        public bool CanReplace(string input)
        {
            Console.WriteLine(Expression != null);
            return Expression != null && Expression.IsMatch(input);
        }

        public string Replace(string input)
        {
            return Expression.Replace(input, Replacement);
        }
    }
}
