using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;
using ColorzCore.Parser;
using ColorzCore.Parser.AST;

namespace ColorzCore.Preprocessor.Directives
{
	internal class ElseDirective : IDirective
	{
		public int MinParams => 0;

		public int? MaxParams => 0;

		public bool RequireInclusion => false;

		public Maybe<ILineNode> Execute(EAParser p, Token self, IList<IParamNode> parameters,
			MergeableGenerator<Token> tokens)
		{
			if (p.Inclusion.IsEmpty)
				p.Error(self.Location, "No matching if[n]def.");
			else
				p.Inclusion = new ImmutableStack<bool>(!p.Inclusion.Head, p.Inclusion.Tail);
			return new Nothing<ILineNode>();
		}
	}
}
