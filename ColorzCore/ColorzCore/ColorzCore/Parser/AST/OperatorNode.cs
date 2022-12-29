using System.Collections.Generic;
using System.Text;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
	internal delegate int BinaryIntOp(int a, int b);

	internal class OperatorNode : AtomNodeKernel
	{
		public static readonly Dictionary<TokenType, BinaryIntOp> Operators = new Dictionary<TokenType, BinaryIntOp>
		{
			{ TokenType.MUL_OP, (x, y) => x * y },
			{ TokenType.DIV_OP, (x, y) => x / y },
			{ TokenType.MOD_OP, (x, y) => x % y },
			{ TokenType.ADD_OP, (x, y) => x + y },
			{ TokenType.SUB_OP, (x, y) => x - y },
			{ TokenType.LSHIFT_OP, (x, y) => x << y },
			{ TokenType.RSHIFT_OP, (x, y) => (int)((uint)x >> y) },
			{ TokenType.SIGNED_RSHIFT_OP, (x, y) => x >> y },
			{ TokenType.AND_OP, (x, y) => x & y },
			{ TokenType.XOR_OP, (x, y) => x ^ y },
			{ TokenType.OR_OP, (x, y) => x | y }
		};

		private IAtomNode left, right;
		private readonly Token op;

		public OperatorNode(IAtomNode l, Token op, IAtomNode r, int prec)
		{
			left = l;
			right = r;
			this.op = op;
			Precedence = prec;
		}

		public override int Precedence { get; }

		public override Location MyLocation => op.Location;

		public override int ToInt()
		{
			return Operators[op.Type](left.ToInt(), right.ToInt());
		}

		public override string PrettyPrint()
		{
			var sb = new StringBuilder(left.PrettyPrint());
			switch (op.Type)
			{
				case TokenType.MUL_OP:
					sb.Append("*");
					break;
				case TokenType.DIV_OP:
					sb.Append("/");
					break;
				case TokenType.ADD_OP:
					sb.Append("+");
					break;
				case TokenType.SUB_OP:
					sb.Append("-");
					break;
				case TokenType.LSHIFT_OP:
					sb.Append("<<");
					break;
				case TokenType.RSHIFT_OP:
					sb.Append(">>");
					break;
				case TokenType.SIGNED_RSHIFT_OP:
					sb.Append(">>>");
					break;
				case TokenType.AND_OP:
					sb.Append("&");
					break;
				case TokenType.XOR_OP:
					sb.Append("^");
					break;
				case TokenType.OR_OP:
					sb.Append("|");
					break;
			}

			sb.Append(right.PrettyPrint());
			return sb.ToString();
		}

		public override IEnumerable<Token> ToTokens()
		{
			foreach (var t in left.ToTokens()) yield return t;
			yield return op;
			foreach (var t in right.ToTokens()) yield return t;
		}

		public override bool CanEvaluate()
		{
			return left.CanEvaluate() && right.CanEvaluate();
		}

		public override IAtomNode Simplify()
		{
			left = left.Simplify();
			right = right.Simplify();
			if (CanEvaluate())
				return new NumberNode(left.MyLocation, ToInt());
			return this;
		}

		public override Maybe<int> Evaluate(ICollection<Token> undefinedIdentifiers)
		{
			var l = left.Evaluate(undefinedIdentifiers);
			var r = right.Evaluate(undefinedIdentifiers);
			return l.Bind(newL => r.Fmap(newR => Operators[op.Type](newL, newR)));
		}
	}
}
