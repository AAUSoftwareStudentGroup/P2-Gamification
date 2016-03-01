using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Framework
{
    public abstract class Expression : IExpression
    {
        public abstract string Value { get; }

        public override bool Equals(object other)
        {
            return (other is Expression) && Equals((Expression)other);
        }

        public bool Equals(Expression other)
        {
            return ToString() == other.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}