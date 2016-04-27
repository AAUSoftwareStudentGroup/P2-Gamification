using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Euclidean;

namespace ThreeOneSevenBee.Model.UI
{
    public class FrameView : View
    {
        public FrameView(double width, double height) : this(width, height, new View(0,0,0,0), true, 0)
        { }

        public FrameView(double width, double height, View content) :
            this(width, height, content, true, height / content.Height)
        { }

        public FrameView(double width, double height, View content, double maxScale) :
            this(width, height, content, true, maxScale)
        { }

        public FrameView(double width, double height, View content, bool propagateClick, double maxScale) : base(0, 0, width, height)
        {
            Width = width;
            Height = height;
            PropagateClick = propagateClick;
            PropagateKeypress = true;
            MaxScale = maxScale;
            Padding = 0;
            Content = Align(Fit(content));
        }

        public View Content { protected set; get; }
        public bool PropagateClick { set; get; }
        public bool PropagateKeypress { set; get; }
        public double Padding;
        public double InnerX { get { return X - Padding; } }
        public double InnerY { get { return Y - Padding; } }
        public double InnerWidth { get { return Width - Padding; } }
        public double InnerHeight { get { return Height - Padding; } }

        public override double Width
        {
            get
            {
                return base.Width;
            }

            set
            {
                if(Content != null)
                    Content = Align(Fit(Content));
                base.Width = value;
            }
        }

        public override bool Active
        {
            get
            {
                return Content.Active;
            }

            set
            {
                if(Content != null)
                    Content.Active = value;
            }
        }

        public double MaxScale;

        public virtual void setContent(View content)
        {
            Content = Align(Fit(content));
        }

        public virtual void updateContent()
        {
            Content = Align(Fit(Content));
        }

		public override void Click(double x, double y, IContext context)
        {

            if (base.ContainsPoint(x, y))
            {
                if (PropagateClick)
                {
					Content.Click(x - X, y - Y, context);
                }

                if (OnClick != null)
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
                Content.KeyPressed(key, context);
            }
        }

        public override void DrawWithContext(IContext context, double offsetX, double offsetY)
        {
            base.DrawWithContext(context, offsetX, offsetY);
            Content.DrawWithContext(context, offsetX + InnerX, offsetY + InnerY);
        }

        public View Align(View view)
        {
            view.X = (InnerWidth - view.Width) / 2;
            view.Y = (InnerHeight - view.Height) / 2;
            return view;
        }

        public View Fit(View view)
        {
            return view.Scale(System.Math.Min(InnerWidth / view.Width, Math.Min(InnerHeight / view.Height, MaxScale == 0 ? Double.MaxValue : MaxScale)));
        }

        public override View Scale(double factor)
        {
            Content.Scale(factor);
            return base.Scale(factor);
        }
    }
}
