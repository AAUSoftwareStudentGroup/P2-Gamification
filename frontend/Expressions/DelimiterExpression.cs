using System.Collections.Generic;

namespace ThreeOneSevenBee.Framework.Expressions
{
    public class DelimiterExpression : Expression
    {
        public DelimiterExpression(Expression expression)
        {
            Expression = expression;
        }

        public Expression Expression { get; protected set; }

        public override string Value
        {
            get
            {
                return "(" + Expression.ToString() + ")";
            }
        }

        public override bool CanCalculate()
        {
            return Expression.CanCalculate();
        }

        public override double? Calculate()
        {
            return Expression.Calculate();
        }

        public override IEnumerable<Expression> GetNodesRecursive()
        {
            yield return this;

            foreach (var node in Expression.GetNodesRecursive())
                yield return node;
        }
    }
}
