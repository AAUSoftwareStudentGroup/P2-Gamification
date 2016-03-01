using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Framework
{
    public interface IExpression
    {
        string Value { get; }
        bool Equals(Expression other);
    }
}