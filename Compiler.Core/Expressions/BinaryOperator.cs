using Compiler.Lexer.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class BinaryOperator : Operator
    {
        public Expression LeftExpression { get; }
        public Expression RightExpression { get; }
        public BinaryOperator(Token token, Expression leftExpression, Expression rightExpression, Type type) 
            : base(token, type)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
        }
    }
}
