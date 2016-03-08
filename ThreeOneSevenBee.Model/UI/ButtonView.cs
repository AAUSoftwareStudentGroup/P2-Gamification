using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class ButtonView : LabelView
    {
        public ButtonView(string text, Action onClick) : base(text)
        {
            OnClick = onClick;
        }

        public override void DrawWithContext(Context context)
        {
            context.Draw(this);
        }
    }
}
