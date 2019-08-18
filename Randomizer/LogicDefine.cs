using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinishRandomizer.Randomizer
{
    public class LogicDefine
    {
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

        }

        public string Replace(string input)
        {
            return "";
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
