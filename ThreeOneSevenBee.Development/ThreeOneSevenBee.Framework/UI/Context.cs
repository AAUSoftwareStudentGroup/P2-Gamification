using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Framework.UI
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

        public void SetContentView(View view)
        {
            _contentView = view;
        }

        public void Draw()
        {
            Clear();
            _contentView.DrawWithContext(this);
        }

        public abstract void Draw(ButtonView buttonView);

        public abstract void Draw(LabelView labelView);

        public abstract void Draw(ProgressbarStarView progressbarStarView);
    }
}
