using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Lexer.Tokens
{
    public enum TokenType
    {
        Plus,
        Minus,
        Asterisk,
        Division,
        Mod,
        LeftParens,
        RightParens,
        LeftBracket,
        RightBracket,
        OpenBrace,
        CloseBrace,
        Point,
        Comma,
        SemiColon,
        Colon,
        Equal,
        LessThan,
        LessOrEqualThan,
        NotEqual,
        GreaterThan,
        GreaterOrEqualThan,
        LogicalAnd,
        LogicalOr,
        LogicalNegation,
        //PrintKeyword,
        ClassKeyword,
        IfKeyword,
        ElseKeyword,
        IntKeyword,
        FloatKeyword,
        TrueKeyword,
        FalseKeyword,
        BoolKeyword,
        DateTimeKeyword,
        ForKeyword,
        WhileKeyword,
        ReturnKeyword,
        ConsoleKeyword,
        WriteLineKeyword,
        ReadLineKeyword,
        Identifier,
        Constant,
        FloatConstant,
        StringLiteral,
        EOF
    }
}
