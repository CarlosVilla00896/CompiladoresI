﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class SequenceStatement : Statement
    {
        public Statement FirstStatement { get; private set; }
        public Statement NextStatement { get; private set; }

        public SequenceStatement(Statement firstStatement, Statement nextStatement)
        {
            FirstStatement = firstStatement;
            NextStatement = nextStatement;
        }

        public override void ValidateSemantic()
        {
            FirstStatement?.ValidateSemantic();
            NextStatement?.ValidateSemantic();
        }

        public override string Generate()
        {

            var code = FirstStatement?.Generate();
            code += NextStatement?.Generate();
            return code;
        }
    }
}
