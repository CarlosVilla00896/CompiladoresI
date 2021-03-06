using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core
{
    public enum SymbolType
    {
        Variable,
        Method
    }

    public class Symbol
    {
        public SymbolType SymbolType { get; }
        public Id Id { get; }
        public dynamic Value { get; set; }
        public Expression Attributes { get; }
        public Symbol(SymbolType symbolType, Id id, dynamic value)
        {
            SymbolType = symbolType;
            Id = id;
            Value = value;
        }

        public Symbol(SymbolType symbolType, Id id, Expression attributes)
        {
            Attributes = attributes;
            SymbolType = symbolType;
            Id = id;
        }
    }
}
