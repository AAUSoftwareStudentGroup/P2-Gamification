using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public enum ParenthesisType { Left, Right };
    public class ParenthesisView : View
    {
        public ParenthesisType Type { get; set; }

        public Color LineColor { get; set; }

        public double LineWidth { get; set; }

        public ParenthesisView(ParenthesisType type) : base(0, 0, 10, 10)
        {
            Type = type;
        }

        public override void DrawWithContext(IContext context, double offsetX, double offsetY)
        {
			context.DrawText(X + offsetX, Y + offsetY, Width, Height, Type == ParenthesisType.Left ? "(" : ")", LineColor, TextAlignment.Centered);
        }

        public override View Scale(double factor)
        {
            LineWidth *= factor;
            return base.Scale(factor);
        }
    }
}
