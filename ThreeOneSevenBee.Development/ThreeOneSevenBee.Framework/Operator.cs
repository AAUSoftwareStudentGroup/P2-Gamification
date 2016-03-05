using System;

namespace ThreeOneSevenBee.Framework
{
    public class Operator
    {
        public Operator(string symbol, int precedence, OperatorAssociativity associativity)
        {
            Symbol = symbol;
            Precedence = precedence;
            Associativity = associativity;
        }

        public Operator(string symbol)
            : this(symbol, default(int), default(OperatorAssociativity))
        { }

        public string Symbol { get; set; }

        public int Precedence { get; set; }

        public OperatorAssociativity Associativity { get; set; }

        public override int GetHashCode()
        {
            return Symbol.GetHashCode() + Precedence.GetHashCode() + Associativity.GetHashCode();
        }

        public override string ToString()
        {
            return Symbol;
        }
    }

    public enum OperatorAssociativity
    {
        Left,
        Right
    }
}

