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
        public View BuildView(ExpressionBase expression, ExpressionModel model)
        {
            UnaryMinusExpression minusExpression = expression as UnaryMinusExpression;
            if(minusExpression != null)
            {
                View view = BuildView(minusExpression.Expression, model);
                OperatorView operatorView = new OperatorView(minusExpression.Type);
                operatorView.Width = NUMVAR_SIZE / 2;
                operatorView.Height = NUMVAR_SIZE;
                operatorView.Baseline = NUMVAR_SIZE / 2;
                operatorView.OnClick = () => model.Select(minusExpression);
                
                operatorView.LineColor = expression.Selected ? new Color(39, 174, 97) : new Color(0, 0, 0);
                operatorView.LineWidth = NUMVAR_SIZE / 15;
                view.X = operatorView.Width;
                operatorView.Y = view.Baseline - operatorView.Baseline;
                View minusView = new CompositeView(operatorView.Width + view.Width, view.Height) { operatorView, view };
                minusView.Baseline = view.Baseline;
                return minusView;
            }
            BinaryOperatorExpression operatorExpression = expression as BinaryOperatorExpression;
            if (operatorExpression != null)
            {
                View left = BuildView(operatorExpression.Left, model);
                View right = BuildView(operatorExpression.Right, model);
                OperatorView operatorView = new OperatorView(operatorExpression.Type);
                switch (operatorExpression.Type)
                {
                    case OperatorType.Divide:
                        double width = System.Math.Max(left.Width, right.Width) + NUMVAR_SIZE;
                        operatorView.Width = width;
                        operatorView.Height = NUMVAR_SIZE / 1.5;
                        operatorView.Y = left.Height;
                        operatorView.Baseline = NUMVAR_SIZE / 2;
                        operatorView.OnClick = () => model.Select(expression);
                        operatorView.LineWidth = NUMVAR_SIZE / 12;
                        operatorView.LineColor = expression.Selected ? new Color(39, 174, 97) : new Color(0, 0, 0);
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
                        left.Y = right.Height - NUMVAR_SIZE / 2;
                        CompositeView exponent = new CompositeView(right.X + right.Width, left.Y + left.Height)
                        {
                            left,
                            right
                        };
                        exponent.Baseline = left.Y + left.Baseline;
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
                    View operand;
                    if (views.Count != 0)
                    {
                        UnaryMinusExpression minus = expr as UnaryMinusExpression;
                        OperatorView operatorView;
                        if (variadicExpression.Type == OperatorType.Add && minus != null)
                        {
                            operatorView = new OperatorView(OperatorType.Subtract);
                            operand = BuildView(minus.Expression, model);
                            operand.OnClick = () => model.Select(minus.Expression);
                        }
                        else
                        {
                            operatorView = new OperatorView(variadicExpression.Type);
                            operand = BuildView(expr, model);
                        }
                        operatorView.X = offsetX;
                        operatorView.Width = (variadicExpression.Type == OperatorType.Multiply ? 0.5 : 1.5)  * NUMVAR_SIZE;
                        operatorView.Height = NUMVAR_SIZE;
                        operatorView.Baseline = NUMVAR_SIZE / 2;
                        operatorView.LineWidth = NUMVAR_SIZE / (variadicExpression.Type == OperatorType.Multiply ? 25 : 15);
                        views.Add(operatorView);
                        offsetX += operatorView.Width;
                    }
                    else
                    {
                        operand = BuildView(expr, model);
                    }
                    
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
                View view = BuildView(delimiterExpression.Expression, model);
                view.X = view.Height / 3;
                view.Y = NUMVAR_SIZE / 8;
                ParenthesisView left = new ParenthesisView(ParenthesisType.Left) { OnClick = () => model.Select(expression), Width = view.Height / 3, Height = view.Height + NUMVAR_SIZE / 4 };
                ParenthesisView right = new ParenthesisView(ParenthesisType.Right) { OnClick = () => model.Select(expression), X = view.Width + view.Height / 3, Width = view.Height / 3, Height = view.Height + NUMVAR_SIZE / 4 };

                View compositeView = new CompositeView(view.Width + view.Height / 1.5, view.Height + NUMVAR_SIZE / 4)
                {
                    left,
                    view,
                    right
                };
                left.LineWidth = NUMVAR_SIZE / 15;
                right.LineWidth = NUMVAR_SIZE / 15;
                left.LineColor = expression.Selected ? new Color(40, 175, 100) : new Color(0, 0, 0);
                right.LineColor = expression.Selected ? new Color(40, 175, 100) : new Color(0, 0, 0);
                compositeView.Baseline = view.Y + view.Baseline;
                return compositeView;
            }
            FunctionExpression functionExpression = expression as FunctionExpression;
            if(functionExpression != null && functionExpression.Function == "sqrt")
            {
                View view = BuildView(functionExpression.Expression, model);
                SqrtView sqrtView = new SqrtView();
                sqrtView.OnClick = () => model.Select(expression);
                sqrtView.SignWidth = view.Height / 2;
                sqrtView.TopHeight = NUMVAR_SIZE / 2;
                sqrtView.LineWidth = NUMVAR_SIZE / 12;
                sqrtView.Width = view.Width + sqrtView.SignWidth + NUMVAR_SIZE / 4;
                sqrtView.LineColor = expression.Selected ? new Color(40, 175, 100) : new Color(0, 0, 0);
                sqrtView.Height = view.Height + sqrtView.TopHeight;
                view.X = sqrtView.SignWidth + NUMVAR_SIZE / 8;
                view.Y = sqrtView.TopHeight;
                View compositeView = new CompositeView(sqrtView.Width, sqrtView.Height)
                {
                    sqrtView,
                    view,
                };
                compositeView.Baseline = view.Baseline + sqrtView.TopHeight;
                return compositeView;
            }
            return new LabelView(expression.ToString())
            {
                OnClick = () => model.Select(expression),
                Width = 3 * NUMVAR_SIZE / 5 * (expression.ToString().Length + 0.25),
                Height = NUMVAR_SIZE,
                Baseline = NUMVAR_SIZE / 2,
                TextColor = expression.Selected ? new Color(39, 174, 97) : new Color(0, 0, 0)
            };
        }

        public void Build(ExpressionModel model)
        {
            if (model != null)
            {
                setContent(BuildView(model.Expression, model));
            }
        }

        public void Update(ExpressionModel model)
        {
            Build(model);
            if (OnChanged != null)
            {
                OnChanged();
            }
        }

        public ExpressionView() : this(0, 0)
        { }

        public ExpressionView(double width, double height) : this(null, width, height, 0)
        { }

        public ExpressionView(ExpressionModel model, double width, double height, double maxScale) : base(width, height)
        {
            MaxScale = maxScale;
            Build(model);
        }
    }
}
