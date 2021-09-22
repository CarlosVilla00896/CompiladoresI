using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class WhileStatement : Statement
    {
        public Expression Expression { get; }
        public Statement Statement { get; }
        public WhileStatement(Expression expression, Statement statement)
        {
            Expression = expression;
            Statement = statement;
        }
    }
}
