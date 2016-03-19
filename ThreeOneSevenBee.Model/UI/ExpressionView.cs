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
                        double width = System.Math.Max(left.Width, right.Width) + NUMVAR_SIZE;
                        operatorView.Width = width;
                        operatorView.Height = NUMVAR_SIZE;
                        operatorView.Y = left.Height;
                        right.Y = left.Height + operatorView.Height;
                        left.X = (width - left.Width) / 2;
                        right.X = (width - right.Width) / 2;
                        CompositeView fraction = new CompositeView(width, left.Height + operatorView.Height + right.Height)
                        {
                            left,
                            operatorView,
                            right
                        };
                        fraction.Baseline = operatorView.Y + operatorView.Height / 2;
                        return fraction;
                    case OperatorType.Power:
                        right.X = left.Width;
                        left.Y = right.Height;
                        CompositeView exponent = new CompositeView(right.X + right.Width, left.Y + left.Height)
                        {
                            left,
                            right
                        };
                        exponent.Baseline = exponent.Height - left.Baseline;
                        return exponent;
                }
            }
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null)
            {
                List<View> views = new List<View>();
                double offsetX = 0;
                double height = 0;
                double maxBaseline = NUMVAR_SIZE / 2;
                foreach (ExpressionBase expr in variadicExpression)
                {
                    if (views.Count != 0)
                    {
                        View operatorView = new OperatorButtonView(variadicExpression.Type, null);
                        operatorView.X = offsetX;
                        operatorView.Width = NUMVAR_SIZE;
                        operatorView.Height = NUMVAR_SIZE;
                        operatorView.Baseline = NUMVAR_SIZE / 2;
                        views.Add(operatorView);
                        offsetX += operatorView.Width;
                    }
                    View operand = Build(expr, model);
                    maxBaseline = System.Math.Max(maxBaseline, operand.Baseline);
                    operand.X = offsetX;
                    offsetX += operand.Width;
                    views.Add(operand);
                }
                foreach (View view in views)
                {
                    view.Y = maxBaseline - view.Baseline;
                    height = System.Math.Max(height, view.Y + view.Height);
                }

                return new CompositeView(offsetX, height) { Children = views };
            }
            return new ButtonView(expression.ToString(), () => model.Select(expression))
            {
                Width = NUMVAR_SIZE,
                Height = NUMVAR_SIZE,
                Baseline = NUMVAR_SIZE / 2,
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
