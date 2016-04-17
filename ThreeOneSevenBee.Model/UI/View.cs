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

        public Color BackgroundColor { get; set; }

        public bool Visible { get; set; }

        public View(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Baseline = height / 2;
            BackgroundColor = new Color();
        }

        public virtual void DrawWithContext(IContext context, double offsetX, double offsetY)
        {
            Console.WriteLine(this.GetType());
            context.DrawRectangle(X + offsetX, Y + offsetY, Width, Height, BackgroundColor);
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
