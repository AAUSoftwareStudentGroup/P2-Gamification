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

            Assert.IsInstanceOfType(exp, typeof(VariadicOperatorExpression), "Root is Operator");
            Assert.AreEqual(OperatorType.Add, (exp as VariadicOperatorExpression).Type, "Root operator is add");
            Assert.IsInstanceOfType((exp as VariadicOperatorExpression)[0], typeof(ConstantExpression), "Root.Left is Constant");
            Assert.AreEqual("Pi", ((exp as VariadicOperatorExpression)[0] as ConstantExpression).Value, "Root.Left is Pi");
            Assert.IsInstanceOfType((exp as VariadicOperatorExpression)[1], typeof(NumericExpression), "Root.Right is Numeric");
            Assert.AreEqual("1", ((exp as VariadicOperatorExpression)[1] as NumericExpression).Value, "Root.Right is 1");
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

        private ExpressionBase New(int value)
        {
            return new NumericExpression(value);
        }

        private ExpressionBase New(string value)
        {
            return new VariableExpression(value);
        }

        private ExpressionBase New(ConstantType type)
        {
            return new ConstantExpression(type);
        }

        private ExpressionBase Minus(ExpressionBase expression)
        {
            return new UnaryMinusExpression(expression);
        }

        private ExpressionBase Add(ExpressionBase left, ExpressionBase right)
        {
            return new BinaryOperatorExpression(left, right, OperatorType.Add);
        }

        private ExpressionBase Add(ExpressionBase first, ExpressionBase second, params ExpressionBase[] expressions)
        {
            return new VariadicOperatorExpression(OperatorType.Add, first, second, expressions);
        }

        private ExpressionBase Divide(ExpressionBase left, ExpressionBase right)
        {
            return new BinaryOperatorExpression(left, right, OperatorType.Divide);
        }

        private ExpressionBase Multiply(ExpressionBase left, ExpressionBase right)
        {
            return new BinaryOperatorExpression(left, right, OperatorType.Multiply);
        }

        private ExpressionBase Multiply(ExpressionBase first, ExpressionBase second, params ExpressionBase[] expressions)
        {
            return new VariadicOperatorExpression(OperatorType.Multiply, first, second, expressions);
        }

        private ExpressionBase Power(ExpressionBase left, ExpressionBase right)
        {
            return new BinaryOperatorExpression(left, right, OperatorType.Power);
        }

        private ExpressionBase Subtract(ExpressionBase left, ExpressionBase right)
        {
            return new BinaryOperatorExpression(left, right, OperatorType.Subtract);
        }

        private ExpressionBase Sqrt(ExpressionBase expression)
        {
            return new FunctionExpression(expression, "sqrt");
        }

        [TestMethod]
        public void ExpressionEquals()
        {
            // numeric
            Assert.IsTrue(New(1) == New(1), "1 == 1");
            Assert.IsTrue(New(1) != New(2), "1 != 2");

            // variable
            Assert.IsTrue(New("a") == New("a"), "a == a");
            Assert.IsTrue(New("a") != New("b"), "a != b");

            // constant
            Assert.IsTrue(New(ConstantType.Pi) == New(ConstantType.Pi), "Pi == Pi");

            // unary minus
            Assert.IsTrue(Minus(New(1)) == Minus(New(1)), "-1 == -1");
            Assert.IsTrue(Minus(New("a")) == Minus(New("a")), "-a == -a");
            Assert.IsTrue(Minus(New(1)) != Minus(New("a")), "-1 != -a");
            Assert.IsTrue(Minus(New("a")) != Minus(New(1)), "-a != -1");

            // binary
            // - add
            Assert.IsTrue(Add(New(1), New("a")) == Add(New(1), New("a")), "1+a == 1+a");
            Assert.IsTrue(Add(New(1), New("a")) == Add(New("a"), New(1)), "1+a == a+1");
            // - mult
            Assert.IsTrue(Multiply(New(1), New("a")) == Multiply(New(1), New("a")), "1*a == 1*a");
            Assert.IsTrue(Multiply(New(1), New("a")) == Multiply(New("a"), New(1)), "1*a == a*1");
            // - sub
            Assert.IsTrue(Subtract(New(1), New("a")) == Subtract(New(1), New("a")), "1-a == 1-a");
            Assert.IsTrue(Subtract(New(1), New("a")) != Subtract(New("a"), New(1)), "1-a != a-1");
            // - div
            Assert.IsTrue(Divide(New(1), New("a")) == Divide(New(1), New("a")), "1/a == 1/a");
            Assert.IsTrue(Divide(New(1), New("a")) != Divide(New("a"), New(1)), "1/a != a/1");
            // - pow
            Assert.IsTrue(Power(New(1), New("a")) == Power(New(1), New("a")), "1^a == 1^a");
            Assert.IsTrue(Power(New(1), New("a")) != Power(New("a"), New(1)), "1 ^ a != a ^ 1");

            // variadic
            Assert.IsTrue(Multiply(New("a"), New("b"), New("c")) == Multiply(New("c"), New("a"), New("b")), "a*b*c == c*a*b");
            Assert.IsTrue(Multiply(New("a"), Multiply(New("b"), Multiply(New("c"), New("d")))) == Multiply(New("a"), Multiply(New("b"), Multiply(New("d"), New("c")))), "a*(b*(c*d)) == a*(b*(d*c))");
            Assert.IsFalse(Multiply(New("a"), Multiply(New("b"), Multiply(New("c"), New("d")))) == Multiply(New("a"), New("b"), New("c"), New("d")), "a*(b*(c*d)) != a*b*c*d");
            Assert.IsTrue(Multiply(New(ConstantType.Pi), New(1)) == Multiply(New(1), New(ConstantType.Pi)), "pi*1 == 1*pi");

            // function
            Assert.IsTrue(Sqrt(New("a")) == Sqrt(New("a")));
        }
    }
}
