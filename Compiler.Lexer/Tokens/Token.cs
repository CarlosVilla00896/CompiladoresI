﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Lexer.Tokens
{
    public class Token
    {

        public TokenType TokenType { get; set; }
        public string Lexeme { get; set; }
        public int? PositionInSymbolTable { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public override string ToString()
        {
            return $"{ Lexeme } Type: {TokenType} Line: {Line} Column: { Column }";
        }

    }
}
