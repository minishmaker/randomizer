using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColorzCore.IO;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
	internal class BlockNode : ILineNode
	{
		public BlockNode()
		{
			Children = new List<ILineNode>();
		}

		public IList<ILineNode> Children { get; }

		public int Size
		{
			get { return Children.Sum(n => n.Size); }
		}

		public string PrettyPrint(int indentation)
		{
			var sb = new StringBuilder();
			sb.Append(' ', indentation);
			sb.Append('{');
			sb.Append('\n');
			foreach (var i in Children)
			{
				sb.Append(i.PrettyPrint(indentation + 4));
				sb.Append('\n');
			}

			sb.Append(' ', indentation);
			sb.Append('}');
			return sb.ToString();
		}

		public void WriteData(ROM rom)
		{
			foreach (var child in Children) child.WriteData(rom);
		}

		public void EvaluateExpressions(ICollection<Token> undefinedIdentifiers)
		{
			foreach (var line in Children)
				line.EvaluateExpressions(undefinedIdentifiers);
		}
	}
}
