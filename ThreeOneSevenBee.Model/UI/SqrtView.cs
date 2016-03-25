using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace threeonesevenbee.Model.UI
{
    public class SqrtView : View
    {
        public double SignWidth { get; set; }
        public double TopHeight { get; set; }
        public SqrtView() : base(0, 0, 30, 20)
        {
            SignWidth = 10;
            TopHeight = 5;
        }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX, offsetY);
        }

        public override View Scale(double factor)
        {
            SignWidth *= factor;
            TopHeight *= factor;
            return base.Scale(factor);
        }
    }
}
