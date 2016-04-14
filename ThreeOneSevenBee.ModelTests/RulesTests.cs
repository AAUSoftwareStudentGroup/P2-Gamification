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

        [TestMethod]
        public void Rules_NumericVariadicRule()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;
            Identity identity;


            parent = ExpressionTests.Add(
                selection1 = ExpressionTests.New(2),
                selection2 = ExpressionTests.New(2));

            identity = Rules.NumericVariadicRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(ExpressionTests.New(4), identity.Suggestion);


            parent = ExpressionTests.Multiply(
                selection1 = ExpressionTests.New(3),
                selection2 = ExpressionTests.New(3));

            identity = Rules.NumericVariadicRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(ExpressionTests.New(9), identity.Suggestion);
        }

        [TestMethod]
        public void Rules_NumericBinaryRule()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;
            Identity identity;


            parent = ExpressionTests.Subtract(
                selection1 = ExpressionTests.New(2),
                selection2 = ExpressionTests.New(2));

            identity = Rules.NumericBinaryRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(ExpressionTests.New(0), identity.Suggestion);


            /*parent = ExpressionTests.Divide(
                selection1 = ExpressionTests.New(3),
                selection2 = ExpressionTests.New(3));

            identity = Rules.NumericBinaryRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(ExpressionTests.New(1), identity.Suggestion);*/


            parent = ExpressionTests.Power(
                selection1 = ExpressionTests.New(3),
                selection2 = ExpressionTests.New(3));

            identity = Rules.NumericBinaryRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(ExpressionTests.New(27), identity.Suggestion);
        }
    }
}
