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

        ExpressionBase expression;
        public ExpressionBase Expression
        {
            get
            {
                return expression;
            }

            set
            {
                expression = value;
                expression.Parent = this;
            }
        }

        public override string Value
        {
            get
            {
                return "(" + Expression.ToString() + ")";
            }
        }

        public override bool Replace(ExpressionBase old, ExpressionBase replacement, bool doRecursively)
        {
            var hasReplaced = false;

            if (Object.ReferenceEquals(Expression, old))
            {
                Expression = replacement.Clone();
                Expression.Parent = this;
                hasReplaced |= true;
            }
            else if (doRecursively)
            {
                hasReplaced |= Expression.Replace(old, replacement, true);
            }

            return hasReplaced;
        }

        public override int Size
        {
            get
            {
                return 1 + Expression.Size;
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

        public override bool Equals(ExpressionBase otherBase)
        {
            var other = (otherBase as DelimiterExpression);

            if (other == null)
                return false;

            return this.Expression == other.Expression;
        }

        public override ExpressionBase Clone()
        {
            return new DelimiterExpression(Expression.Clone()) { Selected = Selected };
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield return Expression;

            foreach (var node in Expression.GetNodesRecursive())
                yield return node;
        }

        public override string ToString()
        {
            return "(" + Expression.ToString() + ")";
        }

        public override string TreePrint(string indent, bool isLast)
        {
            Console.WriteLine(indent + "|-" + "()");
            indent += (isLast ? "  " : "| ");
            this.Expression.TreePrint(indent, true);
            return indent;
        }
    }
}
