using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression
{
    public abstract class ExpressionBase : IEquatable<ExpressionBase>
    {
        public ExpressionBase Parent { get; set; }

        public abstract string Value { get; }

        public abstract ExpressionBase Clone();

        public abstract bool Replace(ExpressionBase old, ExpressionBase replacement);

        public abstract IEnumerable<ExpressionBase> GetNodesRecursive();

        public IEnumerable<ExpressionBase> GetParentPath()
        {
            ExpressionBase currentParent = Parent;
            while (currentParent != null)
            {
                yield return currentParent;
                currentParent = currentParent.Parent;
            }
        }

        public abstract bool CanCalculate();

        public abstract double? Calculate();

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