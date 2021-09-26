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
                throw new ApplicationException("A boolean is required in ifs");
            }
        }
        public override string Generate()
        {
            throw new NotImplementedException();
        }
    }
}
