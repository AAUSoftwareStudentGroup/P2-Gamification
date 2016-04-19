using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public abstract class BinaryExpression : OperatorExpression
    {
        protected BinaryExpression(ExpressionBase left, ExpressionBase right, OperatorType type)
            : base(type)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            Left = left;
            Left.Parent = this;
            Right = right;
            Right.Parent = this;
        }

        public ExpressionBase Left { get; set; }

        public ExpressionBase Right { get; set; }

        public override Boolean Replace(ExpressionBase old, ExpressionBase replacement, bool doRecursively)
        {
            var hasReplaced = false;

            if (Object.ReferenceEquals(Left, old))
            {
                Left = replacement.Clone();
                hasReplaced |= true;
            }
            else if (doRecursively)
            {
                hasReplaced |= Left.Replace(old, replacement, true);
            }

            if (Object.ReferenceEquals(Right, old))
            {
                Right = replacement.Clone();
                hasReplaced |= true;
            }
            else if (doRecursively)
            {
                hasReplaced |= Right.Replace(old, replacement, true);
            }

            return hasReplaced;
        }
    }
}
