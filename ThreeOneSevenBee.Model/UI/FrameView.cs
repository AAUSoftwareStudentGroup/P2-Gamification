using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class FrameView : View
    {
        public FrameView(double width, double height, View content) : 
            this(width, height, content, true, height / content.Height) { }

        public FrameView(double width, double height, View content, double maxScale) :
            this(width, height, content, true, maxScale) { }

        public FrameView(double width, double height, View content, bool propagateClick, double maxScale)
        {
            Width = width;
            Height = height;
            PropagateClick = propagateClick;
            MaxScale = maxScale;
            Content = Align(Fit(content));
        }

        public View Content { protected set; get; }
        public bool PropagateClick { set; get; }
        public double MaxScale;

        public void setContent(View content)
        {
            Content = Align(Fit(content));
        }

        public override void Click(double x, double y)
        {
            
            if (base.ContainsPoint(x, y))
            {
                if (PropagateClick)
                {
                    Content.Click(x - X, y - Y);
                }

                if (OnClick != null)
                {
                    Console.WriteLine("tets");
                    OnClick();
                }
            }
        }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            Content.DrawWithContext(context, offsetX + X, offsetY + Y);
        }

        public View Align(View view)
        {
            view.X = (Width - view.Width) / 2;
            view.Y = (Height - view.Height) / 2;
            return view;
        }

        public View Fit(View view)
        {
            return view.Scale(System.Math.Min(Width / view.Width, Math.Min(Height / view.Height, MaxScale)));
        }
    }
}
