using System;
using System.Collections.Generic;
using System.Linq;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
    public class IdentifierNode : AtomNodeKernel
    {
        private readonly ImmutableStack<Closure> _scope;
        private readonly Token _identifier;

        public IdentifierNode(Token id, ImmutableStack<Closure> scopes)
        {
            _identifier = id;
            _scope = scopes;
        }

        public override int Precedence => 11;
        public override Location MyLocation => _identifier.Location;

        public override int ToInt()
        {
            var temp = _scope;
            while (!temp.IsEmpty)
                if (temp.Head.HasLocalLabel(_identifier.Content))
                    return temp.Head.GetLabel(_identifier.Content);
                else
                    temp = temp.Tail;
            throw new UndefinedIdentifierException(_identifier);
        }

        public override IMaybe<int> Evaluate(ICollection<Token> undefinedIdentifiers)
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

        public override IMaybe<string> GetIdentifier()
        {
            return new Just<string>(_identifier.Content);
        }

        public override string PrettyPrint()
        {
            try
            {
                return "0x" + ToInt().ToString("X");
            }
            catch (UndefinedIdentifierException)
            {
                return _identifier.Content;
            }
        }

        public override IEnumerable<Token> ToTokens()
        {
            yield return _identifier;
        }

        public override string ToString()
        {
            return _identifier.Content;
        }

        public override bool CanEvaluate()
        {
            return _scope.Any(c => c.HasLocalLabel(_identifier.Content));
        }

        public override IAtomNode Simplify()
        {
            if (!CanEvaluate())
                return this;
            return new NumberNode(_identifier, ToInt());
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
