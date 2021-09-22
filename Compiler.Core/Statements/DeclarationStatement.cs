using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class DeclarationStatement : Statement
    {
        public Id Id { get; }
        public Expression Value1 { get; }
        public Expression Value2 { get; }
        public Expression Value3 { get; }

        //Declaracion arreglos y variables normales
        public DeclarationStatement(Id id, Expression value )
        {
            Id = id;
            Value1 = value;
        }

        //Declaracion DateTime
        public DeclarationStatement(Id id, Expression year, Expression month, Expression day)
        {
            Id = id;
            Value1 = year;
            Value2 = month;
            Value3 = day;
        }

        //Se declara sin asignar valor
        public DeclarationStatement(Id id)
        {
            Id = id;
        }
    }
}
