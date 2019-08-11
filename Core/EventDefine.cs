using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinishRandomizer.Core
{
    public class EventDefine
    {
        public string Name;
        public EventDefine(string name)
        {
            Name = name;
        }

        public void WriteDefine(StreamWriter w, string value)
        {
            w.WriteLine($"#define {Name} {value}");
        }
    }
}
