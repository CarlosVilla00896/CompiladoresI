using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class Statement : Node
    {
        public Statement()
        {

        }
        public static Statement Null => new Statement();
    }
}
