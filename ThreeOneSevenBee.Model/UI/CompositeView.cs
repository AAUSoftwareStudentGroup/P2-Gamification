﻿using System.Collections;
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
            PropagateClick = true;
        }

        public List<View> Children;

        public bool PropagateClick { set; get; }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            foreach (var child in Children)
            {
                child.DrawWithContext(context, offsetX + X, offsetY + Y);
            }
        }

        public override void Click(double x, double y)
        {
            if (base.ContainsPoint(x, y))
            {
                if(PropagateClick)
                {
                    foreach (View child in Children)
                    {
                        child.Click(x - X, y - Y);
                    }
                }
                
                if(OnClick != null)
                {
                    OnClick();
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
    }
}
