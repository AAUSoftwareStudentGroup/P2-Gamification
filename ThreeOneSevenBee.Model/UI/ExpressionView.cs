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
    public class ExpressionView : FrameView
    {
        public static double NUMVAR_SIZE = 20;
        public static View Build(ExpressionBase expression, ExpressionModel model)
        {
            UnaryMinusExpression minusExpression = expression as UnaryMinusExpression;
            if(minusExpression != null)
            {
                View view = Build(minusExpression.Expression, model);
                View operatorView = new OperatorView(minusExpression.Type);
                operatorView.Width = NUMVAR_SIZE;
                operatorView.Height = NUMVAR_SIZE;
                operatorView.Baseline = NUMVAR_SIZE / 2;
                view.X = operatorView.Width;
                operatorView.Y = view.Baseline - operatorView.Baseline;
                View minusView = new CompositeView(operatorView.Width + view.Width, view.Height) { operatorView, view };
                minusView.Baseline = view.Baseline;
                return minusView;
            }
            BinaryOperatorExpression operatorExpression = expression as BinaryOperatorExpression;
            if (operatorExpression != null)
            {
                View left = Build(operatorExpression.Left, model);
                View right = Build(operatorExpression.Right, model);
                View operatorView = new OperatorView(operatorExpression.Type);
                switch (operatorExpression.Type)
                {
                    case OperatorType.Divide:
                        double width = System.Math.Max(left.Width, right.Width) + NUMVAR_SIZE;
                        operatorView.Width = width;
                        operatorView.Height = NUMVAR_SIZE;
                        operatorView.Y = left.Height;
                        operatorView.Baseline = NUMVAR_SIZE / 2;
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
                    case OperatorType.Subtract:
                        double baseline = System.Math.Max(operatorView.Baseline, System.Math.Max(left.Baseline, right.Baseline));
                        operatorView.X = left.Width;
                        operatorView.Width = NUMVAR_SIZE;
                        operatorView.Height = NUMVAR_SIZE;
                        operatorView.Baseline = NUMVAR_SIZE / 2;
                        right.X = left.Width + operatorView.Width;
                        left.Y = baseline - left.Baseline;
                        operatorView.Y = baseline - operatorView.Baseline;
                        right.Y = baseline - right.Baseline;
                        double height = System.Math.Max(left.Y + left.Height,
                            System.Math.Max(operatorView.Y + operatorView.Height, right.Y + right.Height));
                        CompositeView subtraction = new CompositeView(right.X + right.Width, height)
                        {
                            left,
                            operatorView,
                            right
                        };
                        subtraction.Baseline = baseline;
                        return subtraction;
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
                        View operatorView = new OperatorView(variadicExpression.Type);
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
                return new CompositeView(offsetX, height) { Children = views, Baseline = maxBaseline };
            }
            DelimiterExpression delimiterExpression = expression as DelimiterExpression;
            if(delimiterExpression != null)
            {
                View view = Build(delimiterExpression.Expression, model);
                view.X = NUMVAR_SIZE / 2;
                View compositeView = new CompositeView(view.Width + NUMVAR_SIZE, view.Height)
                {
                    new ParenthesisView(ParenthesisType.Left) { Width = NUMVAR_SIZE / 2, Height = view.Height },
                    view,
                    new ParenthesisView(ParenthesisType.Right) { X = view.Width + NUMVAR_SIZE / 2, Width = NUMVAR_SIZE / 2, Height = view.Height },
                };
                return compositeView;
            }
            return new ButtonView(expression.ToString(), () => model.Select(expression))
            {
                Width = NUMVAR_SIZE,
                Height = NUMVAR_SIZE,
                Baseline = NUMVAR_SIZE / 2,
                BackgroundColor = model.SelectionIndex(expression) != -1 ? "#cccccc" : "transparent"
            };

        }

        public ExpressionView(ExpressionModel model, double width, double height) : base(width, height, Build(model.Expression, model), 2)
        {
            model.OnChanged += (m) =>
            {
                setContent(Build(m.Expression, m));
            };
        }
    }
}
