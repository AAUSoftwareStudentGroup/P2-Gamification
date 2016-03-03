using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Framework.Expressions
{
    public class OperatorExpression : Expression
    {
        private static Dictionary<OperatorType, string> symbols = new Dictionary<OperatorType, string>()
        {
            { OperatorType.Add, "+" },
            { OperatorType.Subtract, "-" },
            { OperatorType.Divide, "/" },
            { OperatorType.Multiply, "*" },
            { OperatorType.Power, "^" },
        };

        public OperatorExpression(Expression left, Expression right, OperatorType type)
        {
            Left = left;
            Right = right;
            Type = type;
        }
        public Expression Left { get; private set; }

        public Expression Right { get; private set; }

        public OperatorType Type { get; private set; }

        public override string Value
        {
            get
            {
                string symbol;
                if (symbols.TryGetValue(Type, out symbol))
                    return Left.ToString() + symbol + Right.ToString();
                throw new InvalidOperationException("Invalid operator type: " + Type);
            }
        }

        public override bool CanCalculate()
        {
            return Left.CanCalculate() && Right.CanCalculate();
        }

        public override double? Calculate()
        {
            var left = Left.Calculate();
            var right = Right.Calculate();

            if (left == null || right == null)
                return base.Calculate();

            switch (Type)
            {
                case OperatorType.Add:
                    return left + right;
                case OperatorType.Subtract:
                    return left - right;
                case OperatorType.Divide:
                    return left / right;
                case OperatorType.Multiply:
                    return left * right;
                case OperatorType.Power:
                    return Math.Pow(left.Value, right.Value);
            }

            return base.Calculate();
        }

        public override IEnumerable<Expression> GetNodesRecursive()
        {
            yield return this;

            foreach (var node in Left.GetNodesRecursive())
                yield return node;
            foreach (var node in Right.GetNodesRecursive())
                yield return node;
        }
    }

    public enum OperatorType
    {
        Add,
        Subtract,
        Divide,
        Multiply,
        Power
    }
}