using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;
using ColorzCore.Parser;
using ColorzCore.Parser.AST;

namespace ColorzCore.Preprocessor.Directives
{
    internal class UndefineDirective : IDirective
    {
        public int MinParams => 1;

        public int? MaxParams => null;

        public bool RequireInclusion => true;

        public IMaybe<ILineNode> Execute(EaParser p, Token self, IList<IParamNode> parameters,
            MergeableGenerator<Token> tokens)
        {
            foreach (var parm in parameters)
            {
                var s = parm.ToString();
                if (p.Definitions.ContainsKey(s))
                    p.Definitions.Remove(parm.ToString());
                else
                    p.Warning(parm.MyLocation, "Undefining non-existant definition: " + s);
            }

            return new Nothing<ILineNode>();
        }
    }
}
