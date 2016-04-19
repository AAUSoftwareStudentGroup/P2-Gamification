using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class UnaryMinusExpression : UnaryExpression
    {
        public UnaryMinusExpression(ExpressionBase expression)
            : base(OperatorType.Minus, expression)
        { }

        public override string Value
        {
            get { return "-" + Expression; }
        }

        public override int Size
        {
            get
            {
                if (Expression is NumericExpression)
                {
                    return Expression.Size;
                }
                else
                {
                    return 3 + Expression.Size;
                }
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

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield return Expression;

            foreach (var node in Expression.GetNodesRecursive())
                yield return node;
        }

        public override string ToString()
        {
            return "-{" + Expression + "}";
        }

        public override string TreePrint(string indent, bool isLast)
        {
            Console.WriteLine(indent + "|-" + "-");
            indent += (isLast ? "  " : "| ");
            Expression.TreePrint(indent, true);
            return indent;
        }
    }
}
