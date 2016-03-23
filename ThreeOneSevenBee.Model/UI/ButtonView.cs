using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace threeonesevenbee.Model.UI
{
    public class ButtonView : LabelView
    {
        public ButtonView(string text, Action onClick) : base(text)
        {
            OnClick = onClick;
        }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX, offsetY);
        }
    }
}
