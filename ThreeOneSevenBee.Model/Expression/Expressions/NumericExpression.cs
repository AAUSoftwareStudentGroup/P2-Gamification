using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class NumericExpression : ExpressionBase, ILeaf
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
            yield break;
        }

        public override bool CanCalculate()
        {
            return true;
        }

        public override double? Calculate()
        {
            return value;
		}

        public override bool Equals(ExpressionBase otherBase)
        {
            var other = (otherBase as NumericExpression);

            if (other == null)
                return false;

            return (Math.Abs(this.value - other.value) < double.Epsilon);
        }

        public override string TreePrint(string indent, bool isLast)
		{
			Console.WriteLine (indent + "|-" + Value);
			return indent + (isLast ? "  " : "| ");
		}
    }
}
