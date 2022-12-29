using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
	internal class ParenthesizedAtomNode : AtomNodeKernel
	{
		private IAtomNode inner;

		public ParenthesizedAtomNode(Location startLocation, IAtomNode putIn)
		{
			MyLocation = startLocation;
			inner = putIn;
		}

		public override Location MyLocation { get; }
		public override int Precedence => 1;

		public int Tokens { get; private set; }

		public override int ToInt()
		{
			return inner.ToInt();
		}

		public override Maybe<string> GetIdentifier()
		{
			return new Nothing<string>();
		}

		public override string PrettyPrint()
		{
			var sb = new StringBuilder();
			sb.Append('(');
			sb.Append(inner.PrettyPrint());
			sb.Append(')');
			return sb.ToString();
		}

		public override IEnumerable<Token> ToTokens()
		{
			IList<Token> temp = new List<Token>(inner.ToTokens());
			var myStart = temp[0].Location;
			var myEnd = temp.Last().Location;
			yield return new Token(TokenType.OPEN_PAREN,
				new Location(myStart.file, myStart.lineNum, myStart.colNum - 1), "(");
			foreach (var t in temp) yield return t;
			yield return new Token(TokenType.CLOSE_PAREN,
				new Location(myEnd.file, myEnd.lineNum, myEnd.colNum + temp.Last().Content.Length), "(");
		}

		public override bool CanEvaluate()
		{
			return inner.CanEvaluate();
		}

		public override IAtomNode Simplify()
		{
			inner = inner.Simplify();
			if (CanEvaluate())
				return new NumberNode(MyLocation, ToInt());
			return this;
		}

		public override Maybe<int> Evaluate(ICollection<Token> undefinedIdentifiers)
		{
			return inner.Evaluate(undefinedIdentifiers);
		}
	}
}
