using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;

namespace threeonesevenbee.Model.Expression.Expressions
{
    public class UnaryMinusExpression : UnaryExpression
    {
        public UnaryMinusExpression(ExpressionBase expression)
            : base(OperatorType.Subtract, expression)
        { }

        public override string Value
        {
            get { return "-" + Expression.ToString(); }
        }

        public override bool CanCalculate()
        {
            return Expression.CanCalculate();
        }

        public override double? Calculate()
        {
            return -Expression.Calculate();
        }

        public override bool Equals(ExpressionBase otherBase)
        {
            var other = (otherBase as UnaryMinusExpression);

            if (other == null)
                return false;

            return this.Expression == other.Expression;
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
            yield return Expression;

            foreach (var node in Expression.GetNodesRecursive())
                yield return node;
        }

		public override string TreePrint(string indent, bool isLast)
		{
			Console.WriteLine (indent + "|-" + "-");
            indent += (isLast ? "  " : "| ");
            Expression.TreePrint (indent, true);
			return indent;
		}
    }
}
