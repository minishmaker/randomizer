using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;
using ColorzCore.Parser;
using ColorzCore.Parser.AST;

namespace ColorzCore.Preprocessor.Directives
{
	internal class PoolDirective : IDirective
	{
		public int MinParams => 0;
		public int? MaxParams => 0;
		public bool RequireInclusion => true;

		public Maybe<ILineNode> Execute(EAParser p, Token self, IList<IParamNode> parameters,
			MergeableGenerator<Token> tokens)
		{
			var result = new BlockNode();

			// Iterating indices (and not values via foreach)
			// to avoid crashes occuring with AddToPool within AddToPool

			for (var i = 0; i < p.Pool.Lines.Count; ++i)
			{
				var line = p.Pool.Lines[i];

				var tempGenerator = new MergeableGenerator<Token>(line.Tokens);
				tempGenerator.MoveNext();

				while (!tempGenerator.EOS)
					p.ParseLine(tempGenerator, line.Scope).IfJust(
						lineNode => result.Children.Add(lineNode));
			}

			p.Pool.Lines.Clear();

			return new Just<ILineNode>(result);
		}
	}
}
