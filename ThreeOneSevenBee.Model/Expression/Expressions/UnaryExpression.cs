using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public abstract class UnaryExpression : OperatorExpression
    {
        protected UnaryExpression(OperatorType type, ExpressionBase expression)
            : base(type)
        {
            if (type != OperatorType.Subtract)
                throw new ArgumentException("Invalid Type: " + type, "type");
            if (expression == null)
                throw new ArgumentNullException("expression");

            Expression = expression;
            Expression.Parent = this;
        }

        public ExpressionBase Expression { get; set; }
    }
}
