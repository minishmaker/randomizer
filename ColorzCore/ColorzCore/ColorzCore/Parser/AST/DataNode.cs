using System.Collections.Generic;
using ColorzCore.IO;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
	internal class DataNode : ILineNode
	{
		private readonly byte[] data;
		private readonly int offset;

		public DataNode(int offset, byte[] data)
		{
			this.offset = offset;
			this.data = data;
		}

		public int Size => data.Length;

		public string PrettyPrint(int indentation)
		{
			return string.Format("Raw Data Block of Length {0}", Size);
		}

		public void WriteData(ROM rom)
		{
			rom.WriteTo(offset, data);
		}

		public void EvaluateExpressions(ICollection<Token> undefinedIdentifiers)
		{
			// Nothing to be done because we contain no expressions.
		}
	}
}
