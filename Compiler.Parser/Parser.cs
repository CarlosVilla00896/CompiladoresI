using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using System;

namespace Compiler.Parser
{
    public class Parser
    {
        private readonly IScanner scanner;
        private Token lookAhead;
        public Parser( IScanner scanner )
        {
            this.scanner = scanner;
            this.Move();
        }

        public void Parse()
        {
            Program();
        }

        private void Program()
        {
            Match(TokenType.ClassKeyword);
            Match(TokenType.Identifier);
            Block();
        }

        private void Block()
        {
            Match(TokenType.OpenBrace);
            Declarations();
            Statements();
            Match(TokenType.CloseBrace);
        }

        private void Declarations()
        {
            if (this.lookAhead.TokenType == TokenType.IntKeyword
                || this.lookAhead.TokenType == TokenType.FloatKeyword
                || this.lookAhead.TokenType == TokenType.DateTimeKeyword
                || this.lookAhead.TokenType == TokenType.BoolKeyword)
            {
                Declaration();
                Declarations();
            }
            //epsilon
        }

        private void Declaration()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.IntKeyword:
                    Match(TokenType.IntKeyword);
                    Match(TokenType.Identifier);
                    DeclarationOptions();
                    Match(TokenType.SemiColon);
                    break;
                case TokenType.FloatKeyword:
                    Match(TokenType.FloatKeyword);
                    Match(TokenType.Identifier);
                    DeclarationOptions();
                    Match(TokenType.SemiColon);
                    break;
                case TokenType.DateTimeKeyword:
                    Match(TokenType.DateTimeKeyword);
                    Match(TokenType.Identifier);
                    DeclarationOptions();
                    Match(TokenType.SemiColon);
                    break;
                case TokenType.BoolKeyword:
                    Match(TokenType.BoolKeyword);
                    Match(TokenType.Identifier);
                    DeclarationOptions();
                    Match(TokenType.SemiColon);
                    break;
                default:
                    //epsilon
                    break;
            }
        }

        private void DeclarationOptions()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.LeftBracket:
                    Match(TokenType.LeftBracket);
                    Match(TokenType.RightBracket);
                    Match(TokenType.Equal);
                    Match(TokenType.NewKeyword);
                    Match(VarType(this.lookAhead));
                    Match(TokenType.LeftBracket);
                    Expression();
                    Match(TokenType.RightBracket);
                    break;

                case TokenType.Equal:
                    Match(TokenType.Equal);
                    if (this.lookAhead.TokenType == TokenType.NewKeyword)
                    {
                        Match(TokenType.NewKeyword);
                        Match(TokenType.DateTimeKeyword);
                        Match(TokenType.LeftParens);
                        Expression();
                        Match(TokenType.Comma);
                        Expression();
                        Match(TokenType.Comma);
                        Expression();
                        Match(TokenType.RightParens);
                        return;
                    }
                    Expression();
                    break;
                default:
                    break;
            }

        }

        private TokenType VarType(Token token)
        {
            switch (token.TokenType)
            {
                case TokenType.IntKeyword:
                    return TokenType.IntKeyword;
                case TokenType.FloatKeyword:
                    return TokenType.FloatKeyword;
                case TokenType.DateTimeKeyword:
                    return TokenType.DateTimeKeyword;
                case TokenType.BoolKeyword:
                    return TokenType.BoolKeyword;
                default:
                    throw new ApplicationException($"Invalid type {token.ToString()}");
            }
        }

        private void Statements()
        {
            if(this.lookAhead.TokenType == TokenType.CloseBrace)
            {
                return;
            }
            Statement();
            Statements();
        }

        private void Statement()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.IntKeyword:
                case TokenType.FloatKeyword:
                case TokenType.DateTimeKeyword:
                case TokenType.BoolKeyword:
                    Match( VarType( this.lookAhead) );
                    Match(TokenType.Identifier);
                    Match(TokenType.LeftParens);
                    OptParams();
                    Match(TokenType.RightParens);
                    Block();
                    break;

                case TokenType.Identifier:
                    {
                        Match(TokenType.Identifier);
                        if (this.lookAhead.TokenType == TokenType.Equal || this.lookAhead.TokenType == TokenType.LeftBracket)
                        {
                            AssignStatement();
                            Match(TokenType.SemiColon);
                            return;
                        }
                        CallStatement();
                        Match(TokenType.SemiColon);
                    }
                    break;

                case TokenType.IfKeyword:
                    {
                        Match(TokenType.IfKeyword);
                        Match(TokenType.LeftParens);
                        Expression();
                        Match(TokenType.RightParens);
                        Statement();

                        if (this.lookAhead.TokenType != TokenType.ElseKeyword)
                        {
                            return;
                        }

                        Match(TokenType.ElseKeyword);
                        Statement();
                    }
                    break;

                case TokenType.ForKeyword:
                    ForStatement();
                    break;

                case TokenType.ForeachKeyword:
                    ForeachStatement();
                    break;

                case TokenType.WhileKeyword:
                    WhileStatement();
                    break;

                case TokenType.ConsoleKeyword:
                    ConsoleStatement();
                    break;
                case TokenType.ReturnKeyword:
                    Match(TokenType.ReturnKeyword);
                    Expression();
                    if ( this.lookAhead.TokenType == TokenType.SemiColon )
                    {
                        Match(TokenType.SemiColon);
                        return;
                    }
                    //Consume(TokenType, $"Expect '{this.lookAhead.ToString()}' after return return statement")
                    break;
                default:
                    Block();
                    break;
            }
        }

        private void OptParams()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.IntKeyword:
                case TokenType.FloatKeyword:
                case TokenType.DateTimeKeyword:
                case TokenType.BoolKeyword:
                    Match( VarType(this.lookAhead) );
                    Match(TokenType.Identifier);
                    if( this.lookAhead.TokenType != TokenType.Comma)
                    {
                        return;
                    }
                    Match(TokenType.Comma);
                    OptParams();
                    break;
                default:
                    break;
            }
        }

        private void AssignStatement()
        {
            if (this.lookAhead.TokenType == TokenType.Equal) {
                Match(TokenType.Equal);
                Expression();
                return;
            }
            Match(TokenType.LeftBracket);
            Expression();
            Match(TokenType.RightBracket);
            Match(TokenType.Equal);
            Expression();
        }

        private void CallStatement()
        {
            Match(TokenType.LeftParens);
            OptArguments();
            Match(TokenType.RightParens);
        }

        private void OptArguments()
        {
            Expression();
            if( this.lookAhead.TokenType != TokenType.Comma)
            {
                return;
            }
            Match(TokenType.Comma);
            OptArguments();
        }
        private void ForStatement()
        {
            Match(TokenType.ForKeyword);
            Match(TokenType.LeftParens);
            Declarations();
            Expression();
            Match(TokenType.SemiColon);
            Expression();
            Match(TokenType.RightParens);
            Block();
        }

        private void ForeachStatement()
        {
            Match(TokenType.ForeachKeyword);
            Match(TokenType.LeftParens);
            Match(VarType(this.lookAhead));
            Expression();
            Match(TokenType.InKeyword);
            Expression();
            Match(TokenType.RightParens);
            Block();
        }

        private void WhileStatement()
        {
            Match(TokenType.WhileKeyword);
            Match(TokenType.LeftParens);
            Expression();
            Match(TokenType.RightParens);
            Block();
        }

        private void ConsoleStatement()
        {
            Match(TokenType.ConsoleKeyword);
            Match(TokenType.Point);
            if (this.lookAhead.TokenType == TokenType.WriteLineKeyword)
            {
                Match(TokenType.WriteLineKeyword);
                Match(TokenType.LeftParens);
                Expression();
                Match(TokenType.RightParens);
                Match(TokenType.SemiColon);
                return;
            }
            Match(TokenType.ReadLineKeyword);
            Match(TokenType.LeftParens);
            Match(TokenType.RightParens);
            Match(TokenType.SemiColon);
        }
        private void Expression()
        {
            Or();
        }

        private void Or()
        {
            And();
            while( this.lookAhead.TokenType == TokenType.LogicalOr)
            {
                Match(TokenType.LogicalOr);
                And();
            }
        }

        private void And()
        {
            Equality();
            while (this.lookAhead.TokenType == TokenType.LogicalAnd)
            {
                Match(TokenType.LogicalAnd);
                Equality();
            }
        }

        private void Equality()
        {
            Relational();
            while (this.lookAhead.TokenType == TokenType.NotEqual || this.lookAhead.TokenType == TokenType.RelationalEqual)
            {
                if(this.lookAhead.TokenType == TokenType.NotEqual)
                {
                    Match(TokenType.NotEqual);
                }
                else
                {
                    Match(TokenType.RelationalEqual);
                }
                Relational();
            }
        }

        private void Relational()
        {
            Term();
            while (this.lookAhead.TokenType == TokenType.GreaterThan 
                || this.lookAhead.TokenType == TokenType.GreaterOrEqualThan
                || this.lookAhead.TokenType == TokenType.LessThan
                || this.lookAhead.TokenType == TokenType.LessOrEqualThan)
            {
                switch (this.lookAhead.TokenType)
                {
                    case TokenType.GreaterThan:
                        Match(TokenType.GreaterThan);
                        break;
                    case TokenType.GreaterOrEqualThan:
                        Match(TokenType.GreaterOrEqualThan);
                        break;
                    case TokenType.LessThan:
                        Match(TokenType.LessThan);
                        break;
                    case TokenType.LessOrEqualThan:
                        Match(TokenType.LessOrEqualThan);
                        break;
                    default:
                        break;
                }
                Term();
            }
        }

        private void Term()
        {
            Factor();
            while (this.lookAhead.TokenType == TokenType.Minus || this.lookAhead.TokenType == TokenType.Plus)
            {
                if (this.lookAhead.TokenType == TokenType.Minus)
                {
                    Match(TokenType.Minus);
                }
                else
                {
                    Match(TokenType.Plus);
                }
                Factor();
            }
        }

        private void Factor()
        {
            Unary();
            while (this.lookAhead.TokenType == TokenType.Division 
                || this.lookAhead.TokenType == TokenType.Asterisk
                || this.lookAhead.TokenType == TokenType.Mod)
            {
                switch (this.lookAhead.TokenType)
                {
                    case TokenType.Division:
                        Match(TokenType.Division);
                        break;
                    case TokenType.Asterisk:
                        Match(TokenType.Asterisk);
                        break;
                    case TokenType.Mod:
                        Match(TokenType.Mod);
                        break;
                    default:
                        break;
                }
                Unary();
            }
        }

        private void Unary()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.LogicalNegation:
                    Match(TokenType.LogicalNegation);
                    Unary();
                    break;
                case TokenType.Increase:
                    Match(TokenType.Increase);
                    Unary();
                    break;
                case TokenType.Decrease:
                    Match(TokenType.Decrease);
                    Unary();
                    break;
                default:
                    Primary();
                    break;
            }
        }

        private void Primary()
        {
           switch (this.lookAhead.TokenType)
            {
                case TokenType.Constant:
                    Match(TokenType.Constant);
                    break;
                case TokenType.Identifier:
                    Match(TokenType.Identifier);
                    break;
                case TokenType.LeftParens:
                    Match(TokenType.LeftParens);
                    Expression();
                    Match(TokenType.RightParens);
                    break;
                case TokenType.TrueKeyword:
                    Match(TokenType.TrueKeyword);
                    break;
                case TokenType.FalseKeyword:
                    Match(TokenType.FalseKeyword);
                    break;
                case TokenType.StringLiteral:
                    Match(TokenType.StringLiteral);
                    break;
                default:
                    break;
            }
        }

        private void Move()
        {
            this.lookAhead = scanner.GetNextToken();
        }

        private void Match(TokenType tokenType)
        {
            if( this.lookAhead.TokenType != tokenType)
            {
                throw new ApplicationException($"Syntax error: Expected token {tokenType} but found {this.lookAhead.TokenType}. Line: {this.lookAhead.Line} Column: {this.lookAhead.Column} "); 
            }

            this.Move();
        }
    }
}

/*
 * private boolean match(TokenType... types) {
    for (TokenType type : types) {
      if (check(type)) {
        advance();
        return true;
      }
    }

    return false;
  }
 */