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
            Assert.IsTrue(model.Expression.Size > model2.Expression.Size, "Is True");
            // a * a > a^2
            Assert.IsTrue(model3.Expression.Size > model4.Expression.Size);
            // a * a * a > a^2 * a
            Assert.IsTrue(model5.Expression.Size > model6.Expression.Size);
            // a * b + a * c > a * (b + c)
            Assert.IsTrue(model7.Expression.Size > model8.Expression.Size);
            // (a) > a
            Assert.IsTrue(model9.Expression.Size > model10.Expression.Size);
            // --a > a 
            Assert.IsTrue(model11.Expression.Size > model10.Expression.Size);
            // 1 * {a / b} > a / b
            Assert.IsTrue(model12.Expression.Size > model13.Expression.Size);
            // a^2 * b^2 > (a * b)^2
            Assert.IsTrue(model14.Expression.Size > model15.Expression.Size);
            // 1*a > a
            Assert.IsTrue(model16.Expression.Size > model10.Expression.Size);
            // sqrt{4} > 2
            Assert.IsTrue(model17.Expression.Size > model18.Expression.Size);
            // a / a > 1
            Assert.IsTrue(model19.Expression.Size > model20.Expression.Size);
            // 2+2+2+2+2 > 2 * 5
            Assert.IsTrue(model21.Expression.Size > model22.Expression.Size);

        }
    }
}
