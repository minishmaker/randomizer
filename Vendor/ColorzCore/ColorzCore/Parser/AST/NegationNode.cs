using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
    internal class NegationNode : AtomNodeKernel
    {
        private IAtomNode _interior;
        private readonly Token _myToken;

        public NegationNode(Token token, IAtomNode inside)
        {
            _myToken = token;
            _interior = inside;
        }

        public override int Precedence => 11;
        public override Location MyLocation => _myToken.Location;

        public override bool CanEvaluate()
        {
            return _interior.CanEvaluate();
        }

        public override int ToInt()
        {
            return -_interior.ToInt();
        }

        public override string PrettyPrint()
        {
            return '-' + _interior.PrettyPrint();
        }

        public override IAtomNode Simplify()
        {
            _interior = _interior.Simplify();
            if (CanEvaluate())
                return new NumberNode(MyLocation, ToInt());
            return this;
        }

        public override IEnumerable<Token> ToTokens()
        {
            yield return _myToken;
            foreach (var t in _interior.ToTokens())
                yield return t;
        }

        public override IMaybe<int> Evaluate(ICollection<Token> undefinedIdentifiers)
        {
            return _interior.Evaluate(undefinedIdentifiers).Fmap(x => -x);
        }
    }
}
