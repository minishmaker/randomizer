using System;
using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
    public abstract class AtomNodeKernel : IAtomNode
    {
        public abstract int Precedence { get; }

        public abstract int ToInt();

        public ParamType Type => ParamType.ATOM;

        public override string ToString()
        {
            return "0x" + ToInt().ToString("X");
        }

        public virtual IMaybe<string> GetIdentifier()
        {
            return new Nothing<string>();
        }

        public virtual string PrettyPrint()
        {
            return ToString(); //TODO: Mark abstract
        }

        public abstract IEnumerable<Token> ToTokens();
        public abstract Location MyLocation { get; }

        public IEither<int, string> TryEvaluate()
        {
            try
            {
                var res = ToInt();
                return new Left<int, string>(res);
            }
            catch (IdentifierNode.UndefinedIdentifierException e)
            {
                return new Right<int, string>("Unrecognized identifier: " + e.CausedError.Content);
            }
            catch (DivideByZeroException)
            {
                return new Right<int, string>("Division by zero.");
            }
        }

        public abstract bool CanEvaluate();

        public abstract IMaybe<int> Evaluate(ICollection<Token> undefinedIdentifiers);

        IMaybe<IParamNode> IParamNode.Evaluate(ICollection<Token> undefinedIdentifiers)
        {
            return Evaluate(undefinedIdentifiers).Fmap(a => (IParamNode)new NumberNode(MyLocation, a));
        }

        public abstract IAtomNode Simplify();
    }
}
