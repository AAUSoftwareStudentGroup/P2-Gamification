using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class CompositeView : View
    {
        public CompositeView(double width, double height)
        {
            Width = width;
            Height = height;
            Children = new List<View>();
        }

        public List<View> Children;

        public override void DrawWithContext(Context context)
        {
            foreach (var child in Children)
            {
                child.DrawWithContext(context);
            }
        }

        public override void Click(int x, int y)
        {
            if (base.ContainsPoint(x, y))
            {
                foreach (View child in Children)
                {
                    child.Click(x, y);
                }
            }
            base.OnClick();
        }
    }
}
