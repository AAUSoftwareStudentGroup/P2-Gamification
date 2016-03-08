using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class NumericExpression : ExpressionBase
    {
        private double value;

        public NumericExpression(double value)
        {
            this.value = value;
        }

        public override string Value
        {
            get { return value.ToString(); }
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield return this;
        }

        public override bool CanCalculate()
        {
            return true;
        }

        public override double? Calculate()
        {
            return value;
        }
    }
}
