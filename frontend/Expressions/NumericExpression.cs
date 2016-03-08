using System.Collections.Generic;

namespace ThreeOneSevenBee.Framework.Expressions
{
    public class NumericExpression : Expression
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

        public override IEnumerable<Expression> GetNodesRecursive()
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
