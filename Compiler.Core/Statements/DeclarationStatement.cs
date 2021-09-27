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
        public string AssignmentType { get; }

        //Declaracion arreglos y variables normales
        public DeclarationStatement(Id id, TypedExpression value, string assignmentType)
        {
            Id = id;
            Value1 = value;
            AssignmentType = assignmentType;
        }

        //Declaracion DateTime
        public DeclarationStatement(Id id, TypedExpression year, TypedExpression month, TypedExpression day, string assignmentType)
        {
            Id = id;
            Value1 = year;
            Value2 = month;
            Value3 = day;
            AssignmentType = assignmentType;
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
                        throw new ApplicationException($"Type {Value1.GetExpressionType()} is not assignable to {Id.GetExpressionType()}. Line {Id.Token.Line} column {Id.Token.Column}");
                    }
                    break;
                case "array":
                    if (Value1.GetExpressionType() != Type.Int)
                    {
                        throw new ApplicationException($"int type expected, but received {Value1.GetExpressionType()}. Line {Value1.Token.Line} column {Value1.Token.Column}");
                    }
                    break;
                case "date":
                    if (Value1.GetExpressionType() != Type.Int)
                    {
                        throw new ApplicationException($"int type expected, but received {Value1.GetExpressionType()}. Line {Value1.Token.Line} column {Value1.Token.Column}");
                    }
                    if (Value2.GetExpressionType() != Type.Int)
                    {
                        throw new ApplicationException($"int type expected, but received {Value2.GetExpressionType()}. Line {Value2.Token.Line} column {Value2.Token.Column}");
                    }
                    if (Value3.GetExpressionType() != Type.Int)
                    {
                        throw new ApplicationException($"int type expected, but received {Value3.GetExpressionType()}. Line {Value3.Token.Line} column {Value3.Token.Column}");
                    }
                    break;
                default:
                    break;
            }
        }

        public override string Generate()
        {
            var code = "";
            switch (AssignmentType)
            {
                case "normal":
                    code += $"var {Id.Generate()} = {Value1.Generate()};{Environment.NewLine}";
                    break;
                case "array":
                    code += $"var {Id.Generate()} = new Array({Value1.Generate()});{Environment.NewLine}";
                    break;
                case "date":
                    code += $"var {Id.Generate()} = new Date ({Value1.Generate()}, {Value2.Generate()}, {Value3.Generate()});{Environment.NewLine}";
                    break;
                default:
                    code += $"var {Id.Generate()}; {Environment.NewLine}";
                    break;
            }
            return code;
        }
    }
}
