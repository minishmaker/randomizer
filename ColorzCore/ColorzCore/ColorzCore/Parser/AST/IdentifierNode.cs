using System;
using System.Collections.Generic;
using System.Linq;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
	public class IdentifierNode : AtomNodeKernel
	{
		private readonly ImmutableStack<Closure> scope;
		private readonly Token identifier;

		public IdentifierNode(Token id, ImmutableStack<Closure> scopes)
		{
			identifier = id;
			scope = scopes;
		}

		public override int Precedence => 11;
		public override Location MyLocation => identifier.Location;

		public override int ToInt()
		{
			var temp = scope;
			while (!temp.IsEmpty)
				if (temp.Head.HasLocalLabel(identifier.Content))
					return temp.Head.GetLabel(identifier.Content);
				else
					temp = temp.Tail;
			throw new UndefinedIdentifierException(identifier);
		}

		public override Maybe<int> Evaluate(ICollection<Token> undefinedIdentifiers)
		{
			try
			{
				return new Just<int>(ToInt());
			}
			catch (UndefinedIdentifierException e)
			{
				undefinedIdentifiers.Add(e.CausedError);
				return new Nothing<int>();
			}
		}

		public override Maybe<string> GetIdentifier()
		{
			return new Just<string>(identifier.Content);
		}

		public override string PrettyPrint()
		{
			try
			{
				return "0x" + ToInt().ToString("X");
			}
			catch (UndefinedIdentifierException)
			{
				return identifier.Content;
			}
		}

		public override IEnumerable<Token> ToTokens()
		{
			yield return identifier;
		}

		public override string ToString()
		{
			return identifier.Content;
		}

		public override bool CanEvaluate()
		{
			return scope.Any(c => c.HasLocalLabel(identifier.Content));
		}

		public override IAtomNode Simplify()
		{
			if (!CanEvaluate())
				return this;
			return new NumberNode(identifier, ToInt());
		}

		public class UndefinedIdentifierException : Exception
		{
			public UndefinedIdentifierException(Token causedError)
			{
				CausedError = causedError;
			}

			public Token CausedError { get; set; }
		}
	}
}
