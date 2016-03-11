using System;
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

        public override ExpressionBase Clone()
        {
            return new UnaryMinusExpression(Expression.Clone());
        }

        public override Boolean Replace(ExpressionBase old, ExpressionBase replacement)
        {
            if (Expression == old)
            {
                Expression = replacement;
                return true;
            }
            return Expression.Replace(old, replacement);
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield return this;

            foreach (var node in Expression.GetNodesRecursive())
                yield return node;
		}

		public override string TreePrint(string indent, bool isLast)
		{
			Console.Write (indent+"|-");
			indent += (isLast ? "  " : "| ");
			Console.WriteLine ("-");
			Expression.TreePrint (indent, true);
			return indent;
		}
    }
}
