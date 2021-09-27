using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class WhileStatement : Statement
    {
        public TypedExpression Expression { get; }
        public Statement Statement { get; }
        public WhileStatement(TypedExpression expression, Statement statement)
        {
            Expression = expression;
            Statement = statement;
        }

        public override void ValidateSemantic()
        {
            if (Expression.GetExpressionType() != Type.Bool)
            {
                throw new ApplicationException($"A boolean is required in while. Line {Expression.Token.Line}, column {Expression.Token.Column}");
            }
            Statement?.ValidateSemantic();
        }

        public override string Generate()
        {
            throw new NotImplementedException();
        }
    }
}
