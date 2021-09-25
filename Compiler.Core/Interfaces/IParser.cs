using Compiler.Core.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Interfaces
{
    public interface IParser
    {
        Statement Parse();
    }
}
