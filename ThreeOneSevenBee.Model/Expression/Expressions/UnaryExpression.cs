using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public abstract class UnaryExpression : OperatorExpression
    {
        protected UnaryExpression(OperatorType type, ExpressionBase expression)
            : base(type)
        {
            if (type != OperatorType.Minus)
                throw new ArgumentException("Invalid Type: " + type, "type");
            if (expression == null)
                throw new ArgumentNullException("expression");

            Expression = expression;
            Expression.Parent = this;
        }

        public ExpressionBase Expression { get; set; }

        public override bool Replace(ExpressionBase old, ExpressionBase replacement, bool doRecursively)
        {
            var hasReplaced = false;

            if (Object.ReferenceEquals(Expression, old))
            {
                Expression = replacement.Clone();
                Expression.Parent = this;
                hasReplaced |= true;
            }
            else if (doRecursively)
            {
                hasReplaced |= Expression.Replace(old, replacement, true);
            }

            return hasReplaced;
        }
    }
}
