using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class ConstantExpression : ExpressionBase
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

        public override ExpressionBase Clone()
        {
            return new ConstantExpression(value);
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

		public override string TreePrint(string indent, bool isLast) 
		{
			Console.Write (indent+"|-");
			indent += (isLast ? "  " : "| ");
			Console.WriteLine (Value);
			return indent;
		}
    }

    public enum ConstantType
    {
        Pi,
    }
}
