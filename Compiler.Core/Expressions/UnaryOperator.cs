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
            switch (Token.TokenType)
            {
                case TokenType.LogicalNegation:
                    if( Expression.GetExpressionType() != Type.Bool)
                    {
                        throw new ApplicationException($"Cannot perform logical negation operation on type {Expression.GetExpressionType()}, bool type expected. Line {Token.Line}, column {Token.Column}");
                    }
                    break;
                case TokenType.Increase:
                case TokenType.Decrease:
                    if (Expression.GetExpressionType() != Type.Int && Expression.GetExpressionType() != Type.Float)
                    {
                        throw new ApplicationException($"Cannot perform unary operation '{Token.Lexeme}' on type {Expression.GetExpressionType()}. Line {Token.Line}, column {Token.Column}");
                    }
                    break;
                default:
                    break;
            }
            return Expression.GetExpressionType();
        }
    }
}
