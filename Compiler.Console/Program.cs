using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using System;
using System.IO;

namespace Compiler.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = File.ReadAllText("Code.txt").Replace(Environment.NewLine, "\n");
            var input = new Input(code);
            var scanner = new Scanner(input);
            Token token;
            do
            {
                token = scanner.GetNextToken();
                System.Console.WriteLine(token.ToString());
            } while (token.TokenType != TokenType.EOF);
        }
    }
}
