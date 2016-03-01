using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Framework.Expressions
{
    public abstract class BinaryExpression : Expression
    {
        public Expression Left { get; protected set; }

        public Expression Right { get; protected set; }

        protected BinaryExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }
    }
}
