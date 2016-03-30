using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Expression
{
    /// <summary>
    /// A generel representation of an expression rule.
    /// </summary>
    public delegate Identity ExpressionRule(ExpressionBase expression, List<ExpressionBase> selection);
}