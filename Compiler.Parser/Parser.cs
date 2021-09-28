using Compiler.Core;
using System;
using Compiler.Core.Statements;
using Compiler.Core.Expressions;
using Type = Compiler.Core.Type;
using Compiler.Core.Interfaces;
using System.IO;

namespace Compiler.Parser
{
    public class Parser : IParser
    {
        private readonly IScanner scanner;
        private Token lookAhead;
        public Parser( IScanner scanner )
        {
            this.scanner = scanner;
            this.Move();
        }

        public Statement Parse()
        {
            return Program();
        }

        private Statement Program()
        {
            EnvironmentManager.PushContext();
            Match(TokenType.ClassKeyword);
            Match(TokenType.Identifier);
            var block = Block();
            block.ValidateSemantic();
            string code = block.Generate();
            WriteToFile(code);
            return block;
        }
        public void WriteToFile(string code)
        {
            File.WriteAllText("C:\\Users\\carlo\\Github\\Compiler\\Compiler.Console\\GeneratedCode.txt", code);
        }
        private Statement Block()
        {
            Statement statements = null;
            Match(TokenType.OpenBrace);
            EnvironmentManager.PushContext();
            while (this.lookAhead.TokenType != TokenType.CloseBrace)
            {
                statements = Declarations();
            }
            Match(TokenType.CloseBrace);
            EnvironmentManager.PopContext();
            return statements;
        }

        private Statement Declarations()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.IntKeyword:
                    Match(VarType(this.lookAhead));
                    Token token = this.lookAhead;
                    Match(TokenType.Identifier);
                    var id = new Id(token, Type.Int);
                    return TypedDeclarations(token, id);
                case TokenType.FloatKeyword:
                    Match(VarType(this.lookAhead));
                    token = this.lookAhead;
                    Match(TokenType.Identifier);
                    id = new Id(token, Type.Float);
                    return TypedDeclarations(token, id);
                case TokenType.BoolKeyword:
                    Match(VarType(this.lookAhead));
                    token = this.lookAhead;
                    Match(TokenType.Identifier);
                    id = new Id(token, Type.Bool);
                    return TypedDeclarations(token, id);
                case TokenType.DateTimeKeyword:
                    Match(VarType(this.lookAhead));
                    token = this.lookAhead;
                    Match(TokenType.Identifier);
                    id = new Id(token, Type.DateTime);
                    return TypedDeclarations(token, id);
                default:
                    break;
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
                return null;
        }

        private Statement TypedDeclarations(Token token, Id id)
        {
            if(this.lookAhead.TokenType == TokenType.LeftParens)
            {
                return MethodStmt(token,id);
            }
            return VarDeclaration(token, id);
        }

        private Statement VarDeclaration(Token token, Id id)
        {
            return new SequenceStatement(DeclarationOptions(token, id), Declarations());
        }

        private Statement MethodStmt(Token token, Id id)
        {
            Match(TokenType.LeftParens);
            var @params = OptParams();
            Match(TokenType.RightParens);
            EnvironmentManager.AddMethod(token.Lexeme, id, new ArgumentExpression(token, @params as TypedExpression));
            var statement1 = Block();
            return new MethodStatement(id, @params, statement1);
        }
        
        private Expression OptParams()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.IntKeyword:
                    Match(VarType(this.lookAhead));
                    var token = this.lookAhead;
                    Match(TokenType.Identifier);
                    var id = new Id(token, Type.Int);
                    EnvironmentManager.AddVariable(token.Lexeme, id);
                    if (this.lookAhead.TokenType != TokenType.Comma)
                    {
                        return id;
                    }

                    Match(TokenType.Comma);
                    return new ArgumentExpression(this.lookAhead, id, OptParams() as TypedExpression) ;

                case TokenType.FloatKeyword:
                    Match(VarType(this.lookAhead));
                    token = this.lookAhead;
                    Match(TokenType.Identifier);
                    id = new Id(token, Type.Float);
                    if (this.lookAhead.TokenType != TokenType.Comma)
                    {
                        EnvironmentManager.AddVariable(token.Lexeme, id);
                        return id;
                    }

                    Match(TokenType.Comma);
                    return new ArgumentExpression(this.lookAhead, id, OptParams() as TypedExpression);

                case TokenType.BoolKeyword:
                    Match(VarType(this.lookAhead));
                    token = this.lookAhead;
                    Match(TokenType.Identifier);
                    id = new Id(token, Type.Bool);
                    if (this.lookAhead.TokenType != TokenType.Comma)
                    {
                        EnvironmentManager.AddVariable(token.Lexeme, id);
                        return id;
                    }

                    Match(TokenType.Comma);
                    return new ArgumentExpression(this.lookAhead, id, OptParams() as TypedExpression);

                case TokenType.DateTimeKeyword:
                    Match(VarType(this.lookAhead));
                    token = this.lookAhead;
                    Match(TokenType.Identifier);
                    id = new Id(token, Type.DateTime);
                    if (this.lookAhead.TokenType != TokenType.Comma)
                    {
                        EnvironmentManager.AddVariable(token.Lexeme, id);
                        return id;
                    }

                    Match(TokenType.Comma);
                    return new ArgumentExpression(this.lookAhead, id, OptParams() as TypedExpression);
                default:
                    break;
            }
            return null;
        }

        private Statement DeclarationOptions(Token token, Id id)
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
                    EnvironmentManager.AddVariable(token.Lexeme, id);
                    return new DeclarationStatement(id, value1 as TypedExpression, "array");

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
                        EnvironmentManager.AddVariable(token.Lexeme, id);
                        return new DeclarationStatement(id, value1 as TypedExpression, value2 as TypedExpression, value3 as TypedExpression, "date");
                    }
                    value1 = Expression();
                    Match(TokenType.SemiColon);
                    EnvironmentManager.AddVariable(token.Lexeme, id);
                    return new DeclarationStatement(id, value1 as TypedExpression, "normal");
                default:
                    Match(TokenType.SemiColon);
                    EnvironmentManager.AddVariable(token.Lexeme, id);
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
                    throw new ApplicationException($"Invalid type {token.Lexeme.ToString()}");
            }
        }

        private Statement Stmts()
        {
            //if (this.lookAhead.TokenType == TokenType.CloseBrace)
            //{
            //    return null;
            //}
            return new SequenceStatement(Stmt(), Declarations());
        }

        private Statement Stmt()
        {
            Expression expression;
            Statement statement1, statement2;
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Identifier:
                    {
                        var symbol = EnvironmentManager.GetSymbol(this.lookAhead.Lexeme);
                        Match(TokenType.Identifier);
                        if (this.lookAhead.TokenType == TokenType.Equal || this.lookAhead.TokenType == TokenType.LeftBracket)
                        {
                            return AssignStmt(symbol.Id);
                        }
                        return CallStmt(symbol);
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
                            return new IfStatement(expression as TypedExpression, statement1);
                        }

                        Match(TokenType.ElseKeyword);
                        statement2 = Stmt();
                        return new ElseStatement(expression as TypedExpression, statement1, statement2);
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
                    return new ReturnStatement(expression as TypedExpression);
                default:
                    return Block();
            }
        }


        private Statement AssignStmt( Id id)
        {
            Expression value1, value2, value3;
            switch (this.lookAhead.TokenType)
            {
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
                        return new AssignationStatement(id, value1 as TypedExpression, value2 as TypedExpression, value3 as TypedExpression, "date");
                    }
                    value1 = Expression();
                    Match(TokenType.SemiColon);
                    return new AssignationStatement(id, value1 as TypedExpression, "normal");
                default:
                    Match(TokenType.LeftBracket);
                    value1 = Expression();
                    Match(TokenType.RightBracket);
                    Match(TokenType.Equal);
                    value2 = Expression();
                    Match(TokenType.SemiColon);
                    return new AssignationStatement(id, value1 as TypedExpression, value2 as TypedExpression, "array");
            }
            
           

        }

        private Statement CallStmt(Symbol symbol)
        {
           
            Match(TokenType.LeftParens);
            var arguments = OptArguments();
            Match(TokenType.RightParens);
            Match(TokenType.SemiColon);
            return new CallStatement(symbol.Id, arguments, symbol.Attributes);
        }

        public Expression OptArguments()
        {
            if (this.lookAhead.TokenType != TokenType.RightParens)
            {
                return Arguments();
            }
            return null; 
        }
        private Expression Arguments()
        {
            var expression = Expression();
            if( this.lookAhead.TokenType != TokenType.Comma)
            {
                return expression;
            }
            Match(TokenType.Comma);
            expression = new ArgumentExpression(this.lookAhead, expression as TypedExpression, Arguments() as TypedExpression);
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
            var varType = this.lookAhead.TokenType;
            Match(VarType(this.lookAhead));
            var token = this.lookAhead;
            Match(TokenType.Identifier);
            Match(TokenType.InKeyword);
            var symbol = EnvironmentManager.GetSymbol(this.lookAhead.Lexeme);
            Match(TokenType.Identifier);
            Match(TokenType.RightParens);
            var elementId = new Id(token, Type.Int);
            switch (varType)
            {
                case TokenType.IntKeyword:
                    elementId = new Id(token, Type.Int);
                    break;
                case TokenType.FloatKeyword:
                    elementId = new Id(token, Type.Float);
                    break;
                case TokenType.BoolKeyword:
                    elementId = new Id(token, Type.Bool);
                    break;
                case TokenType.DateTimeKeyword:
                    elementId = new Id(token, Type.DateTime);
                    break;
                default:
                    break;
            }
            EnvironmentManager.AddVariable(token.Lexeme, elementId);
            var arrayId = symbol.Id;
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
            return new WhileStatement(expression as TypedExpression, statement);
        }

        private Statement ConsoleStmt()
        {
            Match(TokenType.ConsoleKeyword);
            Match(TokenType.Point);
            if (this.lookAhead.TokenType == TokenType.WriteLineKeyword)
            {
                Match(TokenType.WriteLineKeyword);
                Match(TokenType.LeftParens);
                Match(TokenType.Dollar);
                var expression = Expression();
                Match(TokenType.RightParens);
                Match(TokenType.SemiColon);
                return new ConsoleStatement(expression, true);
            }
            Match(TokenType.ReadLineKeyword);
            Match(TokenType.LeftParens);
            Match(TokenType.RightParens);
            Match(TokenType.SemiColon);
            return new ConsoleStatement(false);
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
                expression = new LogicalExpression(token, expression as TypedExpression, And() as TypedExpression);
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
                expression = new LogicalExpression(token, expression as TypedExpression, Equality() as TypedExpression);
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
                expression = new RelationalExpression(token, expression as TypedExpression, Relational() as TypedExpression);
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
                expression = new RelationalExpression(token, expression as TypedExpression, Term() as TypedExpression);
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
                expression = new ArithmeticOperator(token, expression as TypedExpression, Factor() as TypedExpression);
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
                expression = new ArithmeticOperator(token, expression as TypedExpression, Unary() as TypedExpression);
            }

            return expression;
        }

        private Expression Unary()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.LogicalNegation:
                    var token = this.lookAhead;
                    Match(TokenType.LogicalNegation);
                    return new UnaryOperator(token, Unary() as TypedExpression);
                case TokenType.Increase:
                    token = this.lookAhead;
                    Match(TokenType.Increase);
                    return new UnaryOperator(token, Unary() as TypedExpression);
                case TokenType.Decrease:
                    token = this.lookAhead;
                    Match(TokenType.Decrease);
                    return new UnaryOperator(token, Unary() as TypedExpression);
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
                case TokenType.LeftBracket:
                    Match(TokenType.LeftBracket);
                    expression = Expression();
                    Match(TokenType.RightBracket);
                    return expression;
                case TokenType.IntConstant:
                    var constant = new Constant(lookAhead, Type.Int);
                    Match(TokenType.IntConstant);
                    return constant;
                case TokenType.FloatConstant:
                    constant = new Constant(lookAhead, Type.Float);
                    Match(TokenType.FloatConstant);
                    return constant;
                case TokenType.TrueConstant:
                    constant = new Constant(lookAhead, Type.Bool);
                    Match(TokenType.TrueConstant);
                    return constant;
                case TokenType.FalseConstant:
                    constant = new Constant(lookAhead, Type.Bool);
                    Match(TokenType.FalseConstant);
                    return constant;
                case TokenType.StringLiteral:
                    constant = new Constant(lookAhead, Type.String);
                    Match(TokenType.StringLiteral);
                    return constant;
                default:
                    var symbol = EnvironmentManager.GetSymbol(this.lookAhead.Lexeme);
                    Match(TokenType.Identifier);
                    if( this.lookAhead.TokenType == TokenType.LeftBracket)
                    {
                        return Expression();
                    }
                    return symbol.Id;
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
