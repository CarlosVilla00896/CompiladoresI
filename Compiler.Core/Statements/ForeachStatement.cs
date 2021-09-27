using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class ForeachStatement : Statement
    {
        public Id ElementId { get; }
        public Id ArrayId { get; }
        public Statement Block { get; }
        public ForeachStatement(Id elementId, Id arrayId, Statement block)
        {
            ElementId = elementId;
            ArrayId = arrayId;
            Block = block;
        }

        public override void ValidateSemantic()
        {
            if(ElementId.GetExpressionType() != ArrayId.GetExpressionType())
            {
                throw new ApplicationException($"Element type {ElementId.GetExpressionType()} must be the same as array type {ArrayId.GetExpressionType()}. Line {ElementId.Token.Line} column {ElementId.Token.Column}");
            }
            Block?.ValidateSemantic();
        }

        public override string Generate()
        {
            throw new NotImplementedException();
        }
    }
}
