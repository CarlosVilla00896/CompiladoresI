using Compiler.Lexer.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class TypedExpression : Expression
    {
        public TypedExpression(Token token, Type type) 
            : base(token, type)
        {
        }
    }
}
