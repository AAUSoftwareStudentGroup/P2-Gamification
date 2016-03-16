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

		public override string TreePrint(string indent, bool isLast)
		{
			Console.WriteLine (indent + "|-" + symbols[Type]);
            indent += (isLast ? "  " : "| ");
            Left.TreePrint (indent, false);
			Right.TreePrint (indent, true);
			return indent;
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