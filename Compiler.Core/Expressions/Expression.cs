using Compiler.Lexer.Tokens;
using System;

namespace Compiler.Core.Expressions
{
    public class Expression : Node
    {
        public Token Token { get; private set; }
        public Type Type { get; private set; }
        public Expression(Token token, Type type)
        {
            Token = token;
            Type = type;
        }
        public Expression()
        {

        }
    }
}
