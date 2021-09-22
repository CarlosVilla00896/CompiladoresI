using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class ReturnStatement : Statement
    {
        public Expression Expression { get; }
        public ReturnStatement(Expression expression)
        {
            Expression = expression;
        }

    }
}
