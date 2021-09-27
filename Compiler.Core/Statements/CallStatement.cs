using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class CallStatement : Statement
    {
        public Id Id { get; }
        public Expression Arguments { get; }
        public Expression Attributes { get; }

        public CallStatement(Id id, Expression arguments, Expression attributes)
        {
            Id = id;
            Arguments = arguments;
            Attributes = attributes;
        }

        public override void ValidateSemantic()
        {
            //ValidateArguments(Attributes, Arguments);
        }
        private void ValidateArguments(Expression attributes, Expression arguments)
        {
            if (attributes == null && arguments == null)
            {
                return;
            }

            if (attributes is BinaryOperator binary && binary.RightExpression == null && (arguments is BinaryOperator))
            {
                throw new ApplicationException($"Incorrect amount of arguments supplied. Line {arguments.Token.Line}, column {arguments.Token.Column}");
            }

            if (attributes is BinaryOperator attr && arguments is BinaryOperator arg)
            {
                ValidateArguments(attr.LeftExpression, arg.LeftExpression);
                ValidateArguments(attr.RightExpression, arg.RightExpression);
            }
            else if (attributes is TypedExpression typedAttr && arguments is TypedExpression typedArg && typedAttr.GetExpressionType() != typedArg.GetExpressionType())
            {
                throw new ApplicationException($"Expected {typedAttr.GetExpressionType()} but received {typedArg.GetExpressionType()}.  Line {arguments.Token.Line}, column {arguments.Token.Column}");
            }

        }
        public override string Generate()
        {
            throw new NotImplementedException();
        }
    }
}
