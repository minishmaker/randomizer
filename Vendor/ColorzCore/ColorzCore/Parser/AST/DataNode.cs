using System.Collections.Generic;
using ColorzCore.IO;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
    internal class DataNode : ILineNode
    {
        private readonly byte[] _data;
        private readonly int _offset;

        public DataNode(int offset, byte[] data)
        {
            this._offset = offset;
            this._data = data;
        }

        public int Size => _data.Length;

        public string PrettyPrint(int indentation)
        {
            return string.Format("Raw Data Block of Length {0}", Size);
        }

        public void WriteData(Rom rom)
        {
            rom.WriteTo(_offset, _data);
        }

        public void EvaluateExpressions(ICollection<Token> undefinedIdentifiers)
        {
            // Nothing to be done because we contain no expressions.
        }
    }
}
