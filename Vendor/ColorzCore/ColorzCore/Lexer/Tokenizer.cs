using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ColorzCore.IO;

namespace ColorzCore.Lexer
{
    internal class Tokenizer
    {
        public const int MaxIdLength = 64;
        public static readonly Regex NumRegex = new Regex("\\G([01]+b|0x[\\da-fA-F]+|\\$[\\da-fA-F]+|\\d+)");
        public static readonly Regex IdRegex = new Regex("\\G([a-zA-Z_][a-zA-Z0-9_]*)");

        public static readonly Regex
            StringRegex = new Regex("\\G(([^\\\"]|\\\\\\\")*)"); //"\\G(([^\\\\\\\"]|\\\\[rnt\\\\\\\"])*)");

        public static readonly Regex WinPathnameRegex =
            new Regex(string.Format("\\G([^ \\{0}]|\\ |\\\\)+", Process(Path.GetInvalidPathChars())));

        public static readonly Regex PreprocDirectiveRegex = new Regex("\\G(#[a-zA-Z_][a-zA-Z0-9_]*)");
        public static readonly Regex WordRegex = new Regex("\\G([^\\s]+)");

        private int _multilineCommentNesting;

        public Tokenizer()
        {
            _multilineCommentNesting = 0;
        }

        private static string Process(char[] chars)
        {
            var sb = new StringBuilder();
            foreach (var c in chars)
            {
                switch (c)
                {
                    case '.':
                    case '\\':
                    case '+':
                    case '*':
                    case '?':
                    case '^':
                    case '$':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                    case '(':
                    case ')':
                    case '|':
                    case '/':
                        sb.Append('\\');
                        break;
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

        public IEnumerable<Token> TokenizePhrase(string line, string fileName, int lineNum, int startOffs, int endOffs,
            int offset = 0)
        {
            bool afterInclude = false, afterDirective = false, afterWhitespace = false;

            var curCol = startOffs;
            while (curCol < endOffs)
            {
                var nextChar = line[curCol];
                if (_multilineCommentNesting > 0)
                {
                    if (nextChar == '*' && curCol + 1 < endOffs && line[curCol + 1] == '/')
                    {
                        _multilineCommentNesting -= 1;
                        curCol += 2;
                        continue;
                    }

                    if (nextChar == '/' && curCol + 1 < endOffs && line[curCol + 1] == '*')
                    {
                        _multilineCommentNesting += 1;
                        curCol += 2;
                        continue;
                    }

                    curCol++;
                    continue;
                }

                if (char.IsWhiteSpace(nextChar) && nextChar != '\n')
                {
                    curCol++;
                    afterWhitespace = true;
                    continue;
                }

                switch (nextChar)
                {
                    case ';':
                        yield return new Token(TokenType.SEMICOLON, fileName, lineNum, curCol + offset);
                        break;
                    case ':':
                        yield return new Token(TokenType.COLON, fileName, lineNum, curCol + offset);
                        break;
                    case '{':
                        yield return new Token(TokenType.OPEN_BRACE, fileName, lineNum, curCol + offset);
                        break;
                    case '}':
                        yield return new Token(TokenType.CLOSE_BRACE, fileName, lineNum, curCol + offset);
                        break;
                    case '[':
                        yield return new Token(TokenType.OPEN_BRACKET, fileName, lineNum, curCol + offset);
                        break;
                    case ']':
                        yield return new Token(TokenType.CLOSE_BRACKET, fileName, lineNum, curCol + offset);
                        break;
                    case '(':
                        yield return new Token(TokenType.OPEN_PAREN, fileName, lineNum, curCol + offset);
                        break;
                    case ')':
                        yield return new Token(TokenType.CLOSE_PAREN, fileName, lineNum, curCol + offset);
                        break;
                    case '*':
                        yield return new Token(TokenType.MUL_OP, fileName, lineNum, curCol + offset);
                        break;
                    case '%':
                        yield return new Token(TokenType.MOD_OP, fileName, lineNum, curCol + offset);
                        break;
                    case ',':
                        yield return new Token(TokenType.COMMA, fileName, lineNum, curCol + offset);
                        break;
                    case '/':
                        if (curCol + 1 < endOffs && line[curCol + 1] == '/')
                        {
                            //Is a comment, ignore rest of line
                            curCol = endOffs;
                        }
                        else if (curCol + 1 < endOffs && line[curCol + 1] == '*')
                        {
                            _multilineCommentNesting += 1;
                            curCol += 2;
                            continue;
                        }
                        else
                        {
                            yield return new Token(TokenType.DIV_OP, fileName, lineNum, curCol + offset);
                        }

                        break;
                    case '+':
                        yield return new Token(TokenType.ADD_OP, fileName, lineNum, curCol + offset);
                        break;
                    case '-':
                        if (afterWhitespace && afterDirective)
                        {
                            var wsDelimited = WordRegex.Match(line, curCol, Math.Min(260, endOffs - curCol));
                            if (wsDelimited.Success)
                            {
                                var match = wsDelimited.Value;
                                yield return new Token(TokenType.STRING, fileName, lineNum, curCol,
                                    IoUtility.UnescapePath(match));
                                curCol += match.Length;
                                continue;
                            }
                        }

                        yield return new Token(TokenType.SUB_OP, fileName, lineNum, curCol + offset);
                        break;
                    case '&':
                        yield return new Token(TokenType.AND_OP, fileName, lineNum, curCol + offset);
                        break;
                    case '^':
                        yield return new Token(TokenType.XOR_OP, fileName, lineNum, curCol + offset);
                        break;
                    case '|':
                        yield return new Token(TokenType.OR_OP, fileName, lineNum, curCol + offset);
                        break;
                    case '\"':
                    {
                        curCol++;
                        var quoteInterior = StringRegex.Match(line, curCol, endOffs - curCol);
                        var match = quoteInterior.Value;
                        yield return new Token(TokenType.STRING, fileName, lineNum,
                            curCol, /*IOUtility.UnescapeString(*/match /*)*/);
                        curCol += match.Length;
                        if (curCol == endOffs || line[curCol] != '\"')
                            yield return new Token(TokenType.ERROR, fileName, lineNum, curCol, "Unclosed string.");
                        break;
                    }
                    case '<':
                        if (curCol + 1 < endOffs && line[curCol + 1] == '<')
                        {
                            yield return new Token(TokenType.LSHIFT_OP, fileName, lineNum, curCol + offset);
                            curCol++;
                            break;
                        }

                        yield return new Token(TokenType.ERROR, fileName, lineNum, curCol, "<");
                        break;
                    case '>':
                        if (curCol + 1 < endOffs && line[curCol + 1] == '>')
                        {
                            if (curCol + 2 < endOffs && line[curCol + 2] == '>')
                            {
                                yield return new Token(TokenType.SIGNED_RSHIFT_OP, fileName, lineNum, curCol + offset);
                                curCol += 2;
                            }
                            else
                            {
                                yield return new Token(TokenType.RSHIFT_OP, fileName, lineNum, curCol + offset);
                                curCol++;
                            }

                            break;
                        }

                        yield return new Token(TokenType.ERROR, fileName, lineNum, curCol, ">");
                        break;
                    case '\n':
                        yield return new Token(TokenType.NEWLINE, fileName, lineNum, curCol + offset);
                        break;
                    default:
                        if (afterInclude)
                        {
                            var winPath = WinPathnameRegex.Match(line, curCol, Math.Min(260, endOffs - curCol));
                            if (winPath.Success)
                            {
                                var match = winPath.Value;
                                yield return new Token(TokenType.STRING, fileName, lineNum, curCol,
                                    IoUtility.UnescapePath(match));
                                curCol += match.Length;
                                afterInclude = false;
                                continue;
                            }
                        }
                        else
                        {
                            //Try matching to identifier, then to number
                            //TODO: Restrict Macro invocations to a MAYBE_MACRO that must preceed a (, with no whitespace.
                            var idMatch = IdRegex.Match(line, curCol, Math.Min(MaxIdLength, endOffs - curCol));
                            if (idMatch.Success)
                            {
                                var match = idMatch.Value;
                                var idCol = curCol;
                                curCol += match.Length;
                                if (curCol < endOffs && line[curCol] == '(')
                                    yield return new Token(TokenType.MAYBE_MACRO, fileName, lineNum, idCol, match);
                                else
                                    yield return new Token(TokenType.IDENTIFIER, fileName, lineNum, idCol, match);
                                if (curCol < endOffs && char.IsLetterOrDigit(line[curCol]) | (line[curCol] == '_'))
                                {
                                    var idMatch2 = new Regex("[a-zA-Z0-9_]+").Match(line, curCol, endOffs - curCol);
                                    match = idMatch2.Value;
                                    yield return new Token(TokenType.ERROR, fileName, lineNum, curCol,
                                        string.Format("Identifier longer than {0} characters.", MaxIdLength));
                                    curCol += match.Length;
                                }

                                continue;
                            }

                            var numMatch = NumRegex.Match(line, curCol, endOffs - curCol);
                            if (numMatch.Success)
                            {
                                var match = numMatch.Value;
                                //Verify that next token isn't start of an identifier
                                if (curCol + match.Length >= endOffs || (!char.IsLetter(line[curCol + match.Length]) &&
                                                                         line[curCol + match.Length] != '_'))
                                {
                                    yield return new Token(TokenType.NUMBER, fileName, lineNum, curCol,
                                        match.TrimEnd());
                                    curCol += match.Length;
                                    continue;
                                }
                            }

                            var directiveMatch = PreprocDirectiveRegex.Match(line, curCol,
                                Math.Min(MaxIdLength + 1, endOffs - curCol));
                            if (directiveMatch.Success)
                            {
                                var match = directiveMatch.Value;
                                yield return new Token(TokenType.PREPROCESSOR_DIRECTIVE, fileName, lineNum, curCol,
                                    match);
                                curCol += match.Length;
                                if (match.Substring(1).Equals("include") || match.Substring(1).Equals("incbin"))
                                    afterInclude = true;
                                afterDirective = true;
                                continue;
                            }
                        }

                        var restOfWord = new Regex("\\G\\S+").Match(line, curCol, endOffs - curCol).Value;
                        yield return new Token(TokenType.ERROR, fileName, lineNum, curCol, restOfWord);
                        curCol += restOfWord.Length;
                        continue;
                }

                curCol++;
                afterInclude = false;
                afterWhitespace = false;
            }
        }

        public IEnumerable<Token> TokenizeLine(string line, string fileName, int lineNum, int offset = 0)
        {
            return TokenizePhrase(line, fileName, lineNum, 0, line.Length, offset);
        }

        /***
         *   All Token streams end in a NEWLINE.
         * 
         */
        public IEnumerable<Token> Tokenize(Stream input, string fileName)
        {
            var sin = new StreamReader(input);
            var curLine = 1;
            while (!sin.EndOfStream)
            {
                var line = sin.ReadLine();
                foreach (var t in TokenizeLine(line, fileName, curLine)) yield return t;
                yield return new Token(TokenType.NEWLINE, fileName, curLine, line.Length);
                curLine++;
            }
        }

        public IEnumerable<Token> Tokenize(FileStream fs)
        {
            foreach (var t in Tokenize(fs, fs.Name))
                yield return t;
            fs.Close();
        }
    }
}
