using Compiler.Lexer.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core
{
    public class Type
    {
        public string Lexeme { get; private set; }
        public TokenType TokenType { get; private set; }
        public Type(string lexeme, TokenType tokenType)
        {
            Lexeme = Lexeme;
            TokenType = tokenType;
        }
        public static Type Int => new Type("int", TokenType.IntKeyword);
        public static Type Float => new Type("float", TokenType.FloatKeyword);
        public static Type Bool => new Type("bool", TokenType.BoolKeyword);
        public static Type DateTime => new Type("DateTime", TokenType.DateTimeKeyword);
        public static Type String => new Type("string", TokenType.StringLiteral);
    }
}
