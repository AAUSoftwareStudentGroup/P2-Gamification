using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression
{
    public abstract class ExpressionBase : IEquatable<ExpressionBase>
    {
        ExpressionAnalyzer analyzer = new ExpressionAnalyzer();

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

        /// <summary>
        /// The parent of this node. If this is null, this node is the root of the tree.
        /// </summary>
        public ExpressionBase Parent { get; set; }

        /// <summary>
        /// A string representation of the value of the tree.
        /// </summary>
        public abstract string Value { get; }

        public virtual bool Selected { get; set; } = false;

        public virtual bool ToBeReplaced { get; set; } = false;

        /// <summary>
        /// Clones the tree recursively, making new references all the way.
        /// </summary>
        public abstract ExpressionBase Clone();

        /// <summary>
        /// Does a reference replace, replacing the old expression with a clone of the replacement expression. Optionally does it recursively.
        /// </summary>
        /// <param name="old">The expression to replace.</param>
        /// <param name="replacement">The expresison to replace with.</param>
        /// <param name="doRecursively">Perform the replacement recursively.</param>
        /// <returns>True if a replacement took place.</returns>
        public abstract bool Replace(ExpressionBase old, ExpressionBase replacement, bool doRecursively);

        /// <summary>
        /// Replaces this expression with the replacement expression if possible.
        /// </summary>
        /// <param name="replacement"></param>
        /// <returns>True if a replacement took place.</returns>
        public bool Replace(ExpressionBase replacement)
        {
            if (Parent != null)
            {
                Parent.Replace(this, replacement, false);
            }
            return false;
        }

        /// <summary>
        /// Returns all elements in the tree recursively.
        /// </summary>
        public abstract IEnumerable<ExpressionBase> GetNodesRecursive();

        /// <summary>
        /// Returns the path of nodes to get to the root of the tree.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ExpressionBase> GetParentPath()
        {
            ExpressionBase currentParent = this;
            while (currentParent != null)
            {
                yield return currentParent;
                currentParent = currentParent.Parent;
            }
        }

        /// <summary>
        /// Determines whether the expression can be calculated or not.
        /// </summary>
        /// <returns></returns>
        public abstract bool CanCalculate();

        /// <summary>
        /// Calculates the expression.
        /// </summary>
        /// <returns>The result or null if the tree cannot be calculated.</returns>
        public abstract double? Calculate();

        /// <summary>
        /// An arbitrary size score, to determine which expression is better.
        /// </summary>
        public abstract int Size { get; }

        /// <summary>
        /// Determines if the other expression has the same value, but not reference.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            return (other is ExpressionBase) && Equals((ExpressionBase)other);
        }

        /// <summary>
        /// Determines if the other expression has the same value, but not reference.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool Equals(ExpressionBase other);

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return "{" + Value + "}";
        }

        /// <summary>
        /// Prints the tree to the standard output stream.
        /// </summary>
        public abstract string TreePrint(string indent, bool isLast);

        /// <summary>
        /// Prints the tree to the standard output stream.
        /// </summary>
        public void PrettyPrint()
        {
            this.TreePrint(String.Empty, true);
        }
    }
}