using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace threeonesevenbee.Model.UI
{
    public enum ParenthesisType { Left, Right };
    public class ParenthesisView : View
    {
        public ParenthesisType Type { get; set; }

        public ParenthesisView(ParenthesisType type) : base(0, 0, 10, 10)
        {
            Type = type;
        }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX, offsetY);
        }
    }
}
