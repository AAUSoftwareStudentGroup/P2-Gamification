using System;
using System.Collections.Generic;

namespace threeonesevenbee.Model.Expression.Expressions
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
    }
}
