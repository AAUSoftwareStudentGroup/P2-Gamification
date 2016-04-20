using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class VariableExpression : ExpressionBase, ILeaf
    {
        private string value;

        public VariableExpression(string value)
        {
            this.value = value;
        }

        public override string Value
        {
            get { return value; }
        }

        public override int Size
        {
            get
            {
                return 2;
            }
        }

        public void SetValue(string value)
        {
            this.value = value;
        }

        public override Boolean CanCalculate()
        {
            return false;
        }

        public override Double? Calculate()
        {
            return null;
        }

        public override bool Equals(ExpressionBase otherBase)
        {
            var other = (otherBase as VariableExpression);

            if (other == null)
                return false;

            return this.value == other.value;
        }

        public override ExpressionBase Clone()
        {
            return new VariableExpression(Value) { Selected = Selected };
        }

        public override bool Replace(ExpressionBase old, ExpressionBase replacement, bool doRecursively)
        {
            return false;
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield break;
        }

        public override string ToString()
        {
            return Value;
        }

        public override string TreePrint(string indent, bool isLast)
		{
			Console.WriteLine (indent + "|-" + Value, true);
			return indent + (isLast ? "  " : "| ");
		}
    }
}
