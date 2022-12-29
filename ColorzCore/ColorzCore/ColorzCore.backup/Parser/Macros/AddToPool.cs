using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.Macros
{
	internal class AddToPool : BuiltInMacro
	{
		public AddToPool(EAParser parent)
		{
			ParentParser = parent;
		}
		/*
		 * Macro Usage:
		 * AddToPool(tokens...): adds token to pool, and expands to label name referring to those tokens
		 * AddToPool(tokens..., alignment): adds token to pool and make sure pooled tokens are aligned given alignment        
		 */

		public EAParser ParentParser { get; }

		public override IEnumerable<Token> ApplyMacro(Token head, IList<IList<Token>> parameters,
			ImmutableStack<Closure> scopes)
		{
			var line = new List<Token>(6 + parameters[0].Count);

			var labelName = ParentParser.Pool.MakePoolLabelName();

			if (parameters.Count == 2)
			{
				// Add Alignment directive 

				line.Add(new Token(TokenType.IDENTIFIER, head.Location, "ALIGN"));
				line.Add(parameters[1][0]);
				line.Add(new Token(TokenType.SEMICOLON, head.Location, ";"));
			}

			// TODO: Make label declaration global (when this feature gets implemented)
			// This way the name will be available as long as it is pooled (reguardless of pool scope)

			line.Add(new Token(TokenType.IDENTIFIER, head.Location, labelName));
			line.Add(new Token(TokenType.COLON, head.Location, ":"));

			line.AddRange(parameters[0]);
			line.Add(new Token(TokenType.NEWLINE, head.Location, "\n"));

			ParentParser.Pool.Lines.Add(new Pool.PooledLine(scopes, line));

			yield return new Token(TokenType.IDENTIFIER, head.Location, labelName);
		}

		public override bool ValidNumParams(int num)
		{
			return num == 1 || num == 2;
		}
	}
}
