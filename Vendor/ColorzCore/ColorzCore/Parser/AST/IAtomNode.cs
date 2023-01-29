using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
    public interface IAtomNode : IParamNode
    {
        //TODO: Simplify() partial evaluation as much as is defined, to save on memory space.
        int Precedence { get; }
        int ToInt(); //May throw errors. TODO: Remove and only do calls through TryEvaluate?
        IMaybe<string> GetIdentifier();
        IEnumerable<Token> ToTokens();
        bool CanEvaluate();
        IAtomNode Simplify();
        new IMaybe<int> Evaluate(ICollection<Token> undefinedIdentifiers);
    }
}
