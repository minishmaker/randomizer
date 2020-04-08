using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinishRandomizer.Randomizer.Logic.Interpreting.Lexer
{
    class TokenList
    {
        string processedFile;

        public IEnumerable<Token> TokenizeString(string input)
        {
            yield return new Token();
        }
    }
}
