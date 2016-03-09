using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return base.CanCalculate();
        }

        public override double? Calculate()
        {
            Func<double, double> func;
            if (functions.TryGetValue(Function, out func))
                return func(Expression.Calculate().Value);
            return base.Calculate();
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield return this;

            foreach (var node in Expression.GetNodesRecursive())
                yield return node;
        }
    }
}
