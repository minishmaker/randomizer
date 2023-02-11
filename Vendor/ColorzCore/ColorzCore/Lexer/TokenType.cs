namespace ColorzCore.Lexer
{
    public enum TokenType
    {
        NEWLINE,
        SEMICOLON,
        COLON,
        PREPROCESSOR_DIRECTIVE,
        OPEN_BRACE,
        CLOSE_BRACE,
        OPEN_PAREN,
        CLOSE_PAREN,
        COMMA,
        MUL_OP,
        DIV_OP,
        MOD_OP,
        ADD_OP,
        SUB_OP,
        LSHIFT_OP,
        RSHIFT_OP,
        SIGNED_RSHIFT_OP,
        AND_OP,
        XOR_OP,
        OR_OP,
        NUMBER,
        OPEN_BRACKET,
        CLOSE_BRACKET,
        STRING,
        IDENTIFIER,
        MAYBE_MACRO,
        ERROR //Catch-all for invalid characters.
    }
}
