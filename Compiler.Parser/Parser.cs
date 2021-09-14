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

        private void Program()
        {
            Block();
        }

        private void Block()
        {
            Match(TokenType.OpenBrace);
            Declarations();
            Statements();
            Match(TokenType.CloseBrace);
        }

        private void Statements()
        {
           //todo
        }

        private void Declarations()
        {
                Declaration();
                Declarations();
        }

        private void Declaration()
        {
            if (this.lookAhead.TokenType == TokenType.IntKeyword
               || this.lookAhead.TokenType == TokenType.FloatKeyword
               || this.lookAhead.TokenType == TokenType.DateTimeKeyword
               || this.lookAhead.TokenType == TokenType.BoolKeyword
               )
            {
                Type();
                DeclarationOptions();
            }
            //epsilon
        }

        private void Type()
        {
            switch( this.lookAhead.TokenType)
            {
                case TokenType.IntKeyword:
                    Match(TokenType.IntKeyword);
                    break;
                case TokenType.FloatKeyword:
                    Match(TokenType.FloatKeyword);
                    break;
                case TokenType.DateTimeKeyword:
                    Match(TokenType.DateTimeKeyword);
                    break;
                case TokenType.BoolKeyword:
                    Match(TokenType.BoolKeyword);
                    break;
                default:
                    break;
            }
        }

        private void DeclarationOptions()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Identifier:
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    break;
                case TokenType.LeftBracket:
                    Match(TokenType.LeftBracket);
                    Match(TokenType.RightBracket);
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
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
