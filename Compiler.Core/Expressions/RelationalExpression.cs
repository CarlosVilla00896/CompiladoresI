using Compiler.Lexer.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class RelationalExpression : TypedBinaryOperator
    {
        public RelationalExpression(Token token, TypedExpression leftExpression, TypedExpression rightExpression) 
            : base(token, leftExpression, rightExpression, null)
        {
        }
    }
}
