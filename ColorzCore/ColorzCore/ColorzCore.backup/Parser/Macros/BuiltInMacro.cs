using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.Macros
{
	internal abstract class BuiltInMacro : IMacro
	{
		public abstract IEnumerable<Token> ApplyMacro(Token head, IList<IList<Token>> parameters,
			ImmutableStack<Closure> scopes);

		public abstract bool ValidNumParams(int num);
	}
}
