using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public abstract class TypedBinaryOperator : TypedExpression
    {
        public TypedExpression LeftExpression { get; }
        public TypedExpression RightExpression { get; }
        public TypedBinaryOperator(Token token, TypedExpression leftExpression, TypedExpression rightExpression, Type type) 
            : base(token, type)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
        }

    }
}
