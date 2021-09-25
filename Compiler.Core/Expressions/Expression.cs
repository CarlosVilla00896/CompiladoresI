using System;

namespace Compiler.Core.Expressions
{
    public abstract class Expression : Node
    {
        public Token Token { get; }
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
