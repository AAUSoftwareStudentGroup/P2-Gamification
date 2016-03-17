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
            OperatorExpression operatorExpression;
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                View left;
                View operatorSign;
                View right;

                if (operatorExpression.Type == OperatorType.Divide)
                {
                    // Move right expression up
                    // Move left expression down
                    // Draw line in the middle with max width of left and right

                    left = Build(operatorExpression.Left, model);
                    operatorSign = new OperatorButtonView(operatorExpression.Type, null);
                    right = Build(operatorExpression.Right, model);
                    left.Y -= left.Height / 2;
                    right.Y += right.Height / 2;
                    right.X = left.X+left.Width/2-right.Width/2;
                }
                /*else if (operatorExpression.Type == OperatorType.Power)
                {
                    // Move right expression up

                }*/
                else
                {
                    left = Build(operatorExpression.Left, model);
                    operatorSign = new OperatorButtonView(operatorExpression.Type, null) { X = left.Width, Width = 20, Height = 20 };
                    right = Build(operatorExpression.Right, model);
                    right.X = operatorSign.Width + left.Width;
                }
                return new CompositeView(left.Width + operatorSign.Width + right.Width, System.Math.Max(System.Math.Max(left.Height, operatorSign.Height), right.Height)) { left, operatorSign, right };
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
