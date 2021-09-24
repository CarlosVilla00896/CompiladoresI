using Compiler.Lexer.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class BinaryOperator : Expression
    {
        public TypedExpression LeftExpression { get; }
        public TypedExpression RightExpression { get; }
        public BinaryOperator(Token token, TypedExpression leftExpression, TypedExpression rightExpression, Type type) 
            : base(token, type)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
        }
    }
}
