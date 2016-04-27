#if BRIDGE
using System;
using Bridge.Html5;
#else
using System;
#endif
using ThreeOneSevenBee.Model.Euclidean;

namespace ThreeOneSevenBee.Model.UI
{
    public abstract class Context : IContext
    {
        public Context(double width, double height)
        {
            contentView = new View(0,0,0,0);
            Width = width;
            Height = height;
        }

        public virtual double Width { get; set; }
        public virtual double Height { get; set; }
        public Action<double, double> OnResize { get; set; }
        public View contentView;

        public abstract void Clear();

        public virtual void SetContentView(View view)
        {
            contentView = view;
        }

        public void Draw()
        {
            Clear();
            contentView.DrawWithContext(this, 0, 0);
        }

        public virtual void DrawRectangle(double x, double y, double width, double height, Color fillColor)
        {
            DrawRectangle(x, y, width, height, fillColor, new Color(), 0);
        }

        public virtual void DrawLine(Vector2 first, Vector2 second, Color lineColor, double lineWidth)
        {
            DrawPolygon(
                new Vector2[]
                {
                    first,
                    second
                },
                new Color(),
                lineColor,
                lineWidth
            );
        }

        public virtual void DrawRectangle(double x, double y, double width, double height, Color fillColor, Color lineColor, double lineWidth)
        {
            DrawPolygon(
                new Vector2[]
                {
                    new Vector2(x, y),
                    new Vector2(x + width, y),
                    new Vector2(x + width, y + height),
                    new Vector2(x, y + height)
                },
                fillColor,
                lineColor,
                lineWidth
            );
        }

        public virtual void DrawPolygon(Vector2[] path, Color fillColor)
        {
            DrawPolygon(path, fillColor, new Color(), 1);
        }

        public abstract void DrawPolygon(Vector2[] path, Color fillColor, Color lineColor, double lineWidth);

        public abstract void DrawText(double x, double y, double width, double height, string text, Color textColor);

        public abstract void DrawPNGImage(string fileName, double x, double y, double width, double height);

		public abstract Vector2 GetTextDimensions(string text, double maxWidth, double maxHeight);
    }
}
