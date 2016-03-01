using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Framework.Expressions
{
    public class DelimiterExpression : Expression
    {
        public DelimiterExpression(Expression expression, DelimiterType type)
        {
            Expression = expression;
            Type = type;
        }

        public Expression Expression { get; protected set; }

        public DelimiterType Type { get; protected set; }

        public override string Value
        {
            get
            {
                switch (Type)
                {
                    case DelimiterType.Parenthesis:
                        return "(" + Expression.ToString() + ")";
                    case DelimiterType.Curly:
                        return "{" + Expression.ToString() + "}";
                    case DelimiterType.Bracket:
                        return "[" + Expression.ToString() + "]";
                    default:
                        throw new InvalidOperationException("Invalid expression type: " + Type);
                }
            }
        }
    }

    public enum DelimiterType
    {
        Parenthesis,
        Curly,
        Bracket
    }
}
