using Compiler.Lexer.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class LogicalExpression : BinaryOperator
    {
        public LogicalExpression(Token token, Expression leftExpression, Expression rightExpression) 
            : base(token, leftExpression, rightExpression, null)
        {
        }
    }
}
