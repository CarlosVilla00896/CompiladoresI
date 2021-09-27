using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class ConsoleStatement : Statement
    {
        public Expression Expression { get; }
        public bool IsWriteLine { get; }

        public ConsoleStatement(Expression expression, bool isWriteLine)
        {
            Expression = expression;
            IsWriteLine = isWriteLine;
        }

        public ConsoleStatement(bool isWriteLine)
        {
            IsWriteLine = isWriteLine;
        }

        public override void ValidateSemantic()
        {
            return;
        }

        public override string Generate()
        {   
            if( IsWriteLine)
            {
                var code = $"console.log({Expression.Generate()});{Environment.NewLine}";
                code = code.Replace('\"', '`');
                return code;
            }
            return $"prompt();{Environment.NewLine}";
        }
    }
}
