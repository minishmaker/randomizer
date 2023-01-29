using ColorzCore.DataTypes;

namespace ColorzCore.Lexer
{
    public class Token
    {
        public Token(TokenType type, string fileName, int lineNum, int colNum, string original = "")
        {
            Type = type;
            Location = new Location(fileName, lineNum, colNum + 1);
            Content = original;
        }

        public Token(TokenType type, Location newLoc, string content)
        {
            Type = type;
            Location = newLoc;
            Content = content;
        }

        public Location Location { get; }

        public TokenType Type { get; }
        public string FileName => Location.file;
        public int LineNumber => Location.lineNum;
        public int ColumnNumber => Location.colNum;
        public string Content { get; }

        public override string ToString()
        {
            return string.Format("File {4}, Line {0}, Column {1}, {2}: {3}", LineNumber, ColumnNumber, Type, Content,
                FileName);
        }
    }
}
