using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Model.UI
{
    public class ProgressbarStarView : View
    {
        public ProgressbarStar progressbar;

        public ProgressbarStarView(ProgressbarStar progressbar)
        {
            this.progressbar = new ProgressbarStar(50, 100);
        }

        public override void DrawWithContext(Context context)
        {
            context.Draw(this);
        }
    }
}
