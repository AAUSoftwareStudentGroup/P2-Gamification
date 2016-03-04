using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Framework.UI
{
    public class LabelView : View
    {
        public LabelView(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        public override void DrawWithContext(Context context)
        {
            context.Draw(this);
        }
    }
}
