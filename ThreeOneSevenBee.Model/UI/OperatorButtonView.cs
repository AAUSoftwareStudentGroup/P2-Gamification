using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Expression.Expressions;

namespace ThreeOneSevenBee.Model.UI
{
    public class OperatorButtonView : ButtonView
    {
        public OperatorType type { get; set; }

        public OperatorButtonView(OperatorType type, Action onClick) : base("", onClick)
        {
            this.type = type;
        }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX, offsetY);
        }
    }
}
