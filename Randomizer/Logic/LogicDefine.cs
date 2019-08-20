using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        }

        public string Replace(string input)
        {
            return Expression.Replace(input, Replacement);
        }
    }

    public class LogicFlag : LogicDefine
    {
        public string NiceName;
        public bool Active;

        public LogicFlag(string name, string niceName) : base(name)
        {
            NiceName = niceName;
            Active = false;
        }
    }
}
