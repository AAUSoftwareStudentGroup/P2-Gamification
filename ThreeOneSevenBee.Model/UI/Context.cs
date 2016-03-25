#if BRIDGE
using Bridge.Html5;
#else
using System;
#endif

namespace threeonesevenbee.Model.UI
{
    public abstract class Context
    {
        public Context(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public virtual double Width { get; protected set; }
        public virtual double Height { get; protected set; }

        private View _contentView;

        public abstract void Clear();

        public virtual void SetContentView(View view)
        {
            _contentView = view;
        }

        public void Draw()
        {
            Clear();
            _contentView.DrawWithContext(this, 0, 0);
        }

        public abstract void Draw(View view, double offsetX, double offsetY);

        public virtual void Draw(ImageView view, double offsetX, double offsetY)
        {
            Draw(view as View, offsetX, offsetY);
        }

        public virtual void Draw(LabelView view, double offsetX, double offsetY)
        {
            Draw(view as View, offsetX, offsetY);
        }

        public virtual void Draw(ButtonView view, double offsetX, double offsetY)
        {
            Draw(view as LabelView, offsetX, offsetY);
        }

        public virtual void Draw(ProgressbarStarView view, double offsetX, double offsetY)
        {
            Draw(view as View, offsetX, offsetY);
        }

        public virtual void Draw(OperatorView view, double offsetX, double offsetY)
        {
            Draw(view as View, offsetX, offsetY);
        }

        public virtual void Draw(ParenthesisView view, double offsetX, double offsetY)
        {
            Draw(view as View, offsetX, offsetY);
        }

        public virtual void Draw(SqrtView view, double offsetX, double offsetY)
        {
            Draw(view as View, offsetX, offsetY);
        }

        public virtual void Draw(PolygonView view, double offsetX, double offsetY)
        {
            Draw(view as View, offsetX, offsetY);
        }
    }
}
