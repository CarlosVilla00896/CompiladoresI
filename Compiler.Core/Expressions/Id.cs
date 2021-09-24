﻿using Compiler.Lexer.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class Id : TypedExpression
    {
        public Id(Token token, Type type) : base(token, type)
        {
        }
    }
}