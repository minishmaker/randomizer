using System;
using System.Collections.Generic;
using System.Text;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
	internal class MacroInvocationNode : IParamNode
	{
		private readonly Token invokeToken;

		private readonly EAParser p;
		private readonly ImmutableStack<Closure> scope;

		public MacroInvocationNode(EAParser p, Token invokeTok, IList<IList<Token>> parameters,
			ImmutableStack<Closure> scopes)
		{
			this.p = p;
			invokeToken = invokeTok;
			Parameters = parameters;
			scope = scopes;
		}

		public IList<IList<Token>> Parameters { get; }

		public string Name => invokeToken.Content;

		public ParamType Type => ParamType.MACRO;

		public string PrettyPrint()
		{
			var sb = new StringBuilder();
			sb.Append(invokeToken.Content);
			sb.Append('(');
			for (var i = 0; i < Parameters.Count; i++)
			{
				foreach (var t in Parameters[i]) sb.Append(t.Content);
				if (i < Parameters.Count - 1)
					sb.Append(',');
			}

			sb.Append(')');
			return sb.ToString();
		}

		public Either<int, string> TryEvaluate()
		{
			return new Right<int, string>("Expected atomic parameter.");
		}

		public Location MyLocation => invokeToken.Location;

		public Maybe<IParamNode> Evaluate(ICollection<Token> undefinedIdentifiers)
		{
			//There shouldn't be macros lingering out by the time we're simplifying?
			throw new MacroException(this);
		}

		public IEnumerable<Token> ExpandMacro()
		{
			return p.Macros.GetMacro(invokeToken.Content, Parameters.Count).ApplyMacro(invokeToken, Parameters, scope);
		}

		public class MacroException : Exception
		{
			public MacroException(MacroInvocationNode min)
			{
				CausedError = min;
			}

			public MacroInvocationNode CausedError { get; }
		}
	}
}
