using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class UnaryOperator : TypedExpression
    {
        public UnaryOperator(Token token, TypedExpression expression) 
            : base(token, null)
        {
            Expression = expression;
        }

        public TypedExpression Expression { get; }

        public override Type GetExpressionType()
        {
            return Expression.GetExpressionType();
        }
    }
}
