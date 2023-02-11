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

        private IAtomNode _left, _right;
        private readonly Token _op;

        public OperatorNode(IAtomNode l, Token op, IAtomNode r, int prec)
        {
            _left = l;
            _right = r;
            this._op = op;
            Precedence = prec;
        }

        public override int Precedence { get; }

        public override Location MyLocation => _op.Location;

        public override int ToInt()
        {
            return Operators[_op.Type](_left.ToInt(), _right.ToInt());
        }

        public override string PrettyPrint()
        {
            var sb = new StringBuilder(_left.PrettyPrint());
            switch (_op.Type)
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

            sb.Append(_right.PrettyPrint());
            return sb.ToString();
        }

        public override IEnumerable<Token> ToTokens()
        {
            foreach (var t in _left.ToTokens()) yield return t;
            yield return _op;
            foreach (var t in _right.ToTokens()) yield return t;
        }

        public override bool CanEvaluate()
        {
            return _left.CanEvaluate() && _right.CanEvaluate();
        }

        public override IAtomNode Simplify()
        {
            _left = _left.Simplify();
            _right = _right.Simplify();
            if (CanEvaluate())
                return new NumberNode(_left.MyLocation, ToInt());
            return this;
        }

        public override IMaybe<int> Evaluate(ICollection<Token> undefinedIdentifiers)
        {
            var l = _left.Evaluate(undefinedIdentifiers);
            var r = _right.Evaluate(undefinedIdentifiers);
            return l.Bind(newL => r.Fmap(newR => Operators[_op.Type](newL, newR)));
        }
    }
}
