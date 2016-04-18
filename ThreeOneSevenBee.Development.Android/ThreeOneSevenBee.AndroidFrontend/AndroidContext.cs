using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using A = Android.Views;
using Android.Widget;
using ThreeOneSevenBee.Model.Euclidean;
using ThreeOneSevenBee.Model.UI;
using Android.Graphics;

namespace ThreeOneSevenBee.AndroidFrontend
{
    public class AndroidContext : A.View, IContext
    {
        View contentView;
        Canvas canvas;
        double width, height;

        public AndroidContext(Android.Content.Context context, double width, double height) : base(context)
        {
            contentView = null;
            this.width = width;
            this.height = height;
        }

        public override bool OnTouchEvent(A.MotionEvent e)
        {
            contentView.Click(e.GetX(), e.GetY());
            return base.OnTouchEvent(e);
        }

        public override void Draw(Canvas canvas)
        {
            if(contentView != null)
            {
                this.canvas = canvas;
                contentView.DrawWithContext(this, 0, 0);
            }
        }

        double IContext.Height
        {
            get
            {
                return height;
            }
        }

        double IContext.Width
        {
            get
            {
                return width;
            }
        }

        public void Clear()
        {
            
        }

        public void Draw()
        {
            Invalidate();
        }

        public void DrawLine(Vector2 first, Vector2 second, Model.UI.Color lineColor, double lineWidth)
        {
            Paint paint = new Paint();
            paint.Color = new Android.Graphics.Color((byte)lineColor.Red, (byte)lineColor.Green, (byte)lineColor.Blue, (byte)(lineColor.Alpha * 255));
            paint.StrokeWidth = (float)lineWidth;
            canvas.DrawLine((float)first.X, (float)first.Y, (float)second.X, (float)second.Y, paint);
        }

        public void DrawPolygon(Vector2[] path, Model.UI.Color fillColor)
        {
            DrawPolygon(path, fillColor, new Model.UI.Color(), 0);
        }

        public void DrawPolygon(Vector2[] path, Model.UI.Color fillColor, Model.UI.Color lineColor, double lineWidth)
        {
            Path polygonPath = new Path();
            polygonPath.MoveTo((float)path[0].X, (float)path[0].Y);
            foreach (Vector2 point in path)
            {
                polygonPath.LineTo((float)point.X, (float)point.Y);
            }
            Paint paint = new Paint();
            paint.Color = new Android.Graphics.Color((byte)fillColor.Red, (byte)fillColor.Green, (byte)fillColor.Blue, (byte)(fillColor.Alpha * 255));
            paint.StrokeWidth = (float)lineWidth;
            paint.SetStyle(Paint.Style.Fill);
            canvas.DrawPath(polygonPath, paint);
            paint.Color = new Android.Graphics.Color((byte)lineColor.Red, (byte)lineColor.Green, (byte)lineColor.Blue, (byte)(lineColor.Alpha * 255));
            paint.SetStyle(Paint.Style.Stroke);
            canvas.DrawPath(polygonPath, paint);
        }

        public void DrawRectangle(double x, double y, double width, double height, Model.UI.Color fillColor)
        {
            DrawRectangle(x, y, width, height, fillColor, new Model.UI.Color(), 0);
        }

        public void DrawRectangle(double x, double y, double width, double height, Model.UI.Color fillColor, Model.UI.Color lineColor, double lineWidth)
        {
            Paint paint = new Paint();
            paint.Color = new Android.Graphics.Color((byte)fillColor.Red, (byte)fillColor.Green, (byte)fillColor.Blue, (byte)(fillColor.Alpha * 255));
            canvas.DrawRect((float)x, (float)y, (float)(x + width), (float)(y + height), paint);
        }

        public void DrawText(double x, double y, double width, double height, string text, Model.UI.Color textColor)
        {
            Paint paint = new Paint();
            paint.TextAlign = Paint.Align.Center;
            paint.TextSize = (float)(height);
            if(paint.MeasureText(text) > width)
            {
                paint.TextSize *= (float)(width / paint.MeasureText(text));
            }
            paint.AntiAlias = true;
            
            paint.Color = new Android.Graphics.Color((byte)textColor.Red, (byte)textColor.Green, (byte)textColor.Blue, (byte)(textColor.Alpha * 255));
            canvas.DrawText(text, (float)(x + width / 2), (float)(y + height / 2 + paint.TextSize / 2), paint);
        }

        public void SetContentView(Model.UI.View view)
        {
            contentView = view;
            Invalidate();
        }
    }
}