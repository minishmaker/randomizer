using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinishRandomizer.Randomizer.Logic.Interpreting.Lexer
{
    public enum TokenType
    {
        Untyped,

    }

    public class Token
    {
        public TokenType type;
        public string Content;
    }
}
