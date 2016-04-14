using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.Expressions;

namespace ThreeOneSevenBee.ModelTests
{
    public static class Make
    {
        public static ExpressionBase New(int value)
        {
            if (value < 0)
                return new UnaryMinusExpression(new NumericExpression(-value));
            return new NumericExpression(value);
        }

        public static ExpressionBase New(string value)
        {
            return new VariableExpression(value);
        }

        public static ExpressionBase New(ConstantType type)
        {
            return new ConstantExpression(type);
        }

        public static ExpressionBase Minus(ExpressionBase expression)
        {
            return new UnaryMinusExpression(expression);
        }

        public static ExpressionBase Add(ExpressionBase left, ExpressionBase right)
        {
            return new VariadicOperatorExpression(OperatorType.Add, left, right);
        }

        public static ExpressionBase Add(ExpressionBase first, ExpressionBase second, params ExpressionBase[] expressions)
        {
            return new VariadicOperatorExpression(OperatorType.Add, first, second, expressions);
        }

        public static ExpressionBase Divide(ExpressionBase left, ExpressionBase right)
        {
            return new BinaryOperatorExpression(left, right, OperatorType.Divide);
        }

        public static ExpressionBase Multiply(ExpressionBase left, ExpressionBase right)
        {
            return new VariadicOperatorExpression(OperatorType.Multiply, left, right);
        }

        public static ExpressionBase Multiply(ExpressionBase first, ExpressionBase second, params ExpressionBase[] expressions)
        {
            return new VariadicOperatorExpression(OperatorType.Multiply, first, second, expressions);
        }

        public static ExpressionBase Power(ExpressionBase left, ExpressionBase right)
        {
            return new BinaryOperatorExpression(left, right, OperatorType.Power);
        }

        public static ExpressionBase Subtract(ExpressionBase left, ExpressionBase right)
        {
            return new BinaryOperatorExpression(left, right, OperatorType.Subtract);
        }

        public static ExpressionBase Sqrt(ExpressionBase expression)
        {
            return new FunctionExpression(expression, "sqrt");
        }

        public static ExpressionBase Delimiter(ExpressionBase expression)
        {
            return new DelimiterExpression(expression);
        }
    }

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

        [TestMethod]
        public void ExpressionEquals()
        {
            // numeric
            Assert.IsTrue(Make.New(1) == Make.New(1), "1 == 1");
            Assert.IsTrue(Make.New(1) != Make.New(2), "1 != 2");

            // variable
            Assert.IsTrue(Make.New("a") == Make.New("a"), "a == a");
            Assert.IsTrue(Make.New("a") != Make.New("b"), "a != b");

            // constant
            Assert.IsTrue(Make.New(ConstantType.Pi) == Make.New(ConstantType.Pi), "Pi == Pi");

            // unary minus
            Assert.IsTrue(Make.Minus(Make.New(1)) == Make.Minus(Make.New(1)), "-1 == -1");
            Assert.IsTrue(Make.Minus(Make.New("a")) == Make.Minus(Make.New("a")), "-a == -a");
            Assert.IsTrue(Make.Minus(Make.New(1)) != Make.Minus(Make.New("a")), "-1 != -a");
            Assert.IsTrue(Make.Minus(Make.New("a")) != Make.Minus(Make.New(1)), "-a != -1");

            // binary
            // - add
            Assert.IsTrue(Make.Add(Make.New(1), Make.New("a")) == Make.Add(Make.New(1), Make.New("a")), "1+a == 1+a");
            Assert.IsTrue(Make.Add(Make.New(1), Make.New("a")) == Make.Add(Make.New("a"), Make.New(1)), "1+a == a+1");
            // - mult
            Assert.IsTrue(Make.Multiply(Make.New(1), Make.New("a")) == Make.Multiply(Make.New(1), Make.New("a")), "1*a == 1*a");
            Assert.IsTrue(Make.Multiply(Make.New(1), Make.New("a")) == Make.Multiply(Make.New("a"), Make.New(1)), "1*a == a*1");
            // - sub
            Assert.IsTrue(Make.Subtract(Make.New(1), Make.New("a")) == Make.Subtract(Make.New(1), Make.New("a")), "1-a == 1-a");
            Assert.IsTrue(Make.Subtract(Make.New(1), Make.New("a")) != Make.Subtract(Make.New("a"), Make.New(1)), "1-a != a-1");
            // - div
            Assert.IsTrue(Make.Divide(Make.New(1), Make.New("a")) == Make.Divide(Make.New(1), Make.New("a")), "1/a == 1/a");
            Assert.IsTrue(Make.Divide(Make.New(1), Make.New("a")) != Make.Divide(Make.New("a"), Make.New(1)), "1/a != a/1");
            // - pow
            Assert.IsTrue(Make.Power(Make.New(1), Make.New("a")) == Make.Power(Make.New(1), Make.New("a")), "1^a == 1^a");
            Assert.IsTrue(Make.Power(Make.New(1), Make.New("a")) != Make.Power(Make.New("a"), Make.New(1)), "1 ^ a != a ^ 1");

            // variadic
            Assert.IsTrue(Make.Multiply(Make.New("a"), Make.New("b"), Make.New("c")) == Make.Multiply(Make.New("c"), Make.New("a"), Make.New("b")), "a*b*c == c*a*b");
            Assert.IsTrue(Make.Multiply(Make.New("a"), Make.Multiply(Make.New("b"), Make.Multiply(Make.New("c"), Make.New("d")))) == Make.Multiply(Make.New("a"), Make.Multiply(Make.New("b"), Make.Multiply(Make.New("d"), Make.New("c")))), "a*(b*(c*d)) == a*(b*(d*c))");
            Assert.IsFalse(Make.Multiply(Make.New("a"), Make.Multiply(Make.New("b"), Make.Multiply(Make.New("c"), Make.New("d")))) == Make.Multiply(Make.New("a"), Make.New("b"), Make.New("c"), Make.New("d")), "a*(b*(c*d)) != a*b*c*d");
            Assert.IsTrue(Make.Multiply(Make.New(ConstantType.Pi), Make.New(1)) == Make.Multiply(Make.New(1), Make.New(ConstantType.Pi)), "pi*1 == 1*pi");

            // function
            Assert.IsTrue(Make.Sqrt(Make.New("a")) == Make.Sqrt(Make.New("a")));
        }

        [TestMethod]
        public void ExpressionStructureTest()
        {
            var a = Make.Add(Make.New("a"), Make.New("b"), Make.Add(Make.New("2"), Make.New("2")));
            var b = Make.Add(Make.New("a"), Make.New("b"), Make.New("2"), Make.New("2"));

            Assert.IsTrue(a == b, "a+b+(2+2) == a+b+2+2");
        }
    }
}
