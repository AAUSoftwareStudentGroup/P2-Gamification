using System;
using Bridge.Html5;
using ThreeOneSevenBee.Framework.Euclidean;

namespace ThreeOneSevenBee.Framework
{
    public static class CanvasRenderingContext2DExtensions
    {
        // ==========
        // = CLEAR
        // ==========
        public static void Clear(this CanvasRenderingContext2D context2D)
        {
            context2D.ClearRect(0, 0, context2D.Canvas.Width, context2D.Canvas.Height);
        }

        public static void Clear(this CanvasRenderingContext2D context2D, string fillStyle)
        {
            context2D.Save();

            context2D.FillStyle = fillStyle;
            context2D.FillRect(0, 0, context2D.Canvas.Width, context2D.Canvas.Height);

            context2D.Restore();
        }

        // ==========
        // = LINES
        // ==========
        public static void DrawLine(this CanvasRenderingContext2D context2D, double aX, double aY, double bX, double bY,
            string strokeStyle)
        {
            context2D.Save();

            context2D.StrokeStyle = strokeStyle;
            context2D.BeginPath();
            context2D.MoveTo(aX, aY);
            context2D.LineTo(bX, bY);
            context2D.ClosePath();
            context2D.Stroke();

            context2D.Restore();
        }

        public static void DrawLine(this CanvasRenderingContext2D context2D, Vector2 a, Vector2 b, string strokeStyle)
        {
            context2D.DrawLine(a.X, a.Y, b.X, b.Y, strokeStyle);
        }

        // ==========
        // = CIRCLES
        // ==========
        public static void DrawCircle(this CanvasRenderingContext2D context2D, double centerX, double centerY, double radius,
            string strokeStyle)
        {
            context2D.Save();

            context2D.StrokeStyle = strokeStyle;
            context2D.BeginPath();
            context2D.ArcTo(centerX, centerY, 0.00, 2 * Math.PI, radius);
            context2D.ClosePath();
            context2D.Stroke();

            context2D.Restore();
        }

        public static void DrawCircle(this CanvasRenderingContext2D context2D, Vector2 center, double radius, string strokeStyle)
        {
            context2D.DrawCircle(center.X, center.Y, radius, strokeStyle);
        }

        public static void DrawCircle(this CanvasRenderingContext2D context2D, Circle c, string strokeStyle)
        {
            context2D.DrawCircle(c.Center.X, c.Center.Y, c.Radius, strokeStyle);
        }

        public static void FillCircle(this CanvasRenderingContext2D context2D, double centerX, double centerY, double radius,
            string fillStyle)
        {
            context2D.Save();

            context2D.FillStyle = fillStyle;
            context2D.BeginPath();
            context2D.ArcTo(centerX, centerY, 0.00, 2 * Math.PI, radius);
            context2D.ClosePath();
            context2D.Fill();

            context2D.Restore();
        }

        public static void FillCircle(this CanvasRenderingContext2D context2D, Vector2 center, double radius, string fillStyle)
        {
            context2D.FillCircle(center.X, center.Y, radius, fillStyle);
        }

        public static void FillCircle(this CanvasRenderingContext2D context2D, Circle c, string fillStyle)
        {
            context2D.FillCircle(c.Center.X, c.Center.Y, c.Radius, fillStyle);
        }

        // ==========
        // = RECTANGLES
        // ==========
        public static void DrawRectangle(this CanvasRenderingContext2D context2D, double left, double top, double width, double height,
            string strokeStyle)
        {
            context2D.Save();

            context2D.StrokeStyle = strokeStyle;
            context2D.BeginPath();
            context2D.Rect(left, top, width, height);
            context2D.ClosePath();
            context2D.Stroke();

            context2D.Restore();
        }

        public static void DrawRectangle(this CanvasRenderingContext2D context2D, Vector2 leftTop, double width, double height,
            string strokeStyle)
        {
            context2D.DrawRectangle(leftTop.X, leftTop.Y, width, height, strokeStyle);
        }

        public static void DrawRectangle(this CanvasRenderingContext2D context2D, Rectangle rectangle, string strokeStyle)
        {
            context2D.DrawRectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height, strokeStyle);
        }

        public static void FillRectangle(this CanvasRenderingContext2D context2D, double left, double top, double width, double height,
            string fillStyle)
        {
            context2D.Save();

            context2D.FillStyle = fillStyle;
            context2D.BeginPath();
            context2D.Rect(left, top, width, height);
            context2D.ClosePath();
            context2D.Fill();

            context2D.Restore();
        }

        public static void FillRectangle(this CanvasRenderingContext2D context2D, Vector2 leftTop, double width, double height,
            string fillStyle)
        {
            context2D.FillRectangle(leftTop.X, leftTop.Y, width, height, fillStyle);
        }

        public static void FillRectangle(this CanvasRenderingContext2D context2D, Rectangle rectangle, string fillStyle)
        {
            context2D.FillRectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height, fillStyle);
        }

        // ==========
        // = TEXT
        // ==========
        public static void DrawString(this CanvasRenderingContext2D context2D, double left, double bottom, string text, string font,
            string strokeStyle)
        {
            context2D.Save();

            context2D.StrokeStyle = strokeStyle;
            context2D.Font = font;
            context2D.FillText(text, (int)left, (int)bottom);

            context2D.Restore();
        }

        public static  void StrokeString(this CanvasRenderingContext2D context2D, double left, double bottom, string text, string font,
            string strokeStyle)
        {
            context2D.Save();

            context2D.StrokeStyle = strokeStyle;
            context2D.Font = font;
            context2D.StrokeText(text, (int)left, (int)bottom);

            context2D.Restore();
        }
    }
}
