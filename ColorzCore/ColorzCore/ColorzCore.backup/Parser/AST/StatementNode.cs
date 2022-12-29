using System.Collections.Generic;
using ColorzCore.IO;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
	internal abstract class StatementNode : ILineNode
	{
		protected StatementNode(IList<IParamNode> parameters)
		{
			Parameters = parameters;
		}

		public IList<IParamNode> Parameters { get; }

		public abstract int Size { get; }

		public abstract string PrettyPrint(int indentation);
		public abstract void WriteData(ROM rom);

		public void EvaluateExpressions(ICollection<Token> undefinedIdentifiers)
		{
			for (var i = 0; i < Parameters.Count; i++)
				Parameters[i].Evaluate(undefinedIdentifiers).IfJust(p => { Parameters[i] = p; });
		}

		public void Simplify()
		{
			for (var i = 0; i < Parameters.Count; i++)
				if (Parameters[i].Type == ParamType.ATOM)
					Parameters[i] = ((IAtomNode)Parameters[i]).Simplify();
		}
	}
}
