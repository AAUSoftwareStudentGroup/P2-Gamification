using System;
using Bridge.Html5;
using ThreeOneSevenBee.Model.UI;
using ThreeOneSevenBee.Model.Expression.Expressions;
using System.Collections.Generic;
using ThreeOneSevenBee.Model.Euclidean;

namespace ThreeOneSevenBee.Frontend
{
    class CanvasContext : Context
    {
        private Dictionary<string, ImageElement> imageCache;

        CanvasRenderingContext2D context;

        public CanvasContext(CanvasElement canvas) : base(canvas.Width, canvas.Height)
        {
            imageCache = new Dictionary<string, ImageElement>();

            context = canvas.GetContext(CanvasTypes.CanvasContext2DType.CanvasRenderingContext2D);
            context.FillStyle = "#000000";
            context.LineWidth = 2;
            context.TextBaseline = CanvasTypes.CanvasTextBaselineAlign.Middle;

            double canvasLeft = context.Canvas.GetBoundingClientRect().Left;
            double canvasRight = context.Canvas.GetBoundingClientRect().Left;
            context.Canvas.AddEventListener(EventType.MouseDown,
                (e) =>
                {
                    click(e.As<MouseEvent>().ClientX + Document.Body.ScrollLeft - (int)canvasLeft,
                        e.As<MouseEvent>().ClientY + Document.Body.ScrollTop - (int)canvasRight);
                });
        }

        public string ColorToString(Color color)
        {
            return string.Format("rgba({0},{1},{2},{3})", color.Red.ToString(), color.Green.ToString(), color.Blue.ToString(), color.Alpha.ToString());
        }

        public override void SetContentView(View view)
        {
            base.SetContentView(view);
            Draw();
        }

        public override void Clear()
        {
            context.ClearRect(0, 0, (int)Width, (int)Height);
        }

        private void click(double x, double y)
        {
            contentView.Click(x, y);
        }

        public override void DrawPolygon(Vector2[] path, Color fillColor, Color lineColor, double lineWidth)
        {
            context.FillStyle = ColorToString(fillColor);
            context.StrokeStyle = ColorToString(lineColor);
            context.LineWidth = lineWidth;

            context.BeginPath();
            context.MoveTo(path[0].X, path[0].Y);

            foreach (Vector2 point in path)
            {
                context.LineTo(point.X, point.Y);
            }
            context.Fill();
            context.Stroke();
        }

        public override void DrawText(double x, double y, double width, double height, string text, Color textColor)
        {
            context.Font = height + "px Segoe UI";
            context.TextAlign = CanvasTypes.CanvasTextAlign.Center;
            if (context.MeasureText(text).Width > width)
            {
                Console.WriteLine(context.MeasureText(text).Width);
                context.Font = width / context.MeasureText(text).Width * height + "px Segoe UI";
            }
            context.TextBaseline = CanvasTypes.CanvasTextBaselineAlign.Middle;
            context.FillStyle = ColorToString(textColor);
            context.FillText(text, (int)(x + width / 2), (int)(y + height / 2));
        }
    }
}
