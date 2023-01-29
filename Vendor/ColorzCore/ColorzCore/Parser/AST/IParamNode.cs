using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
    public interface IParamNode
    {
        ParamType Type { get; }
        Location MyLocation { get; }
        string ToString(); //For use in other programs.
        string PrettyPrint();
        IMaybe<IParamNode> Evaluate(ICollection<Token> unidentifiedIdentifiers);

        IEither<int, string> TryEvaluate();
    }
}
