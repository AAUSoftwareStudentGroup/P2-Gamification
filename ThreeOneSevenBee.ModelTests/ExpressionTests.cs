using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.Expressions;

namespace ThreeOneSevenBee.ModelTests
{
    [TestClass]
    public class ExpressionTests
    {
        [TestMethod]
        public void ExpressionSerializerConstruct()
        {
            var parser = new ExpressionSerializer();
            Assert.IsNotNull(parser, "Parser constructed");
        }

        [TestMethod]
        public void ExpressionSerializerSerialize()
        {
            var parser = new ExpressionSerializer();
            ExpressionBase exp;

            exp = new BinaryOperatorExpression(
                new NumericExpression(1),
                new NumericExpression(1),
                OperatorType.Add);
            Assert.AreEqual("1+1", parser.Serialize(exp), "1+1");

            exp = new BinaryOperatorExpression(
                new NumericExpression(1),
                new NumericExpression(1),
                OperatorType.Divide);
            Assert.AreEqual("1/1", parser.Serialize(exp), "1/1");

            exp = new BinaryOperatorExpression(
                new NumericExpression(1),
                new NumericExpression(1),
                OperatorType.Multiply);
            Assert.AreEqual("1*1", parser.Serialize(exp), "1*1");

            exp = new BinaryOperatorExpression(
                new NumericExpression(1),
                new NumericExpression(1),
                OperatorType.Power);
            Assert.AreEqual("1^1", parser.Serialize(exp), "1^1");

            exp = new BinaryOperatorExpression(
                new NumericExpression(1),
                new NumericExpression(1),
                OperatorType.Subtract);
            Assert.AreEqual("1-1", parser.Serialize(exp), "1-1");
        }

        [TestMethod]
        public void ExpressionSerializerDeserialize()
        {
            var parser = new ExpressionSerializer();
            var exp = parser.Deserialize("pi+1");

            Assert.IsInstanceOfType(exp, typeof(BinaryOperatorExpression), "Root is Operator");
            Assert.AreEqual(OperatorType.Add, (exp as BinaryOperatorExpression).Type, "Root operator is add");
            Assert.IsInstanceOfType((exp as BinaryOperatorExpression).Left, typeof(ConstantExpression), "Root.Left is Constant");
            Assert.AreEqual("Pi", ((exp as BinaryOperatorExpression).Left as ConstantExpression).Value, "Root.Left is Pi");
            Assert.IsInstanceOfType((exp as BinaryOperatorExpression).Right, typeof(NumericExpression), "Root.Right is Numeric");
            Assert.AreEqual("1", ((exp as BinaryOperatorExpression).Right as NumericExpression).Value, "Root.Right is 1");
        }

        [TestMethod]
        public void ExpressionSerializerCalculate()
        {
            var parser = new ExpressionSerializer();
            Assert.AreEqual(2, parser.Deserialize("1+1").Calculate(), "1 + 1 = 2");
            Assert.AreEqual(5, parser.Deserialize("--5").Calculate(), "--5 = 5");
            Assert.AreEqual(2, parser.Deserialize("sqrt(-2^2)").Calculate(), "sqrt(-2^2) = 2");
            Assert.AreEqual(2, parser.Deserialize("sqrt(sqrt(16))").Calculate(), "sqrt(sqrt(16)) = 2");
            Assert.AreEqual(-Math.PI, parser.Deserialize("-pi").Calculate(), "-pi = " + -Math.PI);
            Assert.IsTrue(double.IsNaN(parser.Deserialize("sqrt(-1)").Calculate().Value), "sqrt(-1) = NaN");
            Assert.AreEqual(Math.Pow(Math.PI, 2), parser.Deserialize("pi^2").Calculate(), "pi^2 = " + Math.Pow(Math.PI, 2));
            Assert.AreEqual(4, parser.Deserialize("2^sqrt(4)").Calculate(), "2^sqrt(4) = 4");
            Assert.AreEqual(24, parser.Deserialize("sqrt(4)*sqrt(9)*sqrt(16)").Calculate(), "sqrt(4) * sqrt(9) * sqrt(16) = 24");
            Assert.AreEqual(-4, parser.Deserialize("-(2+2)").Calculate(), "-(2 + 2) = -4");
            Assert.AreEqual(24, parser.Deserialize("(2+2)*(3+3)").Calculate(), "(2 + 2) * (3 + 3) = 24");
            Assert.AreEqual(24, parser.Deserialize("-(2+2)*-(3+3)").Calculate(), "-(2 + 2) * -(3 + 3) = 24");
            Assert.AreEqual(20, parser.Deserialize("2*sqrt(4)*5").Calculate(), "2 * sqrt(4) * 5 = 20");
            Assert.AreEqual(-2, parser.Deserialize("-sqrt(4)").Calculate(), "-sqrt(2) = -2");
            Assert.IsTrue(double.IsInfinity(parser.Deserialize("1/0").Calculate().Value), "1/0 = Infinity");
            Assert.IsTrue(double.IsNaN(parser.Deserialize("0/0").Calculate().Value), "0/0 = NaN");
        }

        [TestMethod]
        public void ExpressionSerializerDeserializeToSerialize()
        {
            var parser = new ExpressionSerializer();
            var exp = parser.Deserialize("1   +  1");

            Assert.AreEqual("1+1", parser.Serialize(exp), "'1   +  1' = '1+1'");
        }
    }
}
