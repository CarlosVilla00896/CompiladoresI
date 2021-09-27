using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class ElseStatement : Statement
    {
        public TypedExpression Expression { get; }
        public Statement TrueStatement { get; }
        public Statement FalseStatement { get; }
        public ElseStatement(TypedExpression expression, Statement trueStatement, Statement falseStatement)
        {
            Expression = expression;
            TrueStatement = trueStatement;
            FalseStatement = falseStatement;
        }

        public override void ValidateSemantic()
        {
            if (Expression.GetExpressionType() != Type.Bool)
            {
                throw new ApplicationException($"A boolean is required in if statement. Line {Expression.Token.Line} column {Expression.Token.Column}");
            }
            TrueStatement?.ValidateSemantic();
            FalseStatement?.ValidateSemantic();
        }

        public override string Generate()
        {
            var code = String.Empty;
            code += $"if({Expression.Generate()}){{ {Environment.NewLine}";
            code += $"{TrueStatement.Generate()}{Environment.NewLine}";
            //for (int i = 0; i < tabs; i++)
            //{
            //    code += "\t";
            //}
            code += $"}} else {{{Environment.NewLine}";
            code += $"{FalseStatement.Generate()}{Environment.NewLine}}}{Environment.NewLine}";
            return code;
        }
    }
}
