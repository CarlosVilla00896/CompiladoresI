using Compiler.Lexer.Tokens;
using System;

namespace Compiler.Core.Expressions
{
    public class Expression : Node
    {
        public Token Token { get; private set; }
        protected readonly Type type;
        public Expression(Token token, Type type)
        {
            Token = token;
            this.type = type;
        }
        public Expression()
        {

        }
    }
}
