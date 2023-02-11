using System.Collections.Generic;
using System.Text;
using ColorzCore.IO;
using ColorzCore.Lexer;
using ColorzCore.Raws;

namespace ColorzCore.Parser.AST
{
    internal class RawNode : StatementNode
    {
        private readonly Raw _myRaw;
        private readonly Token _myToken;
        private readonly int _offset;

        public RawNode(Raw raw, Token t, int offset, IList<IParamNode> paramList) : base(paramList)
        {
            _myToken = t;
            _myRaw = raw;
            this._offset = offset;
        }

        public override int Size => _myRaw.LengthBytes(Parameters.Count);

        public override string PrettyPrint(int indentation)
        {
            var sb = new StringBuilder();
            sb.Append(' ', indentation);
            sb.Append(_myToken.Content);
            foreach (var n in Parameters)
            {
                sb.Append(' ');
                sb.Append(n.PrettyPrint());
            }

            return sb.ToString();
        }

        public override void WriteData(Rom rom)
        {
            rom.WriteTo(_offset, _myRaw.GetBytes(Parameters));
        }
    }
}
