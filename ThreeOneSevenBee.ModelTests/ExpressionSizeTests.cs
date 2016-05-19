using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Expression;

namespace ThreeOneSevenBee.ModelTests
{

    [TestClass]
    public class ExpressionSizeTests
    {
        public ExpressionBase expr(string expression)
        {
            ExpressionSerializer serializer = new ExpressionSerializer();
            return serializer.Deserialize(expression);
        }

        [TestMethod]
        public void ExpressionSizeTest()
        {
            ExpressionModel model = new ExpressionModel("a+a");
            ExpressionModel model2 = new ExpressionModel("2*a");
            ExpressionModel model3 = new ExpressionModel("a*a");
            ExpressionModel model4 = new ExpressionModel("a^2");
            ExpressionModel model5 = new ExpressionModel("a*a*a");
            ExpressionModel model6 = new ExpressionModel("a^2*a");
            ExpressionModel model7 = new ExpressionModel("a*b+a*c");
            ExpressionModel model8 = new ExpressionModel("a*(b+c)");
            ExpressionModel model9 = new ExpressionModel("(a)");
            ExpressionModel model10 = new ExpressionModel("a");
            ExpressionModel model11 = new ExpressionModel("--a");
            ExpressionModel model12 = new ExpressionModel("1*{a/b}");
            ExpressionModel model13 = new ExpressionModel("a/b");
            ExpressionModel model14 = new ExpressionModel("a^2*b^2");
            ExpressionModel model15 = new ExpressionModel("(a*b)^2");
            ExpressionModel model16 = new ExpressionModel("1*a");
            ExpressionModel model17 = new ExpressionModel("sqrt{4}");
            ExpressionModel model18 = new ExpressionModel("2");
            ExpressionModel model19 = new ExpressionModel("a/a");
            ExpressionModel model20 = new ExpressionModel("1");
            ExpressionModel model21 = new ExpressionModel("2+2+2+2+2");
            ExpressionModel model22 = new ExpressionModel("2*5");

            // a + a > 2 * a
            Assert.IsTrue(expr("a+a").Size > expr("2*a").Size, "failed: a + a > 2 * a");
            // a * a > a^2
            Assert.IsTrue(expr("a*a").Size > expr("a^2").Size, "failed: a * a > a^2");
            // a * a * a > a^2 * a
            Assert.IsTrue(expr("a*a*a").Size > expr("a^2*a").Size, "failed: a * a * a > a^2 * a");
            // a * b + a * c > a * (b + c)
            Assert.IsTrue(expr("a*b+a*c").Size > expr("a*(b+c)").Size, "failed: a * b + a * c > a * (b + c)");
            // (a) > a
            Assert.IsTrue(expr("(a)").Size > expr("a").Size, "failed: (a) > a ");
            // --a > a 
            Assert.IsTrue(expr("--a").Size > expr("a").Size, "failed: --a > a ");
            // 1 * {a / b} > a / b
            Assert.IsTrue(expr("1*{a/b}").Size > expr("a/a").Size, "failed: 1 * {a / b} > a / b");
            // a^2 * b^2 > (a * b)^2
            Assert.IsTrue(expr("a^2*b^2").Size > expr("(a*b)^2").Size, "failed: a^2 * b^2 > (a * b)^2");
            // 1*a > a
            Assert.IsTrue(expr("1*a").Size > expr("a").Size, "failed: 1*a > a");
            // sqrt{4} > 2
            Assert.IsTrue(expr("sqrt{4}").Size > expr("2").Size, "failed: sqrt{4} > 2");
            // a / a > 1
            Assert.IsTrue(expr("a/a").Size > expr("1").Size, "failed: a / a > 1");
            // 2+2+2+2+2 > 2 * 5
            Assert.IsTrue(expr("2+2+2+2+2").Size > expr("2*5").Size, "failed:  2+2+2+2+2 > 2 * 5");

        }
    }
}
