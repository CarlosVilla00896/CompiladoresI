using System;
using System.Collections.Generic;
using System.Text;
using Compiler.Lexer.Tokens;

namespace Compiler.Lexer
{
    public interface IScanner
    {
        Token GetNextToken();
    }
}
