using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Expression.Expressions;

namespace ThreeOneSevenBee.Model.UI
{
    public class OperatorView : View
    {
        public OperatorType type { get; set; }

        public OperatorView(OperatorType type) : base(0, 0, 10, 10)
        {
            this.type = type;
        }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX, offsetY);
        }
    }
}
