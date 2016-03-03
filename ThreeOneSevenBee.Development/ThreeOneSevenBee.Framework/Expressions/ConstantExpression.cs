using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Framework.Expressions
{
    public class ConstantExpression : Expression
    {
        private ConstantType value;

        public ConstantExpression(ConstantType value)
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
            switch (value)
            {
                case ConstantType.Pi:
                    return true;
            }

            return base.CanCalculate();
        }

        public override double? Calculate()
        {
            switch (value)
            {
                case ConstantType.Pi:
                    return Math.PI;
            }

            return base.Calculate();
        }
    }

    public enum ConstantType
    {
        Pi,
    }
}
