using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace threeonesevenbee.Model.UI
{
    public class LabelView : View
    {
        public LabelView(string text) : base(0, 0, 10, 10)
        {
            Text = text;
        }

        public string Text { get; set; }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX, offsetY);
        }
    }
}
