using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class AssignationStatement : Statement
    {
        public Id Id { get; }
        public TypedExpression Value1 { get; }
        public TypedExpression Value2 { get;  }
        public TypedExpression Value3 { get; }
        public string AssignmentType { get; } = "";

        //Asignacion normal
        public AssignationStatement(Id id, TypedExpression value, string assignmentType)
        {
            Id = id;
            Value1 = value;
            AssignmentType = assignmentType;
        }

        //Asignacion arreglo
        public AssignationStatement(Id id, TypedExpression index, TypedExpression value, string assignmentType)
        {
            Id = id;
            Value1 = index;
            Value2 = value;
            AssignmentType = assignmentType;
        }

        //Asignacion DateTime
        public AssignationStatement(Id id, TypedExpression year, TypedExpression month, TypedExpression day, string assignmentType)
        {
            Id = id;
            Value1 = year;
            Value2 = month;
            Value3 = day;
            AssignmentType = assignmentType;
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
                    if (Id.GetExpressionType() != Value2.GetExpressionType())
                    {
                        throw new ApplicationException($"Type {Value2.GetExpressionType()} is not assignable to {Id.GetExpressionType()}");
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
