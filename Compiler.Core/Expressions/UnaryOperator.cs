using Compiler.Lexer.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class UnaryOperator : Expression
    {
        public UnaryOperator(Token token, TypedExpression expression) 
            : base(token, null)
        {
            Expression = expression;
        }

        public TypedExpression Expression { get; }
    }
}
