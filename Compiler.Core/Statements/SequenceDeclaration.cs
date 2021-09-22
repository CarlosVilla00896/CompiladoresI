using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class SequenceDeclaration : Statement
    {
        public Statement FirstDeclaration { get; private set; }
        public Statement NextDeclaration { get; private set; }

        public SequenceDeclaration(Statement firstDeclaration, Statement nextDeclaration)
        {
            FirstDeclaration = firstDeclaration;
            NextDeclaration = nextDeclaration;
        }
    }
}
