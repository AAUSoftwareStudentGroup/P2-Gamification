using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public abstract class OperatorExpression : ExpressionBase
    {
        private static Dictionary<OperatorType, string> symbols = new Dictionary<OperatorType, string>()
        {
            { OperatorType.Add, "+" },
            { OperatorType.Subtract, "-" },
            { OperatorType.Divide, "/" },
            { OperatorType.Multiply, "*" },
            { OperatorType.Power, "^" },
        };

        public string GetTypeString()
        {
            return symbols[Type];
        }

        //public OperatorExpression(ExpressionBase left, ExpressionBase right, OperatorType type)
        protected OperatorExpression(OperatorType type)
        {
            Type = type;
        }

        public OperatorType Type { get; set; }

        public string Symbol
        {
            get
            {
                if (symbols.ContainsKey(Type))
                    return symbols[Type];
                throw new InvalidOperationException("Invalid Type: " + Type);
            }
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