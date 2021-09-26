using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class ForStatement : Statement
    {
        public Statement Declaration { get; set; }
        public Expression Expression1 { get; }
        public Expression Expression2 { get; }
        public Statement Block { get; }

        public ForStatement(Statement declaration, Expression expression1, Expression expression2, Statement block)
        {
            Declaration = declaration;
            Expression1 = expression1;
            Expression2 = expression2;
            Block = block;
        }

        public override void ValidateSemantic()
        {
            throw new NotImplementedException();
        }

        public override string Generate()
        {
            throw new NotImplementedException();
        }
    }
}
