using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;
using ColorzCore.Parser;
using ColorzCore.Parser.AST;

namespace ColorzCore.Preprocessor.Directives
{
    internal interface IDirective
    {
        /***
         * Minimum number of parameters, inclusive. 
         */
        int MinParams { get; }

        /***
         * Maximum number of parameters, inclusive. Null for no limit.
         */
        int? MaxParams { get; }

        /***
         * Whether requires the parser to be taking in tokens.
         * This may not hold when the parser is skipping, e.g. from an #ifdef.
         */
        bool RequireInclusion { get; }

        /***
         * Perform the directive's action, be it altering tokens, for just emitting a special ILineNode.
         * Precondition: MinParams <= parameters.Count <= MaxParams
         * 
         * Return: If a string is returned, it is interpreted as an error.
         */
        IMaybe<ILineNode> Execute(EaParser p, Token self, IList<IParamNode> parameters,
            MergeableGenerator<Token> tokens);
    }
}
