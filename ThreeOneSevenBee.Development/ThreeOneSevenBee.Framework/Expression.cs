using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Framework
{
    public abstract class Expression : IEquatable<Expression>
    {
        public abstract string Value { get; }

        public abstract IEnumerable<Expression> GetNodesRecursive();

        public virtual bool CanCalculate() { return false; }

        public virtual double? Calculate() { return null; }

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