using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
