using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class VariableExpression : ExpressionBase
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

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield return this;
        }
    }
}
