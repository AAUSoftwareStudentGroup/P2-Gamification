﻿using System;
using System.Collections.Generic;
#if BRIDGE
using Bridge.Html5;
#endif

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class FunctionExpression : ExpressionBase
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

        public string Function { get; protected set; }

        public override string Value
        {
            get
            {
                return Function + Expression;
            }
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

        public override bool Equals(ExpressionBase otherBase)
        {
            var other = (otherBase as FunctionExpression);

            if (other == null)
                return false;

            return this.Function == other.Function && this.Expression == other.Expression;
        }

        public override ExpressionBase Clone()
        {
            return new FunctionExpression(Expression.Clone(), Function) { Selected = Selected };
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

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield return this;

            foreach (var node in Expression.GetNodesRecursive())
                yield return node;
        }

        public override string ToString()
        {
            return Function + "{" + Expression + "}";
        }

        public override string TreePrint(string indent, bool isLast)
        {
            Console.WriteLine(indent + "|-" + Function);
            indent += (isLast ? "  " : "| ");
            this.Expression.TreePrint(indent, true);
            return indent;
        }
    }
}
