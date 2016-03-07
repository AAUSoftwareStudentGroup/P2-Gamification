using System;
using Bridge;
using Bridge.Html5;
using Bridge.QUnit;
using ThreeOneSevenBee.Framework.Euclidean;
using ThreeOneSevenBee.Framework.Expressions;

namespace ThreeOneSevenBee.Framework.Tests
{
    public class App
    {
        [Ready]
        public static void Main()
        {
            QUnit.Module("Euclidean");
            QUnit.Test("Constructor Vector2", (assert) =>
            {
                assert.Expect(1);

                var vectorZero = new Vector2(0, 0);
                assert.Ok(Math.Abs(vectorZero.X) < double.Epsilon && Math.Abs(vectorZero.Y) < double.Epsilon, "Vector2 Zero");
            });

            QUnit.Module("ExpressionSerializer");
            QUnit.Test("Construct", (assert) =>
            {
                assert.Expect(1);

                var parser = new ExpressionSerializer();
                assert.Ok(parser != null, "Parser constructed");
            });
            QUnit.Test("Serialize", (assert) =>
            {
                assert.Expect(5);

                var parser = new ExpressionSerializer();
                Expression exp;

                exp = new OperatorExpression(
                    new NumericExpression(1),
                    new NumericExpression(1),
                    OperatorType.Add);
                assert.Equal(parser.Serialize(exp), "1+1", "1+1");

                exp = new OperatorExpression(
                    new NumericExpression(1),
                    new NumericExpression(1),
                    OperatorType.Divide);
                assert.Equal(parser.Serialize(exp), "1/1", "1/1");

                exp = new OperatorExpression(
                    new NumericExpression(1),
                    new NumericExpression(1),
                    OperatorType.Multiply);
                assert.Equal(parser.Serialize(exp), "1*1", "1*1");

                exp = new OperatorExpression(
                    new NumericExpression(1),
                    new NumericExpression(1),
                    OperatorType.Power);
                assert.Equal(parser.Serialize(exp), "1^1", "1^1");

                exp = new OperatorExpression(
                    new NumericExpression(1),
                    new NumericExpression(1),
                    OperatorType.Subtract);
                assert.Equal(parser.Serialize(exp), "1-1", "1-1");
            });
            QUnit.Test("Deserialize", (assert =>
            {
                assert.Expect(6);

                var parser = new ExpressionSerializer();
                var exp = parser.Deserialize("pi+1");

                assert.Equal(exp.GetClassName(), "ThreeOneSevenBee.Framework.Expressions.OperatorExpression", "Root is Operator");
                assert.Equal(exp.As<OperatorExpression>().Type, OperatorType.Add, "Root operator is add");
                assert.Equal(exp.As<OperatorExpression>().Left.GetClassName(), "ThreeOneSevenBee.Framework.Expressions.ConstantExpression", "Root.Left is Constant");
                assert.Equal(exp.As<OperatorExpression>().Left.As<ConstantExpression>().Value, "Pi", "Root.Left is Pi");
                assert.Equal(exp.As<OperatorExpression>().Right.GetClassName(), "ThreeOneSevenBee.Framework.Expressions.NumericExpression", "Root.Right is Numeric");
                assert.Equal(exp.As<OperatorExpression>().Right.As<NumericExpression>().Value, "1", "Root.Right is 1");
            }));
            QUnit.Test("Calculate", (assert) =>
            {
                assert.Expect(15);

                var parser = new ExpressionSerializer();
                assert.Equal(parser.Deserialize("1+1").Calculate(), 2, "1 + 1 = 2");
                assert.Equal(parser.Deserialize("--5").Calculate(), 5, "--5 = 5");
                assert.Equal(parser.Deserialize("sqrt(-2^2)").Calculate(), 2, "sqrt(-2^2) = 2");
                assert.Equal(parser.Deserialize("sqrt(sqrt(16))").Calculate(), 2, "sqrt(sqrt(16)) = 2");
                assert.Equal(parser.Deserialize("-pi").Calculate(), -Math.PI, "-pi = " + -Math.PI);
                assert.Ok(double.IsNaN(parser.Deserialize("sqrt(-1)").Calculate().Value), "sqrt(-1) = NaN");
                assert.Equal(parser.Deserialize("pi^2").Calculate(), Math.Pow(Math.PI, 2), "pi^2 = " + Math.Pow(Math.PI, 2));
                assert.Equal(parser.Deserialize("2^sqrt(4)").Calculate(), 4, "2^sqrt(4) = 4");
                assert.Equal(parser.Deserialize("sqrt(4)*sqrt(9)*sqrt(16)").Calculate(), 24, "sqrt(4) * sqrt(9) * sqrt(16) = 24");
                assert.Equal(parser.Deserialize("-(2+2)").Calculate(), -4, "-(2 + 2) = -4");
                assert.Equal(parser.Deserialize("(2+2)*(3+3)").Calculate(), 24, "(2 + 2) * (3 + 3) = 24");
                assert.Equal(parser.Deserialize("-(2+2)*-(3+3)").Calculate(), 24, "-(2 + 2) * -(3 + 3) = 24");
                assert.Equal(parser.Deserialize("2*sqrt(4)*5").Calculate(), 20, "2 * sqrt(4) * 5 = 20");
                assert.Ok(double.IsInfinity(parser.Deserialize("1/0").Calculate().Value), "1/0 = Infinity");
                assert.Ok(double.IsNaN(parser.Deserialize("0/0").Calculate().Value), "0/0 = NaN");
            });
            QUnit.Test("Deserialize <-> Serialize", (assert) =>
            {
                assert.Expect(1);

                var parser = new ExpressionSerializer();
                var exp = parser.Deserialize("1   +  1");

                assert.Equal(parser.Serialize(exp), "1+1", "'1   +  1' = '1+1'");
            });
        }
    }
}