using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class ToolTipView : LabelView
    {
        public ToolTipView(string text, int pointX, int pointY) : base(text)
        {
            this.PointX = pointX;
            this.PointY = pointY;
        }

        public int PointX { get; set; }
        public int PointY { get; set; }
    }
}
