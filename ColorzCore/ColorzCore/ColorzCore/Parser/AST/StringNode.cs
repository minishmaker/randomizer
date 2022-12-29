using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
	public class StringNode : IParamNode
	{
		public static readonly Regex idRegex = new Regex("^([a-zA-Z_][a-zA-Z0-9_]*)$");

		public StringNode(Token value)
		{
			MyToken = value;
		}

		public Token MyToken { get; }

		public Location MyLocation => MyToken.Location;
		public ParamType Type => ParamType.STRING;

		public override string ToString()
		{
			return MyToken.Content;
		}

		public string PrettyPrint()
		{
			return '"' + ToString() + '"';
		}

		public Either<int, string> TryEvaluate()
		{
			return new Right<int, string>("Expected atomic parameter.");
		}

		public Maybe<IParamNode> Evaluate(ICollection<Token> undefinedIds)
		{
			//Nothing to be done.
			return new Just<IParamNode>(this);
		}

		public IEnumerable<byte> ToBytes()
		{
			return Encoding.ASCII.GetBytes(ToString());
		}

		public IEnumerable<Token> ToTokens()
		{
			yield return MyToken;
		}

		public bool IsValidIdentifier()
		{
			return idRegex.IsMatch(MyToken.Content);
		}

		public IdentifierNode ToIdentifier(ImmutableStack<Closure> scope)
		{
			return new IdentifierNode(MyToken, scope);
		}
	}
}
