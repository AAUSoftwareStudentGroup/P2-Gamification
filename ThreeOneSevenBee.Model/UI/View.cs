using System;

namespace ThreeOneSevenBee.Model.UI
{
    public abstract class View
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public double Baseline { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; }

        public abstract void DrawWithContext(Context context, double offsetX, double offsetY);

        public virtual void Click(double x, double y)
        {
            if(ContainsPoint(x, y) && OnClick != null)
            {
                OnClick();
            }
        }

        public Action OnClick;
        
        public virtual View Scale(double factor)
        {
            X *= factor;
            Y *= factor;
            Width *= factor;
            Height *= factor;
            return this;
        }

        public virtual bool ContainsPoint(double x, double y)
        {
            return x >= X && y >= Y && x <= X + Width && y <= Y + Height;
        }
    }
}
