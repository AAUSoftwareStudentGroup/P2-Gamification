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
            PointX = 0;
            PointY = 0;
        }

        public int PointX { get; set; }
        public int PointY { get; set; }
    }
}
