using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class LabelView : View
    {
        public virtual Color TextColor { get; set; }

        public LabelView(string text) : base(0, 0, 10, 10)
        {
            Text = text;
            TextColor = new Color(0, 0, 0);
        }

        public virtual string Text { get; set; }

        public override void DrawWithContext(IContext context, double offsetX, double offsetY)
        {
            base.DrawWithContext(context, offsetX, offsetY);
            context.DrawText(X + offsetX, Y + offsetY, Width, Height, Text, TextColor);
        }
    }
}
