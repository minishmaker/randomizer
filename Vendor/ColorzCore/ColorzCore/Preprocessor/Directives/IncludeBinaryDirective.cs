using System;
using System.Collections.Generic;
using System.IO;
using ColorzCore.DataTypes;
using ColorzCore.IO;
using ColorzCore.Lexer;
using ColorzCore.Parser;
using ColorzCore.Parser.AST;

namespace ColorzCore.Preprocessor.Directives
{
    internal class IncludeBinaryDirective : IDirective
    {
        public IncludeFileSearcher FileSearcher { get; set; }
        public int MinParams => 1;

        public int? MaxParams => 1;

        public bool RequireInclusion => true;

        public IMaybe<ILineNode> Execute(EaParser p, Token self, IList<IParamNode> parameters,
            MergeableGenerator<Token> tokens)
        {
            var existantFile = FileSearcher.FindFile(Path.GetDirectoryName(self.FileName), parameters[0].ToString());

            if (!existantFile.IsNothing)
                try
                {
                    var pathname = existantFile.FromJust;
                    return new Just<ILineNode>(new DataNode(p.CurrentOffset, File.ReadAllBytes(pathname)));
                }
                catch (Exception)
                {
                    p.Error(self.Location, "Error reading file \"" + parameters[0].ToString() + "\".");
                }
            else
                p.Error(parameters[0].MyLocation, "Could not find file \"" + parameters[0].ToString() + "\".");

            return new Nothing<ILineNode>();
        }
    }
}
