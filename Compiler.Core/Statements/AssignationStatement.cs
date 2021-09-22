using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class AssignationStatement : Statement
    {
        public Id Id { get; }
        public Expression Value1 { get; }
        public Expression Value2 { get;  }
        public Expression Value3 { get; }

        //Asignacion normal
        public AssignationStatement(Id id, Expression value)
        {
            Id = id;
            Value1 = value;
        }

        //Asignacion arreglo
        public AssignationStatement(Id id, Expression index, Expression value)
        {
            Id = id;
            Value1 = index;
            Value2 = value;
        }

        //Asignacion DateTime
        public AssignationStatement(Id id, Expression year, Expression month, Expression day)
        {
            Id = id;
            Value1 = year;
            Value2 = month;
            Value3 = day;
        }
    }
}
