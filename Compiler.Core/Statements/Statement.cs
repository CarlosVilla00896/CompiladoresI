using Compiler.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public abstract class Statement : Node, ISemanticValidation
    {
        public Statement()
        {

        }
        public abstract void ValidateSemantic();
        public abstract string Generate();
    }
}
