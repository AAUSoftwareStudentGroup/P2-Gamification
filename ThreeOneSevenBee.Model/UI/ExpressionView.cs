#if BRIDGE
using Bridge.Html5;
using Bridge;
#else
using System;
#endif
using ThreeOneSevenBee.Model.UI;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.Expressions;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.UI
{
    public class ExpressionView : CompositeView
    {
        public static View Build(ExpressionBase expression, ExpressionModel model)
        {
            if(expression is NumericExpression || expression is VariableExpression)
            {
                return new ButtonView(expression.ToString(), () => model.Select(expression))
                {
                    Width = 20,
                    Height = 20,
                    Selected = model.SelectionIndex(expression) != -1
                };
            }
            BinaryOperatorExpression operatorExpression;
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
            {
                View left = Build(operatorExpression.Left, model);
                View operatorSign = new ButtonView("+", null) { X = left.Width, Width = 20, Height = 20 };
                View right = Build(operatorExpression.Right, model);
                right.X = left.Width + operatorSign.Width;
                return new CompositeView(right.X + right.Width, System.Math.Max(System.Math.Max(left.Height, operatorSign.Height), right.Height)) { left, operatorSign, right };
            }
            return null;
        }

        public ExpressionView(ExpressionModel model, double width, double height) : base(width, height)
        {
            View view = Build(model.Expression, model);
            Children.Add(Build(model.Expression, model));
            model.OnChanged += (m) => 
            {
                Children.Clear();
                Children.Add(Build(m.Expression, m));
            };
        }


    }
}
