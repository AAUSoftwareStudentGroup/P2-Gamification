using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace threeonesevenbee.Model.Expression
{
    /// <summary>
    /// A generel representation of an expression rule.
    /// </summary>
    public delegate bool ExpressionRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity);
}