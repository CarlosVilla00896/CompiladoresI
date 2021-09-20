using Compiler.Lexer.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Lexer
{
    public class Scanner : IScanner
    {
        private Input input;
        private readonly Dictionary<string, TokenType> keywords;
        private readonly List<Symbol> symbolsTable;
        public Scanner(Input input)
        {
            this.input = input;
            this.keywords = new Dictionary<string, TokenType>
            {
                //{ "print", TokenType.PrintKeyword },
                { "if", TokenType.IfKeyword },
                { "else", TokenType.ElseKeyword },
                { "int", TokenType.IntKeyword },
                { "float", TokenType.FloatKeyword },
                { "bool", TokenType.BoolKeyword },
                { "DateTime", TokenType.DateTimeKeyword },
                { "return", TokenType.ReturnKeyword },
                { "while", TokenType.WhileKeyword },
                { "for", TokenType.ForKeyword },
                { "foreach", TokenType.ForeachKeyword },
                { "class", TokenType.ClassKeyword },
                { "true", TokenType.TrueKeyword },
                { "false", TokenType.FalseKeyword },
                { "Console", TokenType.ConsoleKeyword },
                { "WriteLine", TokenType.WriteLineKeyword},
                { "ReadLine", TokenType.ReadLineKeyword},
                { "in", TokenType.ReadLineKeyword},
                { "type", TokenType.TypeKeyWord},
                { "new", TokenType.NewKeyword },
            };

            this.symbolsTable = new List<Symbol>();
        }
        public Token GetNextToken()
        {
            var lexeme = new StringBuilder();
            var currentChar = GetNextChar();
            while (true)
            {
                while (char.IsWhiteSpace(currentChar) || currentChar == '\n')
                {
                    currentChar = GetNextChar();
                }

                if (char.IsLetter(currentChar))
                {
                    lexeme.Append(currentChar);
                    currentChar = PeekNextChar();

                    while (char.IsLetterOrDigit(currentChar))
                    {
                        //currentChar = GetNextChar(); // Descomentar en caso de error
                        GetNextChar(); //Consumo el caracter que acabo de ver
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar(); //Peek al siguiente caracter para ver si sigo en el ciclo
                    }

                    if (keywords.ContainsKey(lexeme.ToString()))
                    {
                        return new Token
                        {
                            TokenType = this.keywords[lexeme.ToString()],
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    }

                    this.symbolsTable.Add(new Symbol(lexeme.ToString(), SymbolType.Identifier));
                    return new Token
                    {
                        TokenType = TokenType.Identifier,
                        Column = input.Position.Column,
                        Line = input.Position.Line,
                        Lexeme = lexeme.ToString(),
                        PositionInSymbolTable = this.symbolsTable.Count - 1
                    };
                }
                else if (char.IsDigit(currentChar))
                {
                    lexeme.Append(currentChar);
                    currentChar = PeekNextChar();
                    while (char.IsDigit(currentChar))
                    {
                        //currentChar = GetNextChar(); //Descomentar en caso de error
                        GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                    }
                    this.symbolsTable.Add(new Symbol(lexeme.ToString(), SymbolType.Constant));
                    return new Token
                    {
                        TokenType = TokenType.Constant,
                        Column = input.Position.Column,
                        Line = input.Position.Line,
                        Lexeme = lexeme.ToString(),
                        PositionInSymbolTable = this.symbolsTable.Count - 1
                    };
                }
                else switch (currentChar)
                {
                    case '/':
                        {
                            var nextChar = PeekNextChar();
                            if(nextChar != '*')
                            {
                                lexeme.Append(currentChar);
                                return new Token
                                {
                                    TokenType = TokenType.Division,
                                    Column = input.Position.Column,
                                    Line = input.Position.Line,
                                    Lexeme = lexeme.ToString()
                                };
                            }

                            while (true)
                            {
                                currentChar = GetNextChar();
                                while( currentChar == '*')
                                {
                                    currentChar = GetNextChar();
                                }
                                if( currentChar == '/')
                                {
                                    currentChar = GetNextChar();
                                    break;
                                }
                            }
                            
                        }
                        break;

                    case '<':
                        {
                            lexeme.Append(currentChar);
                            var nextChar = PeekNextChar();
                            switch (nextChar)
                            {
                                case '=':
                                    GetNextChar();
                                    lexeme.Append(nextChar);
                                    return new Token
                                    {
                                        TokenType = TokenType.LessOrEqualThan,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString()
                                    };

                                //case '>':
                                //    GetNextChar();
                                //    lexeme.Append(nextChar);
                                //    return new Token
                                //    {
                                //        TokenType = TokenType.NotEqual,
                                //        Column = input.Position.Column,
                                //        Line = input.Position.Line,
                                //        Lexeme = lexeme.ToString()
                                //    };
                                default:
                                    return new Token
                                    {
                                        TokenType = TokenType.LessThan,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString()
                                    };

                            }
                        }

                    case '>':
                        {
                            lexeme.Append(currentChar);
                            var nextChar = PeekNextChar();
                            if (nextChar != '=')
                            {
                                return new Token
                                {
                                    TokenType = TokenType.GreaterThan,
                                    Column = input.Position.Column,
                                    Line = input.Position.Line,
                                    Lexeme = lexeme.ToString().Trim()
                                };
                            }

                            lexeme.Append(nextChar);
                            GetNextChar();
                            return new Token
                            {
                                TokenType = TokenType.GreaterOrEqualThan,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString().Trim()
                            };
                        }

                    case '+':
                            {
                                lexeme.Append(currentChar);
                                var nextChar = PeekNextChar();
                                if(nextChar == '+')
                                {
                                    lexeme.Append(nextChar);
                                    GetNextChar();
                                    return new Token
                                    {
                                        TokenType = TokenType.Increase,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString()
                                    };
                                }
                                return new Token
                                {
                                    TokenType = TokenType.Plus,
                                    Column = input.Position.Column,
                                    Line = input.Position.Line,
                                    Lexeme = lexeme.ToString()
                                };
                            }

                    case '-':
                            {
                                lexeme.Append(currentChar);
                                var nextChar = PeekNextChar();
                                if (nextChar == '-')
                                {
                                    lexeme.Append(nextChar);
                                    GetNextChar();
                                    return new Token
                                    {
                                        TokenType = TokenType.Decrease,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString()
                                    };
                                }

                                return new Token
                                {
                                    TokenType = TokenType.Minus,
                                    Column = input.Position.Column,
                                    Line = input.Position.Line,
                                    Lexeme = lexeme.ToString()
                                };
                            }

                        case '*':
                        lexeme.Append(currentChar);
                        return new Token
                        {
                            TokenType = TokenType.Asterisk,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };

                     case '%':
                        lexeme.Append(currentChar);
                        return new Token
                        {
                            TokenType = TokenType.Mod,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };

                    case '(':
                        lexeme.Append(currentChar);
                        return new Token
                        {
                            TokenType = TokenType.LeftParens,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };

                    case ')':
                        lexeme.Append(currentChar);
                        return new Token
                        {
                            TokenType = TokenType.RightParens,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };

                    case ';':
                        lexeme.Append(currentChar);
                        return new Token
                        {
                            TokenType = TokenType.SemiColon,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };

                    case '=':
                            {
                                lexeme.Append(currentChar);
                                var nextChar = PeekNextChar();
                                if (nextChar == '=')
                                {
                                    lexeme.Append(currentChar);
                                    GetNextChar();
                                    return new Token
                                    {
                                        TokenType = TokenType.RelationalEqual,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString().Trim()
                                    };
                                }
                                return new Token
                                {
                                    TokenType = TokenType.Equal,
                                    Column = input.Position.Column,
                                    Line = input.Position.Line,
                                    Lexeme = lexeme.ToString()
                                };
                            }
                        
                    case ':':
                        lexeme.Append(currentChar);
                        return new Token
                        {
                            TokenType = TokenType.Colon,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };

                    case '\"':
                            {
                                lexeme.Append(currentChar);
                                currentChar = GetNextChar(); //si dejo esto, creo que incluye las comillas al lexema
                                while (currentChar != '\"')
                                {
                                    lexeme.Append(currentChar);
                                    currentChar =  GetNextChar();
                                }
                                lexeme.Append(currentChar);
                                this.symbolsTable.Add(new Symbol(lexeme.ToString(), SymbolType.Literal));
                                return new Token
                                {
                                    TokenType = TokenType.StringLiteral,
                                    Column = input.Position.Column,
                                    Line = input.Position.Line,
                                    Lexeme = lexeme.ToString(),
                                    PositionInSymbolTable = this.symbolsTable.Count - 1
                                };
                            }

                        case '{':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.OpenBrace,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };

                        case '}':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.CloseBrace,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };

                        case '[':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.LeftBracket,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };

                        case ']':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.RightBracket,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };

                        case ',':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Comma,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };

                          case '&':
                            {
                                lexeme.Append(currentChar);
                                var nextChar = PeekNextChar();
                                if (nextChar == '&')
                                {
                                    GetNextChar();
                                    return new Token
                                    {
                                        TokenType = TokenType.LogicalAnd,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString()
                                    };
                                }
                            }
                            break;

                        case '|':
                            {
                                lexeme.Append(currentChar);
                                var nextChar = PeekNextChar();
                                if (nextChar == '|')
                                {
                                    GetNextChar();
                                    return new Token
                                    {
                                        TokenType = TokenType.LogicalOr,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString()
                                    };
                                }
                            }
                            break;

                        case '.':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Point,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };

                        case '!':
                            {
                                lexeme.Append(currentChar);
                                var nextChar = PeekNextChar();
                                if (nextChar != '=')
                                {
                                    return new Token
                                    {
                                        TokenType = TokenType.LogicalNegation,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString().Trim()
                                    };
                                }

                                lexeme.Append(nextChar);
                                GetNextChar();
                                return new Token
                                {
                                    TokenType = TokenType.NotEqual,
                                    Column = input.Position.Column,
                                    Line = input.Position.Line,
                                    Lexeme = lexeme.ToString().Trim()
                                };
                            }


                        case '\0':
                            return new Token
                            {
                                TokenType = TokenType.EOF,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = string.Empty
                            };
                        default:
                        throw new ApplicationException($"Invalid caracter '{lexeme}' in column: {input.Position.Column}, line: ${input.Position.Line} ");
                }
            }
        }

        public char GetNextChar()
        {
            var next = input.NextChar();
            input = next.Reminder;
            return next.Value;
        }
        private char PeekNextChar()
        {
            var next = input.NextChar();
            return next.Value;
        }
    }
}
