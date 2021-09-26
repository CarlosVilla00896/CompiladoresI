using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class ReturnStatement : Statement
    {
        public TypedExpression Expression { get; }
        public ReturnStatement(TypedExpression expression)
        {
            Expression = expression;
        }

        public override void ValidateSemantic()
        {
            return;
        }

        public override string Generate()
        {
            throw new NotImplementedException();
        }
    }
}
