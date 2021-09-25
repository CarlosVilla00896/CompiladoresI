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
        public Expression Attributes { get; }

        public CallStatement(Id id, Expression arguments, Expression attributes)
        {
            Id = id;
            Arguments = arguments;
            Attributes = attributes;
            Attributes = attributes;
        }
    }
}
