using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class VariableExpression : ExpressionBase
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

        public void SetValue(string value)
        {
            this.value = value;
        }

        public override ExpressionBase Clone()
        {
            return new VariableExpression(Value);
        }

        public override Boolean Replace(ExpressionBase old, ExpressionBase replacement)
        {
            return false;
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield return this;
		}

		public override string TreePrint(string indent, bool isLast)
		{
			Console.Write (indent+"|-");
			indent += (isLast ? "  " : "| ");
			Console.WriteLine (Value, true);
			return indent;
		}
    }
}
