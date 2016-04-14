using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class LabelView : View
    {
        public string FontColor { get; set; }
        public string Font { get; set; }
        public double FontSize { get; set; }
        public string Align { get; set; }

        public LabelView(string text) : base(0, 0, 10, 10)
        {
            Text = text;
            FontColor = "#000000";
            Font = "Segoe UI";
            FontSize = Height;
            Align = "center";
        }

        public override View Scale(double factor)
        {
            FontSize *= factor;
            return base.Scale(factor);
        }

        public string Text { get; set; }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX, offsetY);
        }
    }
}
