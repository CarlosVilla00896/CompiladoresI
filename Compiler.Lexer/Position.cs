using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Lexer
{
    public readonly struct Position
    {
        public int Absolute { get; }
        public int Line { get; }
        public int Column { get; }
        public Position(int absolute, int line, int column)
        {
            Absolute = absolute;
            Line = line;
            Column = column;
        }
        public static Position Start => new Position(0, 1, 1);
        public Position MovePointer(char @char)
        {
            return @char == '\n'
                    ? new Position(Absolute + 1, Line + 1, 1)
                    : new Position(Absolute + 1, Line, Column + 1);
        }
    }
}