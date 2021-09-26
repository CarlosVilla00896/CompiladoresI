using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class DeclarationStatement : Statement
    {
        public Id Id { get; }
        public TypedExpression Value1 { get; }
        public TypedExpression Value2 { get; }
        public TypedExpression Value3 { get; }
        public string AssignmentType { get; } = "";

        //Declaracion arreglos y variables normales
        public DeclarationStatement(Id id, TypedExpression value, string assignmentType)
        {
            Id = id;
            Value1 = value;
        }

        //Declaracion DateTime
        public DeclarationStatement(Id id, TypedExpression year, TypedExpression month, TypedExpression day, string assignmentType)
        {
            Id = id;
            Value1 = year;
            Value2 = month;
            Value3 = day;
        }

        //Se declara sin asignar valor
        public DeclarationStatement(Id id)
        {
            Id = id;
        }

        public override void ValidateSemantic()
        {
            switch (AssignmentType)
            {
                case "normal":
                    if (Id.GetExpressionType() != Value1.GetExpressionType())
                    {
                        throw new ApplicationException($"Type {Value1.GetExpressionType()} is not assignable to {Id.GetExpressionType()}");
                    }
                    break;
                case "array":
                    if (Value1.GetExpressionType() != Type.Int)
                    {
                        throw new ApplicationException($"int type expected, but received {Value1.GetExpressionType()}");
                    }
                    break;
                case "date":
                    if (Value1.GetExpressionType() != Type.Int)
                    {
                        throw new ApplicationException($"int type expected, but received {Value1.GetExpressionType()}");
                    }
                    if (Value2.GetExpressionType() != Type.Int)
                    {
                        throw new ApplicationException($"int type expected, but received {Value2.GetExpressionType()}");
                    }
                    if (Value3.GetExpressionType() != Type.Int)
                    {
                        throw new ApplicationException($"int type expected, but received {Value2.GetExpressionType()}");
                    }
                    break;
                default:
                    break;
            }
        }

        public override string Generate()
        {
            throw new NotImplementedException();
        }
    }
}
