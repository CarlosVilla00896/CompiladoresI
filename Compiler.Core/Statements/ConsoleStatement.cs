using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class ConsoleStatement : Statement
    {
        public Expression Expression { get; }
        public ConsoleStatement(Expression expression)
        {
            Expression = expression;
        }

        public ConsoleStatement()
        {

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
