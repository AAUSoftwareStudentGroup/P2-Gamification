using System;

namespace ThreeOneSevenBee.Model.UI
{
    public class View
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public double Baseline { get; set; }

        public string BackgroundColor { get; set; }

        public bool Visible { get; set; }

        public View(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Baseline = height / 2;
            BackgroundColor = "transparent";
        }

        public virtual void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX, offsetY);
        }

        public virtual void Click(double x, double y)
        {
            if(ContainsPoint(x, y) && OnClick != null)
            {
                OnClick();
            }
        }

        public Action OnClick;
        public Action OnChanged;
        
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
