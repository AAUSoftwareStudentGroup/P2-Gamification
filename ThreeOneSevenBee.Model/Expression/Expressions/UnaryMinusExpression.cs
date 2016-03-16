using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class UnaryMinusExpression : UnaryExpression
    {
        public UnaryMinusExpression(ExpressionBase expression)
            : base(OperatorType.Subtract, expression)
        { }

        public override string Value
        {
            get { return "-" + Expression.ToString(); }
        }

        public override bool CanCalculate()
        {
            return Expression.CanCalculate();
        }

        public override double? Calculate()
        {
            return -Expression.Calculate();
        }

        public override ExpressionBase Clone()
        {
            return new UnaryMinusExpression(Expression.Clone());
        }

        public override Boolean Replace(ExpressionBase old, ExpressionBase replacement)
        {
            if (Expression == old)
            {
                Expression = replacement;
                return true;
            }
            return Expression.Replace(old, replacement);
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield return Expression;

            foreach (var node in Expression.GetNodesRecursive())
                yield return node;
        }
    }
}
