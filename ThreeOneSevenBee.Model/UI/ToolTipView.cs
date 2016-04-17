using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class ToolTipView : LabelView
    {
        public ToolTipView(string text) : base(text)
        {
        }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX, offsetY);
        }

        public position Position { get; set; }
        public string Description { get; set; }

    }
    public enum position { upperLeft, upperRight, bottomMiddle }
}
