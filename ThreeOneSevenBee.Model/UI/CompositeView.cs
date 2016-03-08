using System.Collections;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.UI
{
    public class CompositeView : View, IEnumerable<View>
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

        public IEnumerator<View> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(View view)
        {
            Children.Add(view);
        }
    }
}
