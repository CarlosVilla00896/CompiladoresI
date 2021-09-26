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
            return;
        }

        public override string Generate()
        {
            throw new NotImplementedException();
        }
    }
}
