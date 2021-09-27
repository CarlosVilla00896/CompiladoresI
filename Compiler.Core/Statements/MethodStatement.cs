using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class MethodStatement : Statement
    {
        public Id Id { get; }
        public Expression Parameters { get; }
        public Statement Block { get; }
        public MethodStatement(Id id, Expression parameters, Statement block)
        {
            Id = id;
            Parameters = parameters;
            Block = block;
        }

        public override void ValidateSemantic()
        {
            Block?.ValidateSemantic();
            
        }

        public override string Generate()
        {
            var code = "";
            var innerCode = InnerCodeGenerateCode(Parameters);
            code += $"function {Id.Generate()}({innerCode}){{{Environment.NewLine}";
            code += $"{Block.Generate()}{{{Environment.NewLine}}}{Environment.NewLine}";
            return code;
        }

        private string InnerCodeGenerateCode(Expression arguments)
        {
            var code = string.Empty;
            if (arguments is BinaryOperator binary)
            {
                code += $"{InnerCodeGenerateCode(binary.LeftExpression)}, " ;
                code += $"{InnerCodeGenerateCode(binary.RightExpression)}";
            }
            else
            {
                code += arguments.Generate();
            }
            return code;
        }
    }
}
