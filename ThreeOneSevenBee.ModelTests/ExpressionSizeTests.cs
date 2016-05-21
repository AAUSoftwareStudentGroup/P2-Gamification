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
