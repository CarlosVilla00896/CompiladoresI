using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class RelationalExpression : TypedBinaryOperator
    {
        private readonly Dictionary<(Type, Type), Type> _typeRules;
        public RelationalExpression(Token token, TypedExpression leftExpression, TypedExpression rightExpression) 
            : base(token, leftExpression, rightExpression, null)
        {
            _typeRules = new Dictionary<(Type, Type), Type>
            {
                { (Type.Float, Type.Float), Type.Bool },
                { (Type.Int, Type.Int), Type.Bool },
                { (Type.String, Type.String), Type.Bool },
                { (Type.Bool, Type.Bool), Type.Bool },
                { (Type.DateTime, Type.DateTime), Type.Bool },
                { (Type.Float, Type.Int), Type.Bool },
                { (Type.Int, Type.Float), Type.Bool },
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

            throw new ApplicationException($"Cannot perform relational operation between {LeftExpression.GetExpressionType()} and {RightExpression.GetExpressionType()}. Line {Token.Line}, column {Token.Column}");
        }
    }
}
