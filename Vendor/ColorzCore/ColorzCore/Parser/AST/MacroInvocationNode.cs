using System;
using System.Collections.Generic;
using System.Text;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
    internal class MacroInvocationNode : IParamNode
    {
        private readonly Token _invokeToken;

        private readonly EaParser _p;
        private readonly ImmutableStack<Closure> _scope;

        public MacroInvocationNode(EaParser p, Token invokeTok, IList<IList<Token>> parameters,
            ImmutableStack<Closure> scopes)
        {
            this._p = p;
            _invokeToken = invokeTok;
            Parameters = parameters;
            _scope = scopes;
        }

        public IList<IList<Token>> Parameters { get; }

        public string Name => _invokeToken.Content;

        public ParamType Type => ParamType.MACRO;

        public string PrettyPrint()
        {
            var sb = new StringBuilder();
            sb.Append(_invokeToken.Content);
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

        public IEither<int, string> TryEvaluate()
        {
            return new Right<int, string>("Expected atomic parameter.");
        }

        public Location MyLocation => _invokeToken.Location;

        public IMaybe<IParamNode> Evaluate(ICollection<Token> undefinedIdentifiers)
        {
            //There shouldn't be macros lingering out by the time we're simplifying?
            throw new MacroException(this);
        }

        public IEnumerable<Token> ExpandMacro()
        {
            return _p.Macros.GetMacro(_invokeToken.Content, Parameters.Count).ApplyMacro(_invokeToken, Parameters, _scope);
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
