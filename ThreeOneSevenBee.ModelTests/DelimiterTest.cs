using System;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.Expressions;

namespace ThreeOneSevenBee.ModelTests
{
    [TestClass]
    public class DelimiterAndEqualsTests
    {
        [TestMethod]
        public void BracketTest()
        {
            var parser = new ExpressionSerializer();
            var expressionTree = parser.Deserialize("2/{2*2}");
            var otherExpressionTree = parser.Deserialize("{2/2}*2");
            Assert.IsFalse(expressionTree.Equals(otherExpressionTree));

        }

    }
}