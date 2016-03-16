using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class DelimiterExpression : ExpressionBase
	{
        public DelimiterExpression(ExpressionBase expression)
        {
            Expression = expression;
            Expression.Parent = this;
        }

        public ExpressionBase Expression { get; protected set; }

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

        public override ExpressionBase Clone()
        {
            return new DelimiterExpression(Expression.Clone());
        }

        public override bool Replace(ExpressionBase old, ExpressionBase replacement)
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
            yield return Expression;

            foreach (var node in Expression.GetNodesRecursive())
                yield return node;
		}

		public override string TreePrint(string indent, bool isLast)
		{
			Console.WriteLine (indent + "|-" + "()");
            indent += (isLast ? "  " : "| ");
            this.Expression.TreePrint (indent, true);
			return indent;
		}
    }
}
