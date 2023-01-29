using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
    public class ListNode : IParamNode
    {
        public ListNode(Location startLocation, IList<IAtomNode> param)
        {
            MyLocation = startLocation;
            Interior = param;
        }

        public IList<IAtomNode> Interior { get; }

        public int NumCoords => Interior.Count;
        public Location MyLocation { get; }

        public ParamType Type => ParamType.LIST;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            for (var i = 0; i < Interior.Count; i++)
            {
                sb.Append(Interior[i].ToInt());
                if (i < Interior.Count - 1)
                    sb.Append(',');
            }

            sb.Append(']');
            return sb.ToString();
        }

        public string PrettyPrint()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            for (var i = 0; i < Interior.Count; i++)
            {
                sb.Append(Interior[i].PrettyPrint());
                if (i < Interior.Count - 1)
                    sb.Append(',');
            }

            sb.Append(']');
            return sb.ToString();
        }

        public IEither<int, string> TryEvaluate()
        {
            return new Right<int, string>("Expected atomic parameter.");
        }

        public IMaybe<IParamNode> Evaluate(ICollection<Token> undefinedIdentifiers)
        {
            IEnumerable<Token> acc = new List<Token>();
            for (var i = 0; i < Interior.Count; i++)
                Interior[i].Evaluate(undefinedIdentifiers).IfJust(a =>
                {
                    Interior[i] = new NumberNode(Interior[i].MyLocation, a);
                });
            return new Just<IParamNode>(this);
        }

        public IEnumerable<Token> ToTokens()
        {
            //Similar code to ParenthesizedAtom
            IList<IList<Token>> temp = new List<IList<Token>>();
            foreach (var n in Interior) temp.Add(new List<Token>(n.ToTokens()));
            var myStart = MyLocation;
            var myEnd = temp.Count > 0 ? temp.Last().Last().Location : MyLocation;
            yield return new Token(TokenType.OPEN_BRACKET,
                new Location(myStart.file, myStart.lineNum, myStart.colNum - 1), "[");
            for (var i = 0; i < temp.Count; i++)
            {
                foreach (var t in temp[i]) yield return t;
                if (i < temp.Count - 1)
                {
                    var tempEnd = temp[i].Last().Location;
                    yield return new Token(TokenType.COMMA,
                        new Location(tempEnd.file, tempEnd.lineNum, tempEnd.colNum + temp[i].Last().Content.Length),
                        ",");
                }
            }

            yield return new Token(TokenType.CLOSE_BRACKET,
                new Location(myEnd.file, myEnd.lineNum,
                    myEnd.colNum + (temp.Count > 0 ? temp.Last().Last().Content.Length : 1)), "]");
        }
    }
}
