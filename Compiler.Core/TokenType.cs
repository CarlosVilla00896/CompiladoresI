using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core
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
        Increase,
        Decrease,
        Dollar,
        ClassKeyword,
        IfKeyword,
        ElseKeyword,
        IntKeyword,
        FloatKeyword,
        TrueConstant,
        FalseConstant,
        BoolKeyword,
        BasicType,
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
        BoolConstant,
        StringLiteral,
        EOF
    }
}
