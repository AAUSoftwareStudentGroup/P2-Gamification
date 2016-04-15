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

        public string LineColor { get; set; }

        public double LineWidth { get; set; }

        public ParenthesisView(ParenthesisType type) : base(0, 0, 10, 10)
        {
            Type = type;
        }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX, offsetY);
        }

        public override View Scale(double factor)
        {
            LineWidth *= factor;
            return base.Scale(factor);
        }
    }
}
