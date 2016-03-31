using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Expression
{
    public class Identity
    {
        public ExpressionBase Suggestion, Result;
        public Identity(ExpressionBase suggestion, ExpressionBase result)
        {
            Suggestion = suggestion;
            Result = result;
        }
    }
}
