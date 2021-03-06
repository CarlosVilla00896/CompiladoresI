using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public abstract class TypedExpression : Expression
    {
        public TypedExpression(Token token, Type type) 
            : base(token, type)
        {
        }

        public abstract Type GetExpressionType();
    }
}
