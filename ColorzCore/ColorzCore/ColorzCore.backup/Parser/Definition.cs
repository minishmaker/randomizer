using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser
{
	public class Definition
	{
		private readonly IList<Token> replacement;

		public Definition()
		{
			replacement = new List<Token>();
		}

		public Definition(IList<Token> defn)
		{
			replacement = defn;
		}

		public IEnumerable<Token> ApplyDefinition(Token toReplace)
		{
			for (var i = 0; i < replacement.Count; i++)
			{
				var newLoc = new Location(toReplace.FileName, toReplace.LineNumber, toReplace.ColumnNumber);
				yield return new Token(replacement[i].Type, newLoc, replacement[i].Content);
			}
		}
	}
}
