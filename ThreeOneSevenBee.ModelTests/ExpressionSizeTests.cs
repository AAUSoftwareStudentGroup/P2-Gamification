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
            ExpressionModel model = new ExpressionModel("3*a+3*b-1*a");
            ExpressionModel model2 = new ExpressionModel("2 ^ 2 ^ 2 * 3 * 5 + sqrt(4)");
            ExpressionModel model3 = new ExpressionModel("2");
            ExpressionModel model4 = new ExpressionModel("2/4");
            ExpressionModel model5 = new ExpressionModel("----1");
            ExpressionModel model6 = new ExpressionModel("a^{3+2}");
            ExpressionModel model7 = new ExpressionModel("a^2*a");
            ExpressionModel model8 = new ExpressionModel("a*a*a");
            ExpressionModel model9 = new ExpressionModel("a + a");
            ExpressionModel model10 = new ExpressionModel("2a");
            ExpressionModel model11 = new ExpressionModel("(a)");
            ExpressionModel model12 = new ExpressionModel("a");
            ExpressionModel model13 = new ExpressionModel("a * b + a * c");
            ExpressionModel model14 = new ExpressionModel("a*(b+c)");
            ExpressionModel model15 = new ExpressionModel("sqrt(4)");
            ExpressionModel model16 = new ExpressionModel("2");
            ExpressionModel model17 = new ExpressionModel("--a");
            ExpressionModel model18 = new ExpressionModel("a");
            ExpressionModel model19 = new ExpressionModel("1/2");
            ExpressionModel model20 = new ExpressionModel("2/4");
        

            Assert.AreEqual(model.Expression.Size, 36, "Is True");
            Assert.AreEqual(model2.Expression.Size, 28, "Is True");
            Assert.AreEqual(model3.Expression.Size, 1, "Is True");
            Assert.AreEqual(model4.Expression.Size, 5, "Is True");
            Assert.AreEqual(model5.Expression.Size, 13, "Is True");
            Assert.AreEqual(model6.Expression.Size, 10, "Is True");
            Assert.AreEqual(model7.Expression.Size, 11, "Is True");
  
            // a^2*a < a * a * a
            Assert.IsTrue(model7.Expression.Size < model8.Expression.Size, "Is True");
            // a + a > 2a
            Assert.IsTrue(model9.Expression.Size > model10.Expression.Size);
            // (a) > a
            Assert.IsTrue(model11.Expression.Size > model12.Expression.Size);
            // a * b + a * c > a * (b+c)
            Assert.IsTrue(model13.Expression.Size > model14.Expression.Size);
            // sqrt(4) > 2 
            Assert.IsTrue(model15.Expression.Size > model16.Expression.Size);
            // --a > a 
            Assert.IsTrue(model17.Expression.Size > model18.Expression.Size);
            // 1/2 < 2/4
            //Assert.IsTrue(model19.Expression.Size < model20.Expression.Size);










        }
    }
}
