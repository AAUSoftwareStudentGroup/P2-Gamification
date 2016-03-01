using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Framework.Expressions
{
    public class OperatorExpression : BinaryExpression
    {
        private static Dictionary<OperatorType, string> symbols = new Dictionary<OperatorType, string>()
        {
            { OperatorType.Add, "+" },
            { OperatorType.Subtract, "-" },
            { OperatorType.Divide, "/" },
            { OperatorType.Multiply, "*" },
        };

        public OperatorExpression(Expression left, Expression right, OperatorType type)
            : base(left, right)
        {
            Type = type;
        }

        public OperatorType Type { get; private set; }

        public override string Value
        {
            get
            {
                string symbol;
                if (symbols.TryGetValue(Type, out symbol))
                    return "{" + Left.ToString() + "} " + symbol + " {" + Right.ToString() + "}";
                throw new InvalidOperationException("Invalid operator type: " + Type);
            }
        }
    }

    public enum OperatorType
    {
        Add,
        Subtract,
        Divide,
        Multiply
    }
}
