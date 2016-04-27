using System.Collections;
using System.Collections.Generic;
using ThreeOneSevenBee.Model.Euclidean;
#if BRIDGE
using Bridge.Html5;
#endif
using System;
namespace ThreeOneSevenBee.Model.UI
{
    public class CompositeView : View, IEnumerable<View>
    {
        public CompositeView(double width, double height) : base(0, 0, width, height)
        {
            Children = new List<View>();
            PropagateClick = true;
            PropagateKeypress = true;
        }

        public List<View> Children;

        public bool PropagateClick { set; get; }
        public bool PropagateKeypress { set; get; }
        public override bool Active
        {
            get
            {
                if (Children != null)
                {
                    foreach (View child in Children)
                    {
                        if (child.Active)
                            return true;
                    }
                }
				return false;
            }

            set
            {
                if (value == false && Children != null)
                {
                    foreach (View child in Children)
                    {
                        child.Active = value;
                    }
                }
                base.Active = value;
            }
        }

        public override void DrawWithContext(IContext context, double offsetX, double offsetY)
        {
            if (Visible == true)
            {
                base.DrawWithContext(context, offsetX, offsetY);
                foreach (var child in Children)
                {
                    child.DrawWithContext(context, offsetX + X, offsetY + Y);
                }
            }
        }

		public override void Click(double x, double y, IContext context)
        {
            if (base.ContainsPoint(x, y))
            {
                Active = true;
                if (PropagateClick)
                {
                    foreach (View child in Children)
                    {
						child.Click(x - X, y - Y, context);
                    }
                }
                
                if(OnClick != null)
                {
                    OnClick();
                }
            } else
            {
                Active = false;
            }
        }

        public override void KeyPressed(string key, IContext context)
        {
            if (PropagateKeypress)
            {
                foreach (View child in Children)
                {
					child.KeyPressed(key, context);
                }
            }
        }

        public override View Scale(double factor)
        {
            foreach (View child in Children)
            {
                child.Scale(factor);
            }
            return base.Scale(factor);
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

        public void Clear()
        {
            Children.Clear();
        }
    }
}
