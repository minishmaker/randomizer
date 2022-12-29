using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
	internal class NegationNode : AtomNodeKernel
	{
		private IAtomNode interior;
		private readonly Token myToken;

		public NegationNode(Token token, IAtomNode inside)
		{
			myToken = token;
			interior = inside;
		}

		public override int Precedence => 11;
		public override Location MyLocation => myToken.Location;

		public override bool CanEvaluate()
		{
			return interior.CanEvaluate();
		}

		public override int ToInt()
		{
			return -interior.ToInt();
		}

		public override string PrettyPrint()
		{
			return '-' + interior.PrettyPrint();
		}

		public override IAtomNode Simplify()
		{
			interior = interior.Simplify();
			if (CanEvaluate())
				return new NumberNode(MyLocation, ToInt());
			return this;
		}

		public override IEnumerable<Token> ToTokens()
		{
			yield return myToken;
			foreach (var t in interior.ToTokens())
				yield return t;
		}

		public override Maybe<int> Evaluate(ICollection<Token> undefinedIdentifiers)
		{
			return interior.Evaluate(undefinedIdentifiers).Fmap(x => -x);
		}
	}
}
