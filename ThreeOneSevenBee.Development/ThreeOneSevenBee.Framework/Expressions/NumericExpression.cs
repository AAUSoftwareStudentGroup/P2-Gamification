using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Framework.Expressions
{
    public class NumericExpression : Expression
    {
        private int value;

        public NumericExpression(int value)
        {
            this.value = value;
        }

        public override string Value
        {
            get { return value.ToString(); }
        }
    }
}
