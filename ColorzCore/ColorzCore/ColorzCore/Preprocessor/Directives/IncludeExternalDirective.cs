using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ColorzCore.DataTypes;
using ColorzCore.IO;
using ColorzCore.Lexer;
using ColorzCore.Parser;
using ColorzCore.Parser.AST;

namespace ColorzCore.Preprocessor.Directives
{
	internal class IncludeExternalDirective : IDirective
	{
		public IncludeFileSearcher FileSearcher { get; set; }
		public int MinParams => 1;
		public int? MaxParams => null;
		public bool RequireInclusion => true;

		public Maybe<ILineNode> Execute(EAParser parse, Token self, IList<IParamNode> parameters,
			MergeableGenerator<Token> tokens)
		{
			var validFile = FileSearcher.FindFile(Path.GetDirectoryName(self.FileName),
				IOUtility.GetToolFileName(parameters[0].ToString()));

			if (validFile.IsNothing)
			{
				parse.Error(parameters[0].MyLocation, "Tool " + parameters[0].ToString() + " not found.");
				return new Nothing<ILineNode>();
			}
			//TODO: abstract out all this running stuff into a method so I don't have code duplication with inctext

			//from http://stackoverflow.com/a/206347/1644720
			// Start the child process.
			var p = new Process();
			p.StartInfo.RedirectStandardError = true;
			// Redirect the output stream of the child process.
			p.StartInfo.WorkingDirectory = Path.GetDirectoryName(self.FileName);
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.FileName = validFile.FromJust;
			var argumentBuilder = new StringBuilder();
			for (var i = 1; i < parameters.Count; i++)
			{
				if (parameters[i].Type == ParamType.ATOM) parameters[i] = ((IAtomNode)parameters[i]).Simplify();
				argumentBuilder.Append(parameters[i].PrettyPrint());
				argumentBuilder.Append(' ');
			}

			argumentBuilder.Append("--to-stdout");
			p.StartInfo.Arguments = argumentBuilder.ToString();
			p.Start();
			// Do not wait for the child process to exit before
			// reading to the end of its redirected stream.
			// p.WaitForExit();
			// Read the output stream first and then wait.
			var outputBytes = new MemoryStream();
			var errorStream = new MemoryStream();
			p.StandardOutput.BaseStream.CopyTo(outputBytes);
			p.StandardError.BaseStream.CopyTo(errorStream);
			p.WaitForExit();

			var output = outputBytes.GetBuffer().Take((int)outputBytes.Length).ToArray();
			if (errorStream.Length > 0)
				parse.Error(self.Location,
					Encoding.ASCII.GetString(errorStream.GetBuffer().Take((int)errorStream.Length).ToArray()));
			else if (output.Length >= 7 && Encoding.ASCII.GetString(output.Take(7).ToArray()) == "ERROR: ")
				parse.Error(self.Location, Encoding.ASCII.GetString(output.Skip(7).ToArray()));
			return new Just<ILineNode>(new DataNode(parse.CurrentOffset, output));
		}
	}
}
