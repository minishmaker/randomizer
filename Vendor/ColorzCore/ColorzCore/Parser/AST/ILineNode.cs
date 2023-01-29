using System.Collections.Generic;
using ColorzCore.IO;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.AST
{
    internal interface ILineNode
    {
        int Size { get; }
        string PrettyPrint(int indentation);
        void WriteData(Rom rom);
        void EvaluateExpressions(ICollection<Token> undefinedIdentifiers); //Return: undefined identifiers
    }
}
