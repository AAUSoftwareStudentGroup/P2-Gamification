using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Expression
{
    public interface IExpression
    {
        string Value { get; }
        bool Equals(ExpressionBase other);
    }
}