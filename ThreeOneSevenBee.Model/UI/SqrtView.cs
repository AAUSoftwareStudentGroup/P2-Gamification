using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Euclidean;

namespace ThreeOneSevenBee.Model.UI
{
    public class SqrtView : View
    {
        public double SignWidth { get; set; }
        public double TopHeight { get; set; }

        public Color LineColor { get; set; }
        public double LineWidth { get; set; }

        public SqrtView() : base(0, 0, 30, 20)
        {
            LineColor = new Color(0, 0, 0);
            LineWidth = 5;
            SignWidth = 10;
            TopHeight = 5;
        }

        public override void DrawWithContext(IContext context, double offsetX, double offsetY)
        {
            context.DrawPolygon(
                new Vector2[]
                {
                    new Vector2(X + offsetX + SignWidth / 8, Y + offsetY + Height - SignWidth / 2),
                    new Vector2(X + offsetX + SignWidth / 4, Y + offsetY + Height - SignWidth / 2),
                    new Vector2(X + offsetX + SignWidth / 2 - LineWidth / 2, Y + offsetY + Height),
                    new Vector2(X + offsetX + SignWidth / 2, Y + offsetY + Height),
                    new Vector2(X + offsetX + SignWidth, Y + offsetY + TopHeight / 2),
                    new Vector2(X + offsetX + Width, Y + offsetY + TopHeight / 2)
                },
                new Color(),
                LineColor,
                LineWidth
            );
        }

        public override View Scale(double factor)
        {
            SignWidth *= factor;
            TopHeight *= factor;
            LineWidth *= factor;
            return base.Scale(factor);
        }

        public override bool ContainsPoint(double x, double y)
        {
            return base.ContainsPoint(x, y) && (X + SignWidth > x || Y + TopHeight > y);
        }
    }
}
