using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.ExpressionRules;
using System.Collections.Generic;
using ThreeOneSevenBee.Model.Expression.Expressions;

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

            var suggestion = Rules.ProductToExponentRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.Power(Make.New("a"), Make.New(2)), suggestion);
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

            var suggestion = Rules.ExponentToProductRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.Multiply(Make.New("a"), Make.New("a")), suggestion);
        }

        [TestMethod]
        public void Rules_NumericVariadicRule()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;
            ExpressionBase suggestion;

            // 2+2 => 4
            parent = Make.Add(
                selection1 = Make.New(2),
                selection2 = Make.New(2));

            suggestion = Rules.CalculateVariadicRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.New(4), suggestion);

            // 3*3 => 9
            parent = Make.Multiply(
                selection1 = Make.New(3),
                selection2 = Make.New(3));

            suggestion = Rules.CalculateVariadicRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.New(9), suggestion);
        }

        [TestMethod]
        public void Rules_CalculateBinaryRule()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;

            // 3^3 => 27
            parent = Make.Power(
                selection1 = Make.New(3),
                selection2 = Make.New(3));

            ExpressionBase suggestion = Rules.CalculateBinaryRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.New(27), suggestion);
        }

        [TestMethod]
        public void Rules_CommonPowerParenthesisRule()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;

            // 3^5*2^5 => (3*2)^5
            parent = Make.Multiply(
                Make.Power(
                    Make.New(3),
                    selection1 = Make.New(5)),
                Make.Power(
                    Make.New(2),
                    selection2 = Make.New(5)));

            var suggestion = Rules.CommonPowerParenthesisRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.Power(Make.Delimiter(Make.Multiply(Make.New(3), Make.New(2))), Make.New(5)), suggestion);

            // 3^5+2^5 => NULL
            parent = Make.Add(
                Make.Power(
                    Make.New(3),
                    selection1 = Make.New(5)),
                Make.Power(
                    Make.New(2),
                    selection2 = Make.New(5)));

            suggestion = Rules.CommonPowerParenthesisRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNull(suggestion);
        }

        [TestMethod]
        public void Rules_ReverseCommonPowerParenthesisRule()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;

            // (3*2)^5 => 3^5*2^5
            parent = Make.Power(
                selection1 = Make.Delimiter(
                    Make.Multiply(
                        Make.New(3),
                        Make.New(2))),
                selection2 = Make.New(5));

            var suggestion = Rules.ReverseCommonPowerParenthesisRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.Multiply(
                Make.Power(
                    Make.New(3),
                    Make.New(5)),
                Make.Power(
                    Make.New(2),
                   Make.New(5))), suggestion);
        }

        [TestMethod]
        public void Rules_VariableWithNegativeExponent()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase parent;

            // x^-2 = 1/x^2
            parent = Make.Power(
                selection1 = Make.New("x"),
                selection2 = Make.Minus(Make.New(2)));

            var suggestion = Rules.VariableWithNegativeExponent(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.Divide(Make.New(1), Make.Power(Make.New("x"), Make.New(2))), suggestion);
        }

        [TestMethod]
        public void Rules_ReverseVariableWithNegativeExponent()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase selection3;
            ExpressionBase parent;

            // x^-2 = 1/x^2
            parent = selection1 = Make.Divide(
                selection2 = Make.New(1),
                Make.Power(
                    selection3 = Make.New("x"),
                    Make.New(2)));

            var suggestion = Rules.ReverseVariableWithNegativeExponent(parent, new List<ExpressionBase>() { selection1, selection2, selection3 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.Power(Make.New("x"), Make.Minus(Make.New(2))), suggestion);
        }

        [TestMethod]
        public void Rules_FactorizationRule()
        {
            ExpressionBase selection;
            ExpressionBase parent;

            parent = selection = Make.New(10);

            var suggestion = Rules.FactorizationRule(parent, new List<ExpressionBase>() { selection });
            Assert.AreEqual(suggestion, Make.Multiply(Make.New(2), Make.New(5)));
        }

        [TestMethod]
        public void Rules_AddFractionsWithSameNumerators()
        {
            ExpressionBase selection1;
            ExpressionBase selection2;
            ExpressionBase selection3;
            ExpressionBase parent;

            // a/b + c/b = a+c/b
            parent = Make.Add(
            selection1 = Make.Divide(Make.New("a"), Make.New("b")),
            selection2 = Make.Divide(Make.New("c"), Make.New("b")));

            var suggestion = Rules.AddFractionWithCommonDenominatorRule(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.Divide(Make.Add(Make.New("a"), Make.New("c")), Make.New("b")), suggestion);

            // a/x - y/x + b/x = {a-y+b}/x
            parent = Make.Add(
                selection1 = Make.Divide(Make.New("a"), Make.New("x")),
                selection2 = Make.Divide(Make.Minus(Make.New("y")), Make.New("x")),
                selection3 = Make.Divide(Make.New("b"), Make.New("x")));

            suggestion = Rules.AddFractionWithCommonDenominatorRule(parent, new List<ExpressionBase>() { selection1, selection2, selection3 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.Divide(Make.Add(Make.New("a"), Make.Minus(Make.New("y")), Make.New("b")), Make.New("x")), suggestion);
        }

        [TestMethod]
        public void Rules_SplittingFractions()
        {
            ExpressionBase parent;
            ExpressionBase selection1;

            // {a+b}/c = a/c + b/c
            parent = selection1 = Make.Divide(
            Make.Add(Make.New("a"), Make.New("b")),
            Make.New("c"));

            var suggestion = Rules.SplittingFractions(parent, new List<ExpressionBase>() { selection1 });
            Assert.IsNotNull(suggestion);
            Assert.AreEqual(Make.Add(Make.Divide(Make.New("a"), Make.New("c")), Make.Divide(Make.New("b"), Make.New("c"))), suggestion);

            // {a-c-d+f}/x = a/x - c/x - d/x + f/x
            parent = selection1 = Make.Divide(Make.Add(Make.New("a"), Make.Minus(Make.New("c")),
                Make.Minus(Make.New("d")), Make.New("f")), Make.New("x"));

            suggestion = Rules.SplittingFractions(parent, new List<ExpressionBase>() { selection1 });

            // TODO: Nedenstående er: a/x + -c/x + -d/x + f/x + 3, denne virker
            // Den nedenunder er: a/x - c/x - d/x + f/x + 3, den virker ikke, det skal ændres i reglen
            //Husk at Bruge issue nummeret ved commit!

            Assert.AreEqual(Make.Add(Make.Divide(Make.New("a"), Make.New("x")), Make.Divide(Make.Minus(Make.New("c")), Make.New("x")), Make.Divide(Make.Minus(Make.New("d")), Make.New("x")), Make.Divide(Make.New("f"), Make.New("x"))), suggestion);

            //Assert.AreEqual(Make.Add(Make.Divide(Make.New("a"), Make.New("x")), Make.Minus(Make.Divide(Make.New("c"), Make.New("x"))), Make.Minus(Make.Divide(Make.New("d"), Make.New("x"))), Make.Divide(Make.New("f"), Make.New("x")), Make.New(3)), identity.Result);
        }

        [TestMethod]
        public void Rules_DivisionEqualsOneRule()
        {
            ExpressionBase selection1;
            ExpressionBase parent;


            parent = selection1 = Make.Divide(Make.New(15), Make.New(15));

            var suggestion = Rules.DivisionEqualsOneRule(parent, new List<ExpressionBase>() { selection1 });
            Assert.IsNotNull(suggestion);

            NumericExpression a = new NumericExpression(1);
            NumericExpression b = new NumericExpression(2);

            Assert.IsTrue(suggestion == a);
            Assert.IsFalse(suggestion == b);

        }

        [TestMethod]
        public void Rules_ProductParenthesis()
        {
            ExpressionBase selection1, selection2;
            ExpressionBase parent = Make.Add(
                Make.Multiply(
                    selection1 = Make.New("x"),
                    Make.New("y")
                ),
                Make.Multiply(
                    selection2 = Make.New("x"),
                    Make.New("z")
                )
            );
            selection1.Selected = true;
            selection2.Selected = true;
            ExpressionBase suggestion = Rules.ProductParenthesis(parent, new List<ExpressionBase>() { selection1, selection2 });
            Assert.AreEqual(new ExpressionSerializer().Deserialize("(y+z)*x"), suggestion, parent.ToString());
        }
    }
}