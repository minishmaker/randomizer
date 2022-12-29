using System.Collections.Generic;
using System.Text;
using ColorzCore.IO;
using ColorzCore.Lexer;
using ColorzCore.Raws;

namespace ColorzCore.Parser.AST
{
	internal class RawNode : StatementNode
	{
		private readonly Raw myRaw;
		private readonly Token myToken;
		private readonly int offset;

		public RawNode(Raw raw, Token t, int offset, IList<IParamNode> paramList) : base(paramList)
		{
			myToken = t;
			myRaw = raw;
			this.offset = offset;
		}

		public override int Size => myRaw.LengthBytes(Parameters.Count);

		public override string PrettyPrint(int indentation)
		{
			var sb = new StringBuilder();
			sb.Append(' ', indentation);
			sb.Append(myToken.Content);
			foreach (var n in Parameters)
			{
				sb.Append(' ');
				sb.Append(n.PrettyPrint());
			}

			return sb.ToString();
		}

		public override void WriteData(ROM rom)
		{
			rom.WriteTo(offset, myRaw.GetBytes(Parameters));
		}
	}
}
