using System;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class VariableExpression : ExpressionBase, ILeaf
    {
        private string value;

        public VariableExpression(string value)
        {
            this.value = value;
        }

        public override string Value
        {
            get { return value; }
        }

        public void SetValue(string value)
        {
            this.value = value;
        }

        public override Boolean CanCalculate()
        {
            return false;
        }

        public override Double? Calculate()
        {
            return null;
        }

        public override ExpressionBase Clone()
        {
            return new VariableExpression(Value);
        }

        public override Boolean Replace(ExpressionBase old, ExpressionBase replacement)
        {
            return false;
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield break;
        }
    }
}
