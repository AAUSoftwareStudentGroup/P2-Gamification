using System.Collections.Generic;

namespace ThreeOneSevenBee.Framework.Expressions
{
    public class VariableExpression : Expression
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

        public override IEnumerable<Expression> GetNodesRecursive()
        {
            yield return this;
        }
    }
}
