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
            Right = right;
        }

        ExpressionBase left;
        public ExpressionBase Left
        {
            get
            {
                return left;
            }

            set
            {
                ExpressionBase wrapped = new ExpressionAnalyzer().WrapInDelimiterIfNeccessary(value, this);
                left = wrapped;
                left.Parent = this;
            }
        }

        ExpressionBase right;
        public ExpressionBase Right
        {
            get
            {
                return right;
            }

            set
            {
                right = value;
                right.Parent = this;
            }
        }

        public override Boolean Replace(ExpressionBase old, ExpressionBase replacement, bool doRecursively)
        {
            var hasReplaced = false;

            if (Object.ReferenceEquals(Left, old))
            {
                Left = replacement.Clone();
                Left.Parent = this;
                hasReplaced |= true;
            }
            else if (doRecursively)
            {
                hasReplaced |= Left.Replace(old, replacement, true);
            }

            if (Object.ReferenceEquals(Right, old))
            {
                Right = replacement.Clone();
                Right.Parent = this;
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
