using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using Compiler.Core;
using System;
using Compiler.Core.Statements;
using Compiler.Core.Expressions;

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

        public Node Parse()
        {
            return Program();
        }

        private Statement Program()
        {
            Match(TokenType.ClassKeyword);
            Match(TokenType.Identifier);
            return Block();
        }

        private Statement Block()
        {
            Statement statements = Statement.Null;
            Match(TokenType.OpenBrace);
            while (this.lookAhead.TokenType != TokenType.CloseBrace)
            {
                statements = Declarations();
            }
            Match(TokenType.CloseBrace);
            return statements;
        }

        private Statement Declarations()
        {
            if (this.lookAhead.TokenType == TokenType.IntKeyword
                || this.lookAhead.TokenType == TokenType.FloatKeyword
                || this.lookAhead.TokenType == TokenType.DateTimeKeyword
                || this.lookAhead.TokenType == TokenType.BoolKeyword)
            {
                Match(VarType(this.lookAhead));
                var id = new Id(this.lookAhead, Core.Type.Int);
                Match(TokenType.Identifier);
                return TypedDeclarations(id);
            }
            if(this.lookAhead.TokenType == TokenType.Identifier
                || this.lookAhead.TokenType == TokenType.IfKeyword
                || this.lookAhead.TokenType == TokenType.ForKeyword
                || this.lookAhead.TokenType == TokenType.ForeachKeyword
                || this.lookAhead.TokenType == TokenType.WhileKeyword
                || this.lookAhead.TokenType == TokenType.ReturnKeyword
                || this.lookAhead.TokenType == TokenType.ConsoleKeyword
                || this.lookAhead.TokenType == TokenType.OpenBrace)
            {
                return Stmts();
            }
                return Statement.Null;
        }

        private Statement TypedDeclarations(Id id)
        {
            if(this.lookAhead.TokenType == TokenType.LeftParens)
            {
                return MethodStmt(id);
            }
            return VarDeclaration(id);
        }

        private Statement VarDeclaration(Id id)
        {
            return new SequenceStatement(DeclarationOptions(id), Declarations());
        }

        private Statement MethodStmt(Id id)
        {
            Match(TokenType.LeftParens);
            var @params = OptParams();
            Match(TokenType.RightParens);
            var statement1 = Block();
            return new MethodStatement(id, @params, statement1);
        }

        //private Statement Declaration(Id id)
        //{
        //    switch (this.lookAhead.TokenType)
        //    {
        //        case TokenType.IntKeyword:
        //            Match(TokenType.IntKeyword);
        //            id = new Id(this.lookAhead, Core.Type.Int);
        //            Match(TokenType.Identifier);
        //            return DeclarationOptions( id );

        //        case TokenType.FloatKeyword:
        //            Match(TokenType.FloatKeyword);
        //            id = new Id(this.lookAhead, Core.Type.Float);
        //            Match(TokenType.Identifier);
        //            return DeclarationOptions( id );

        //        case TokenType.DateTimeKeyword:
        //            Match(TokenType.DateTimeKeyword);
        //            id = new Id(this.lookAhead, Core.Type.DateTime);
        //            Match(TokenType.Identifier);
        //            return DeclarationOptions( id );

        //        case TokenType.BoolKeyword:
        //            Match(TokenType.BoolKeyword);
        //            id = new Id(this.lookAhead, Core.Type.Bool);
        //            Match(TokenType.Identifier);
        //            return DeclarationOptions( id );
                    
        //        default:
        //            return Statement.Null;
        //    }
        //}

        private Statement DeclarationOptions(Id id)
        {
            Expression value1, value2, value3;

            switch (this.lookAhead.TokenType)
            {
                case TokenType.LeftBracket:
                    Match(TokenType.LeftBracket);
                    Match(TokenType.RightBracket);
                    Match(TokenType.Equal);
                    Match(TokenType.NewKeyword);
                    Match(VarType(this.lookAhead));
                    Match(TokenType.LeftBracket);
                    value1 = Expression();
                    Match(TokenType.RightBracket);
                    Match(TokenType.SemiColon);
                    return new DeclarationStatement(id, value1);

                case TokenType.Equal:
                    Match(TokenType.Equal);
                    if (this.lookAhead.TokenType == TokenType.NewKeyword)
                    {
                        Match(TokenType.NewKeyword);
                        Match(TokenType.DateTimeKeyword);
                        Match(TokenType.LeftParens);
                        value1 = Expression();
                        Match(TokenType.Comma);
                        value2 = Expression();
                        Match(TokenType.Comma);
                        value3 = Expression();
                        Match(TokenType.RightParens);
                        Match(TokenType.SemiColon);
                        return new DeclarationStatement(id, value1, value2, value3);
                    }
                    value1 = Expression();
                    Match(TokenType.SemiColon);
                    return new DeclarationStatement(id, value1);
                default:
                    Match(TokenType.SemiColon);
                    return new DeclarationStatement(id);
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

        private Statement Stmts()
        {
            //if (this.lookAhead.TokenType == TokenType.CloseBrace)
            //{
            //    return Statement.Null;
            //}
            return new SequenceStatement(Stmt(), Declarations());
        }

        private Statement Stmt()
        {
            Expression expression;
            Statement statement1, statement2;
            Id id;
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Identifier:
                    {
                        id = new Id(this.lookAhead, Core.Type.Int);
                        Match(TokenType.Identifier);
                        if (this.lookAhead.TokenType == TokenType.Equal || this.lookAhead.TokenType == TokenType.LeftBracket)
                        {
                            return AssignStmt(id);
                        }
                        return CallStmt(id);
                    }

                case TokenType.IfKeyword:
                    {
                        Match(TokenType.IfKeyword);
                        Match(TokenType.LeftParens);
                        expression = Expression();
                        Match(TokenType.RightParens);
                        statement1 = Stmt();

                        if (this.lookAhead.TokenType != TokenType.ElseKeyword)
                        {
                            return new IfStatement(expression, statement1);
                        }

                        Match(TokenType.ElseKeyword);
                        statement2 = Stmt();
                        return new ElseStatement(expression, statement1, statement2);
                    }

                //case TokenType.ForKeyword:
                //    return ForStmt();

                case TokenType.ForeachKeyword:
                    return ForeachStmt();

                case TokenType.WhileKeyword:
                    return WhileStmt();

                case TokenType.ConsoleKeyword:
                    return ConsoleStmt();

                case TokenType.ReturnKeyword:
                    Match(TokenType.ReturnKeyword);
                    expression = Expression();
                    Match(TokenType.SemiColon);
                    return new ReturnStatement(expression);
                default:
                    return Block();
            }
        }

        private Expression OptParams()
        {
            Expression expression;
            switch (this.lookAhead.TokenType)
            {
                case TokenType.IntKeyword:
                case TokenType.FloatKeyword:
                case TokenType.DateTimeKeyword:
                case TokenType.BoolKeyword:
                    Match( VarType(this.lookAhead) );
                    expression = Expression();
                    if( this.lookAhead.TokenType != TokenType.Comma)
                    {
                       return expression;
                    }

                    Match(TokenType.Comma);
                    expression = new ArgumentExpression(this.lookAhead, expression, OptParams());
                    return expression;
                default:
                    break;
            }
            return null;
        }

        private Statement AssignStmt( Id id)
        {
            Expression value1, value2;
            if (this.lookAhead.TokenType == TokenType.Equal) {
                Match(TokenType.Equal);
                value1 = Expression();
                Match(TokenType.SemiColon);
                return new AssignationStatement(id, value1);
            }
            Match(TokenType.LeftBracket);
            value1 = Expression();
            Match(TokenType.RightBracket);
            Match(TokenType.Equal);
            value2 = Expression();
            Match(TokenType.SemiColon);
            return new AssignationStatement(id, value1, value2);
        }

        private Statement CallStmt(Id id)
        {
           
            Match(TokenType.LeftParens);
            var arguments = OptArguments();
            Match(TokenType.RightParens);
            Match(TokenType.SemiColon);
            return new CallStatement(id, arguments);
        }

        private Expression OptArguments()
        {
            var expression = Expression();
            if( this.lookAhead.TokenType != TokenType.Comma)
            {
                return expression;
            }
            Match(TokenType.Comma);
            expression = new ArgumentExpression(this.lookAhead, expression, OptArguments());
            return expression;
        }
        private Statement ForStmt()
        {
            Match(TokenType.ForKeyword);
            Match(TokenType.LeftParens);
            var declaration = Declarations();
            var expression1 = Expression();
            Match(TokenType.SemiColon);
            var expression2 = Expression();
            Match(TokenType.RightParens);
            var statement = Block();
            return new ForStatement(declaration, expression1, expression2, statement);

        }

        private Statement ForeachStmt()
        {
            Match(TokenType.ForeachKeyword);
            Match(TokenType.LeftParens);
            Match(VarType(this.lookAhead));
            var elementId = new Id(this.lookAhead, Core.Type.Int);
            Match(TokenType.Identifier);
            Match(TokenType.InKeyword);
            var arrayId = new Id(this.lookAhead, Core.Type.Int);
            Match(TokenType.Identifier);
            Match(TokenType.RightParens);
            var statement = Block();
            return new ForeachStatement(elementId, arrayId, statement);
        }

        private Statement WhileStmt()
        {
            Match(TokenType.WhileKeyword);
            Match(TokenType.LeftParens);
            var expression = Expression();
            Match(TokenType.RightParens);
            var statement = Block();
            return new WhileStatement(expression, statement);
        }

        private Statement ConsoleStmt()
        {
            Match(TokenType.ConsoleKeyword);
            Match(TokenType.Point);
            if (this.lookAhead.TokenType == TokenType.WriteLineKeyword)
            {
                Match(TokenType.WriteLineKeyword);
                Match(TokenType.LeftParens);
                var expression = Expression();
                Match(TokenType.RightParens);
                Match(TokenType.SemiColon);
                return new ConsoleStatement(expression);
            }
            Match(TokenType.ReadLineKeyword);
            Match(TokenType.LeftParens);
            Match(TokenType.RightParens);
            Match(TokenType.SemiColon);
            return new ConsoleStatement();
        }
        private Expression Expression()
        {
            return Or();
        }

        private Expression Or()
        {
            var expression = And();
            while ( this.lookAhead.TokenType == TokenType.LogicalOr)
            {
                var token = this.lookAhead;
                Match(TokenType.LogicalOr);
                expression = new LogicalExpression(token, expression, And());
            }
            return expression;
        }

        private Expression And()
        {
            var expression = Equality();
            while (this.lookAhead.TokenType == TokenType.LogicalAnd)
            {
                var token = this.lookAhead;
                Match(TokenType.LogicalAnd);
                expression = new LogicalExpression(token, expression, Equality());
            }
            return expression;
        }

        private Expression Equality()
        {
            var expression = Relational();
            while (this.lookAhead.TokenType == TokenType.NotEqual || this.lookAhead.TokenType == TokenType.RelationalEqual)
            {
                var token = this.lookAhead;
                if(this.lookAhead.TokenType == TokenType.NotEqual)
                {
                    Match(TokenType.NotEqual);
                }
                else
                {
                    Match(TokenType.RelationalEqual);
                }
                expression = new RelationalExpression(token, expression, Relational());
            }

            return expression;
        }

        private Expression Relational()
        {
            var expression = Term();
            while (this.lookAhead.TokenType == TokenType.GreaterThan 
                || this.lookAhead.TokenType == TokenType.GreaterOrEqualThan
                || this.lookAhead.TokenType == TokenType.LessThan
                || this.lookAhead.TokenType == TokenType.LessOrEqualThan)
            {
                var token = this.lookAhead;
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
                expression = new RelationalExpression(token, expression, Term());
            }
            return expression;
        }

        private Expression Term()
        {
            var expression = Factor();
            while (this.lookAhead.TokenType == TokenType.Minus || this.lookAhead.TokenType == TokenType.Plus)
            {
                var token = this.lookAhead;
                if (this.lookAhead.TokenType == TokenType.Minus)
                {
                    Match(TokenType.Minus);
                }
                else
                {
                    Match(TokenType.Plus);
                }
                expression = new ArithmeticOperator(token, expression, Factor());
            }
            return expression;
        }

        private Expression Factor()
        {
            var expression = Unary();
            while (this.lookAhead.TokenType == TokenType.Division 
                || this.lookAhead.TokenType == TokenType.Asterisk
                || this.lookAhead.TokenType == TokenType.Mod)
            {
                var token = this.lookAhead;
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
                expression = new ArithmeticOperator(token, expression, Unary());
            }

            return expression;
        }

        private Expression Unary()
        {
            var unaryOperator="";
            switch (this.lookAhead.TokenType)
            {
                case TokenType.LogicalNegation:
                    unaryOperator = "!";
                    Match(TokenType.LogicalNegation);
                    return Unary();
                case TokenType.Increase:
                    unaryOperator = "++";
                    Match(TokenType.Increase);
                    return Unary();
                case TokenType.Decrease:
                    unaryOperator = "--";
                    Match(TokenType.Decrease);
                    return Unary();
                default:
                    return Primary();
                    //CallStmt();
            }
        }

        private Expression Primary()
        {
           switch (this.lookAhead.TokenType)
            {
                case TokenType.LeftParens:
                    Match(TokenType.LeftParens);
                    var expression = Expression();
                    Match(TokenType.RightParens);
                    return expression;
                case TokenType.Constant:
                    var constant = new Constant(lookAhead, Core.Type.Int);
                    Match(TokenType.Constant);
                    return constant;
                case TokenType.TrueKeyword:
                    constant = new Constant(lookAhead, Core.Type.Bool);
                    Match(TokenType.TrueKeyword);
                    return constant;
                case TokenType.FalseKeyword:
                    constant = new Constant(lookAhead, Core.Type.Bool);
                    Match(TokenType.FalseKeyword);
                    return constant;
                case TokenType.StringLiteral:
                    constant = new Constant(lookAhead, Core.Type.String);
                    Match(TokenType.StringLiteral);
                    return constant;
                default:
                    var id = new Id(lookAhead, Core.Type.Int);
                    Match(TokenType.Identifier);
                    return id;
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
