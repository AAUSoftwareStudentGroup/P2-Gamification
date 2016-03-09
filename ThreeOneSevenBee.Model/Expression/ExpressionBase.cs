using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression
{
    public abstract class ExpressionBase : IEquatable<ExpressionBase>
    {
        public ExpressionBase Parent { get; set; }

        public abstract string Value { get; }

        public abstract ExpressionBase Clone();

        public abstract IEnumerable<ExpressionBase> GetNodesRecursive();

        public virtual bool CanCalculate() { return false; }

        public virtual double? Calculate() { return null; }

        public override bool Equals(object other)
        {
            return (other is ExpressionBase) && Equals((ExpressionBase)other);
        }

        public bool Equals(ExpressionBase other)
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