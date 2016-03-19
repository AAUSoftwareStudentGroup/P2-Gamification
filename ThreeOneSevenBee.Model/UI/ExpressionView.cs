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
            
            BinaryOperatorExpression operatorExpression = expression as BinaryOperatorExpression;
            if (operatorExpression != null)
            {
                View left = Build(operatorExpression.Left, model);
                View right = Build(operatorExpression.Right, model);
                View operatorView = new OperatorButtonView(operatorExpression.Type, null);
                switch (operatorExpression.Type)
                {
                    case OperatorType.Divide:
                        double width = System.Math.Max(left.Width, right.Width);
                        operatorView.Width = width;
                        operatorView.Height = NUMVAR_SIZE;
                        operatorView.Y = left.Height;
                        right.Y = left.Height + operatorView.Height;
                        left.X = (width - left.Width) / 2;
                        right.X = (width - right.Width) / 2;
                        return new CompositeView(width, left.Height + right.Height)
                        {
                            left,
                            operatorView,
                            right
                        };
                    case OperatorType.Power:
                        right.X = left.Width;
                        left.Y = right.Height;
                        return new CompositeView(right.X + right.Width, left.Y + left.Height)
                        {
                            left,
                            right
                        };
                }
            }
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if(variadicExpression != null)
            {
                List<View> operands = new List<View>();
                double offsetX = 0;
                double height = 0;
                foreach(ExpressionBase expr in variadicExpression)
                {
                    View operand = Build(expr, model);
                    operand.X = offsetX;
                    height = System.Math.Max(height, operand.Height);
                    offsetX += operand.Width;
                    operands.Add(operand);
                }
                return new CompositeView(offsetX, height) { Children = operands };
            }
            return new ButtonView(expression.ToString(), () => model.Select(expression))
                {
                    Width = NUMVAR_SIZE,
                    Height = NUMVAR_SIZE,
                    Selected = model.SelectionIndex(expression) != -1
                };
            
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
