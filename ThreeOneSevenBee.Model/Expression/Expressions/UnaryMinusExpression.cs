using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class UnaryMinusExpression : ExpressionBase
    {
        public UnaryMinusExpression(ExpressionBase expression)
        {
            Expression = expression;
            Expression.Parent = this;
        }

        public ExpressionBase Expression { get; protected set; }

        public override string Value
        {
            get
            {
                return "-" + Expression.ToString();
            }
        }

        public override bool CanCalculate()
        {
            return Expression.CanCalculate();
        }

        public override double? Calculate()
        {
            return -Expression.Calculate();
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield return this;

            foreach (var node in Expression.GetNodesRecursive())
                yield return node;
        }
    }
}
