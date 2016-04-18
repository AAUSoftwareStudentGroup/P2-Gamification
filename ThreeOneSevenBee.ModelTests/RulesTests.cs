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

            // a*a => a^2
            parent = Make.Multiply(
                selection1 = Make.New("a"),
                selection2 = Make.New("a"));

            var identity = Rules.ProductToExponentRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(Make.Power(Make.New("a"), Make.New(2)), identity.Suggestion);
        }

        [TestMethod]
        public void Rules_ExponentProduct()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;

            // a^2 => a*a
            parent = Make.Power(
                selection1 = Make.New("a"),
                selection2 = Make.New(2));

            var identity = Rules.ExponentToProductRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(Make.Multiply(Make.New("a"), Make.New("a")), identity.Suggestion);
        }

        [TestMethod]
        public void Rules_NumericVariadicRule()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;
            Identity identity;

            // 2+2 => 4
            parent = Make.Add(
                selection1 = Make.New(2),
                selection2 = Make.New(2));

            identity = Rules.NumericVariadicRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(Make.New(4), identity.Suggestion);

            // 3*3 => 9
            parent = Make.Multiply(
                selection1 = Make.New(3),
                selection2 = Make.New(3));

            identity = Rules.NumericVariadicRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(Make.New(9), identity.Suggestion);
        }

        [TestMethod]
        public void Rules_NumericBinaryRule()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;
            Identity identity;

            // 2-2 => 0
            parent = Make.Subtract(
                selection1 = Make.New(2),
                selection2 = Make.New(2));

            identity = Rules.NumericBinaryRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(Make.New(0), identity.Suggestion);

            // 3/3 => 1
            /*parent = ExpressionTests.Divide(
                selection1 = ExpressionTests.New(3),
                selection2 = ExpressionTests.New(3));

            identity = Rules.NumericBinaryRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(ExpressionTests.New(1), identity.Suggestion);*/

            // 3^3 => 27
            parent = Make.Power(
                selection1 = Make.New(3),
                selection2 = Make.New(3));

            identity = Rules.NumericBinaryRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(Make.New(27), identity.Suggestion);
        }

        [TestMethod]
        public void Rules_CommonPowerParenthesisRule()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;
            Identity identity;

            // 3^5*2^5 => (3*2)^5
            parent = Make.Multiply(
                Make.Power(
                    Make.New(3),
                    selection1 = Make.New(5)),
                Make.Power(
                    Make.New(2),
                    selection2 = Make.New(5)));

            identity = Rules.CommonPowerParenthesisRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(Make.Power(Make.Delimiter(Make.Multiply(Make.New(3), Make.New(2))), Make.New(5)), identity.Suggestion);

            // 3^5+2^5 => NULL
            parent = Make.Add(
                Make.Power(
                    Make.New(3),
                    selection1 = Make.New(5)),
                Make.Power(
                    Make.New(2),
                    selection2 = Make.New(5)));

            identity = Rules.CommonPowerParenthesisRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNull(identity);
        }

        [TestMethod]
        public void Rules_ReverseCommonPowerParenthesisRule()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;
            Identity identity;

            // (3*2)^5 => 3^5*2^5
            parent = Make.Power(
                selection1 = Make.Delimiter(
                    Make.Multiply(
                        Make.New(3),
                        Make.New(2))),
                selection2 = Make.New(5));

            identity = Rules.ReverseCommonPowerParenthesisRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(Make.Multiply(
                Make.Power(
                    Make.New(3),
                    Make.New(5)),
                Make.Power(
                    Make.New(2),
                   Make.New(5))), identity.Suggestion);
        }

        [TestMethod]
        public void Rules_VariableWithNegativeExponent()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;
            Identity identity;

            // x^-2 = 1/x^2
            parent = Make.Power(
                selection1 = Make.New("x"),
                selection2 = Make.Minus(Make.New(2)));

            identity = Rules.VariableWithNegativeExponent(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(Make.Divide(Make.New(1), Make.Power(Make.New("x"), Make.New(2))), identity.Suggestion);
        }

        [TestMethod]
        public void Rules_ReverseVariableWithNegativeExponent()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase selection3;
            ExpressionBase parent;
            Identity identity;

            // x^-2 = 1/x^2
            parent = selection1 = Make.Divide(
                selection2 = Make.New(1),
                Make.Power(
                    selection3 = Make.New("x"),
                    Make.New(2)));

            identity = Rules.ReverseVariableWithNegativeExponent(parent, new List<ExpressionBase>() { selection1, selection2, selection3 });
            Assert.IsNotNull(identity);
            Assert.AreEqual(Make.Power(Make.New("x"), Make.Minus(Make.New(2))), identity.Suggestion);
        }

        [TestMethod]
        public void Rules_FactorizationRule()
        {
            ExpressionBase selection1;
            ExpressionBase parent;
            Identity identity;

            // 2/10 = 2/5*2
            parent = selection1 = Make.New(10);

            identity = Rules.FactorizationRule(parent, new List<ExpressionBase>() { selection1 });
            Assert.IsNotNull(identity);

        }
    } // sad
} 
