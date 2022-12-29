using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser
{
    public class Definition
    {
        private readonly IList<Token> _replacement;

        public Definition()
        {
            _replacement = new List<Token>();
        }

        public Definition(IList<Token> defn)
        {
            _replacement = defn;
        }

        public IEnumerable<Token> ApplyDefinition(Token toReplace)
        {
            for (var i = 0; i < _replacement.Count; i++)
            {
                var newLoc = new Location(toReplace.FileName, toReplace.LineNumber, toReplace.ColumnNumber);
                yield return new Token(_replacement[i].Type, newLoc, _replacement[i].Content);
            }
        }
    }
}
