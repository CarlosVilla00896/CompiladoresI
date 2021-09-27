using Compiler.Core.Expressions;
using System;

namespace Compiler.Core.Statements
{
    public class IfStatement : Statement
    {
        public TypedExpression Expression { get; }
        public Statement Statement { get; }

        public IfStatement(TypedExpression expression, Statement statement)
        {
            Expression = expression;
            Statement = statement;
        }

        public override void ValidateSemantic()
        {
            if (Expression.GetExpressionType() != Type.Bool)
            {
                throw new ApplicationException($"A boolean is required in if statement. Line {Expression.Token.Line}, column {Expression.Token.Column}");
            }
            Statement?.ValidateSemantic();
        }
        public override string Generate()
        {
            throw new NotImplementedException();
        }
    }
}
