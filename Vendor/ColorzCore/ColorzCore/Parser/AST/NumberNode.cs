using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
    public class NumberNode : AtomNodeKernel
    {
        private readonly int _value;

        public NumberNode(Token num)
        {
            MyLocation = num.Location;
            _value = num.Content.ToInt();
        }

        public NumberNode(Token text, int value)
        {
            MyLocation = text.Location;
            this._value = value;
        }

        public NumberNode(Location loc, int value)
        {
            MyLocation = loc;
            this._value = value;
        }

        public override Location MyLocation { get; }
        public override int Precedence => 11;

        public override int ToInt()
        {
            return _value;
        }

        public override IEnumerable<Token> ToTokens()
        {
            yield return new Token(TokenType.NUMBER, MyLocation, _value.ToString());
        }

        public override bool CanEvaluate()
        {
            return true;
        }

        public override IAtomNode Simplify()
        {
            return this;
        }

        public override IMaybe<int> Evaluate(ICollection<Token> undefinedIdentifiers)
        {
            return new Just<int>(_value);
        }
    }
}
