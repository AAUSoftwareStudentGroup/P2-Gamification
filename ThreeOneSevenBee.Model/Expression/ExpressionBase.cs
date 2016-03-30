using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression
{
    public abstract class ExpressionBase : IEquatable<ExpressionBase>
    {
        public static bool operator ==(ExpressionBase left, ExpressionBase right)
        {
            if (System.Object.ReferenceEquals(left, null))
            {
                // ...and right hand side is null...
                if (System.Object.ReferenceEquals(right, null))
                {
                    //...both are null and are Equal.
                    return true;
                }

                // ...right hand side is not null, therefore not Equal.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(ExpressionBase left, ExpressionBase right)
        {
            return !(left == right);
        }

        public ExpressionBase Parent { get; set; }

        public abstract string Value { get; }



        public abstract ExpressionBase Clone();

        public abstract bool Replace(ExpressionBase old, ExpressionBase replacement);

        public abstract IEnumerable<ExpressionBase> GetNodesRecursive();

        public IEnumerable<ExpressionBase> GetParentPath()
        {
            ExpressionBase currentParent = this;
            while (currentParent != null)
            {
                yield return currentParent;
                currentParent = currentParent.Parent;
            }
        }

        public abstract bool CanCalculate();

        public abstract double? Calculate();

        public abstract int Size { get; }

        public override bool Equals(object other)
        {
            return (other is ExpressionBase) && Equals((ExpressionBase)other);
        }

        public abstract bool Equals(ExpressionBase other);

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public abstract string TreePrint(string indent, bool isLast);

        public void PrettyPrint()
        {
            this.TreePrint("", true);
        }
    }
}