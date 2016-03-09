using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class OperatorExpression : ExpressionBase
    {
        private static Dictionary<OperatorType, string> symbols = new Dictionary<OperatorType, string>()
        {
            { OperatorType.Add, "+" },
            { OperatorType.Subtract, "-" },
            { OperatorType.Divide, "/" },
            { OperatorType.Multiply, "*" },
            { OperatorType.Power, "^" },
        };

        public OperatorExpression(ExpressionBase left, ExpressionBase right, OperatorType type)
        {
            Left = left;
            Left.Parent = this;
            Right = right;
            Right.Parent = this;
            Type = type;
        }
        public ExpressionBase Left { get; private set; }

        public ExpressionBase Right { get; private set; }

        public OperatorType Type { get; private set; }

        public override string Value
        {
            get
            {
                if (symbols.ContainsKey(Type))
                    return Left.ToString() + symbols[Type] + Right.ToString();
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

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
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