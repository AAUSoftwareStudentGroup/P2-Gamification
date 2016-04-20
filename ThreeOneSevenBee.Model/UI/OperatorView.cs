using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Expression.Expressions;
using ThreeOneSevenBee.Model.Euclidean;

namespace ThreeOneSevenBee.Model.UI
{
    public class OperatorView : View
    {
        public OperatorType Type { get; set; }

        public double LineWidth { get; set; }
        public Color LineColor { get; set; }

        public OperatorView(OperatorType type) : base(0, 0, 10, 10)
        {
            Type = type;
            LineWidth = 1;
            LineColor = new Color(0, 0, 0);
        }

        public override void DrawWithContext(IContext context, double offsetX, double offsetY)
        {
            base.DrawWithContext(context, offsetX, offsetY);
            switch (Type)
            {
                case OperatorType.Add:
                    context.DrawText(X + offsetX, Y + offsetY, Width, Height, "+", LineColor);
                    break;
                case OperatorType.Subtract:
                    context.DrawText(X + offsetX, Y + offsetY, Width, Height, "-", LineColor);
                    break;
                case OperatorType.Minus:
                    context.DrawText(X + offsetX, Y + offsetY, Width, Height, "-", LineColor);
                    break;
                case OperatorType.Divide:
                    context.DrawLine(
                        new Vector2(X + offsetX, Y + offsetY + Height / 2),
                        new Vector2(X + offsetX + Width, Y + offsetY + Height / 2),
                        LineColor,
                        LineWidth
                    );
                    break;
                case OperatorType.Multiply:
                    context.DrawText(X + offsetX, Y + offsetY, Width, Height, "·", LineColor);
                    break;
                case OperatorType.Power:
                    break;
                default:
                    break;
            }
        }

        public override View Scale(double factor)
        {
            LineWidth *= factor;
            return base.Scale(factor);
        }
    }
}
