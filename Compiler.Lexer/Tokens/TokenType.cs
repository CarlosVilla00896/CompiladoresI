using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Lexer.Tokens
{
    public enum TokenType
    {
        //one or two 
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
        RelationalEqual,
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
        ForeachKeyword,
        ReturnKeyword,
        TypeKeyWord,
        NewKeyword,
        ConsoleKeyword,
        WriteLineKeyword,
        ReadLineKeyword,
        InKeyword,
        Identifier,
        IntConstant,
        FloatConstant,
        StringLiteral,
        Increase,
        Decrease,
        EOF
    }
}
