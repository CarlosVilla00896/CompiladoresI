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
        public CallStatement(Id id, Expression arguments)
        {
            Id = id;
            Arguments = arguments;
        }
    }
}
