using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using threeonesevenbee.Model.Expression.Expressions;

namespace threeonesevenbee.Model.UI
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
