using System.Collections.Generic;
using System.Text;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser.Macros
{
    internal class String : BuiltInMacro
    {
        public override IEnumerable<Token> ApplyMacro(Token head, IList<IList<Token>> parameters,
            ImmutableStack<Closure> scopes)
        {
            yield return new Token(TokenType.IDENTIFIER, head.Location, "BYTE");
            foreach (var num in
                     Encoding.ASCII.GetBytes(parameters[0][0].Content.ToCharArray())) //TODO: Errors if not adherent?
                yield return new Token(TokenType.NUMBER, head.Location, num.ToString());
        }

        public override bool ValidNumParams(int num)
        {
            return num == 1;
        }
    }
}
