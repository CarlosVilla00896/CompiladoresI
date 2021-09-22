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
    }
}
