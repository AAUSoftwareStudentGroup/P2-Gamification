using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.ExpressionRules;
using System.Collections.Generic;

namespace ThreeOneSevenBee.ModelTests
{
    [TestClass]
    public class RulesTests
    {
        [TestMethod]
        public void Rules_ProductExponent()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;

            parent = ExpressionTests.Multiply(
                selection1 = ExpressionTests.New("a"),
                selection2 = ExpressionTests.New("a"));

            var identity = Rules.ProductToExponentRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(ExpressionTests.Power(ExpressionTests.New("a"), ExpressionTests.New(2)), identity.Suggestion);
        }

        [TestMethod]
        public void Rules_ExponentProduct()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;

            parent = ExpressionTests.Power(
                selection1 = ExpressionTests.New("a"),
                selection2 = ExpressionTests.New(2));

            var identity = Rules.ExponentToProductRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(ExpressionTests.Multiply(ExpressionTests.New("a"), ExpressionTests.New("a")), identity.Suggestion);
        }
    }
}
