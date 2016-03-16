using System;
using System.Collections.Generic;
#if BRIDGE
using Bridge.Html5;
#endif

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    class FunctionExpression : ExpressionBase
    {
        private static Dictionary<string, Func<double, double>> functions = new Dictionary<string, Func<double, double>>()
        {
            { "sqrt", Math.Sqrt },
            { "sin", Math.Sin },
            { "cos", Math.Cos },
            { "tan", Math.Tan },
        };

        public FunctionExpression(ExpressionBase expression, string function)
        {
            Expression = expression;
            Expression.Parent = this;
            Function = function.ToLower();
        }

        public ExpressionBase Expression { get; protected set; }

        public string Function { get; protected set; }

        public override string Value
        {
            get
            {
                return Function + Expression;
            }
        }

        public override bool CanCalculate()
        {
            if (functions.ContainsKey(Function))
                return Expression.CanCalculate();
            return false;
        }

        public override double? Calculate()
        {
            Func<double, double> func;
            if (functions.TryGetValue(Function, out func))
                return func(Expression.Calculate().Value);
            return null;
        }

        public override ExpressionBase Clone()
        {
            return new FunctionExpression(Expression.Clone(), Function);
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
            yield return this;

            foreach (var node in Expression.GetNodesRecursive())
                yield return node;
        }

		public override string TreePrint(string indent, bool isLast)
		{
			Console.WriteLine (indent + "|-" + Function);
            indent += (isLast ? "  " : "| ");
            this.Expression.TreePrint (indent, true);
			return indent;
		}
    }
}
