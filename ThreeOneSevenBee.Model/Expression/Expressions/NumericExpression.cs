using System;
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

        public override ExpressionBase Clone()
        {
            return new NumericExpression(value);
        }

        public override Boolean Replace(ExpressionBase old, ExpressionBase replacement)
        {
            return false;
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
