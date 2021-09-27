using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class LogicalExpression : TypedBinaryOperator
    {
        private readonly Dictionary<(Type, Type), Type> _typeRules;
        public LogicalExpression(Token token, TypedExpression leftExpression, TypedExpression rightExpression) 
            : base(token, leftExpression, rightExpression, null)
        {
            _typeRules = new Dictionary<(Type, Type), Type>
            {
                { (Type.Bool, Type.Bool), Type.Bool },
            };
        }

        public override string Generate()
        {
            return $"({LeftExpression.Generate()} {Token.Lexeme} {RightExpression.Generate()})";
        }

        public override Type GetExpressionType()
        {
            if (_typeRules.TryGetValue((LeftExpression.GetExpressionType(), RightExpression.GetExpressionType()), out var resultType))
            {
                return resultType;
            }

            throw new ApplicationException($"Cannot perform logical operation between {LeftExpression.GetExpressionType()} and {RightExpression.GetExpressionType()}. Line {Token.Line}, column {Token.Column}");
        }
    }
}
