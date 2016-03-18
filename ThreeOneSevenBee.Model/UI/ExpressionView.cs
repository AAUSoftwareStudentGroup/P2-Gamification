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
        public static int NUMVAR_SIZE = 20;
        public static View Build(ExpressionBase expression, ExpressionModel model)
        {
            if(expression is NumericExpression || expression is VariableExpression)
            {
                return new ButtonView(expression.ToString(), () => model.Select(expression))
                {
                    Width = NUMVAR_SIZE,
                    Height = NUMVAR_SIZE,
                    Selected = model.SelectionIndex(expression) != -1
                };
            }
            BinaryOperatorExpression operatorExpression;
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
            {
                View left;
                View operatorSign;
                View right;
                double viewWidth;

                if (operatorExpression.Type == OperatorType.Divide)
                {
                    // Move right expression up
                    // Move left expression down
                    // Draw line in the middle with max width of left and right

                    left = Build(operatorExpression.Left, model);
                    right = Build(operatorExpression.Right, model);
                    operatorSign = new OperatorButtonView(operatorExpression.Type, null) { Width = System.Math.Max(left.Width, right.Width), Height = left.Height + right.Height };
                    left.Y -= left.Height / 2;
                    right.Y += right.Height / 2;
                    right.X = left.X + left.Width / 2 - right.Width / 2;
                    operatorSign.Y = right.Y-1;
                    viewWidth = operatorSign.Width;
                }
                else if (operatorExpression.Type == OperatorType.Power)
                {
                    // Move right expression up and make font smaller
                    left = Build(operatorExpression.Left, model);
                    operatorSign = new OperatorButtonView(operatorExpression.Type, null) { X = left.Width, Width = 0, Height = 0 };
                    right = Build(operatorExpression.Right, model);
                    right.Y -= right.Height / 2;
                    right.X += left.Width - NUMVAR_SIZE / 3;
                    viewWidth = left.Width + operatorSign.Width + right.Width - NUMVAR_SIZE / 3;
                }
                else
                {
                    left = Build(operatorExpression.Left, model);
                    operatorSign = new OperatorButtonView(operatorExpression.Type, null) { X = left.Width, Width = NUMVAR_SIZE, Height = NUMVAR_SIZE };
                    right = Build(operatorExpression.Right, model);
                    viewWidth = left.Width + operatorSign.Width + right.Width;
                    right.X += left.Width + operatorSign.Width;
                }
                Console.WriteLine("Gon make som binary views 'ere");
                return new CompositeView(viewWidth, System.Math.Max(System.Math.Max(left.Height, operatorSign.Height), right.Height)) { left, operatorSign, right };
            }
            VariadicOperatorExpression variadicExpression;
            if ((variadicExpression = expression as VariadicOperatorExpression) != null)
            {
                View var;
                View operatorSign;
                double viewWidth = 0;
                double viewHeight = 0;
                List<View> views = new List<View>();
                for (int i = 0; i < variadicExpression.Count; i++)
                {
                    var = Build(variadicExpression, model);
                    Console.WriteLine("Gon make som buttonviews 'ere");
                    operatorSign = new OperatorButtonView(variadicExpression.Type, null) { X = var.Width, Width = NUMVAR_SIZE, Height = NUMVAR_SIZE };
                    viewWidth += var.Width + operatorSign.Width;
                    viewHeight = System.Math.Max(viewHeight, System.Math.Max(var.Height, operatorSign.Height));
                    views.Add(var);
                    if (i != variadicExpression.Count - 1)
                        views.Add(operatorSign);
                }
                Console.WriteLine("Gon make som views 'ere");
                return new CompositeView(viewWidth, viewWidth) { views };
            }
            Console.WriteLine("FUCKSHIT");
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
