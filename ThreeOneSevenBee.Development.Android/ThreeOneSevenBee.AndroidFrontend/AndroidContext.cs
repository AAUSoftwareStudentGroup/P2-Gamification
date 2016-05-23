using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using A = Android.Views;
using Android.Widget;
using ThreeOneSevenBee.Model.Euclidean;
using ThreeOneSevenBee.Model.UI;
using Android.Graphics;
using Android.Views.InputMethods;
using Android.InputMethodServices;
using Android.Text;

namespace ThreeOneSevenBee.AndroidFrontend
{
    public class AndroidContext : EditText, IContext
    {
        FrameView contentView;
        Canvas canvas;
        InputMethodManager input;
        double width, height;
        Dictionary<string, Bitmap> imageCache;

        public Action<double, double> OnResize { get; set; }

        public AndroidContext(Android.Content.Context context, InputMethodManager input, double width, double height) : base(context)
        {
            contentView = null;
            this.width = width;
            this.height = height;
            this.input = input;

            this.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;

            imageCache = new Dictionary<string, Bitmap>()
            {
                {"icon.png", BitmapFactory.DecodeResource(Resources, Resource.Drawable.Icon) },
                {"brøkbadge.png", BitmapFactory.DecodeResource(Resources, Resource.Drawable.brokbadge) },
                {"master_of_algebrabadge.png", BitmapFactory.DecodeResource(Resources, Resource.Drawable.master_of_algebrabadge) },
                {"parenthesis_badge.png", BitmapFactory.DecodeResource(Resources, Resource.Drawable.parenthesis_badge) },
                {"star.png", BitmapFactory.DecodeResource(Resources, Resource.Drawable.star) },
                {"star_activated.png", BitmapFactory.DecodeResource(Resources, Resource.Drawable.star_activated) },
                {"tutorial_badge.png", BitmapFactory.DecodeResource(Resources, Resource.Drawable.tutorial_badge) },
                {"restart.png", BitmapFactory.DecodeResource(Resources, Resource.Drawable.restart) },
                {"potens_badge.png", BitmapFactory.DecodeResource(Resources, Resource.Drawable.potens_badge) }
            };

            Text = "";
            Append("webmat");
            this.TextChanged += (obj, args) =>
            {
                Console.WriteLine(this.Text);
                if (args.Text.ToString() == "webma")
                {
                    contentView.KeyPressed("Back", this);
                    Text = "";
                    Append("webmat");
                }
                else if(args.Text.ToString().Length > "webmat".Length)
                {
                    contentView.KeyPressed(args.Text.Last() + "", this);
                    Text = "";
                    Append("webmat");
                }
            };
        }

        protected override void OnFocusChanged(bool gainFocus, [GeneratedEnum] A.FocusSearchDirection direction, Rect previouslyFocusedRect)
        {
            base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
            ImeOptions = (ImeAction)ImeFlags.NoExtractUi;
        }
    

         public override IInputConnection OnCreateInputConnection(EditorInfo outAttrs)
        {
            outAttrs.ImeOptions = ImeFlags.NoFullscreen;
            return base.OnCreateInputConnection(outAttrs);
        }

        public override bool OnTouchEvent(A.MotionEvent e)
        {
            contentView.Click(e.GetX(), e.GetY(), this);
            if (contentView.Active)
            {
                input.ShowSoftInput(this, ShowFlags.Forced);
            }
            else
            {
                input.HideSoftInputFromWindow(this.WindowToken, 0);
            }
            Draw();
            return false;
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
            paint.SetStyle(Android.Graphics.Paint.Style.Fill);
            canvas.DrawPath(polygonPath, paint);
            paint.Color = new Android.Graphics.Color((byte)lineColor.Red, (byte)lineColor.Green, (byte)lineColor.Blue, (byte)(lineColor.Alpha * 255));
            paint.SetStyle(Android.Graphics.Paint.Style.Stroke);
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

        public void DrawText(double x, double y, double width, double height, string text, Model.UI.Color textColor, Model.UI.TextAlignment alignment)
        {
            Paint paint = new Paint();
            switch (alignment)
            {
                case Model.UI.TextAlignment.Left:
                    paint.TextAlign = Android.Graphics.Paint.Align.Left;
                    break;
                case Model.UI.TextAlignment.Right:
                    paint.TextAlign = Android.Graphics.Paint.Align.Right;
                    break;
                case Model.UI.TextAlignment.Centered:
                    paint.TextAlign = Android.Graphics.Paint.Align.Center;
                    break;
                default:
                    break;
            }
            
            string[] lines = text.Split('\n');
            double minFontSize = height / lines.Length;
            paint.AntiAlias = true;
            paint.Color = new Android.Graphics.Color((byte)textColor.Red, (byte)textColor.Green, (byte)textColor.Blue, (byte)(textColor.Alpha * 255));
            foreach (string line in lines)
            {
                paint.TextSize = (float)(height/ lines.Length);
                if (paint.MeasureText(line) > width)
                {
                    minFontSize = Math.Min(minFontSize, width / paint.MeasureText(line) * (height / lines.Length));
                }
            }
            paint.TextSize = (float)minFontSize;
            for (int index = 0; index < lines.Length; index++)
            {
                canvas.DrawText(lines[index], (float)(x + (alignment == Model.UI.TextAlignment.Centered ? width / 2 : 0)), (float)(y + height - paint.Descent() - ((height / lines.Length) * (0.5 - index + lines.Length - 1) - paint.TextSize / 2)), paint);
            }

        }

        public void SetContentView(Model.UI.FrameView view)
        {
            contentView = view;
            Draw();
        }

        public void DrawPNGImage(string fileName, double x, double y, double width, double height)
        {
            Paint paint = new Paint();
            paint.AntiAlias = true;
            
            canvas.DrawBitmap(imageCache[fileName], null, new Rect((int)x, (int)y, (int)(x + width), (int)(y + height)), paint);
        }

        public Vector2 GetTextDimensions(string text, double maxWidth, double maxHeight)
        {
            Paint paint = new Paint();
            double minFontSize = maxHeight;
            paint.AntiAlias = true;
            paint.TextAlign = Android.Graphics.Paint.Align.Left;
            paint.TextSize = (float)minFontSize;
            if (paint.MeasureText(text) > maxWidth)
            {
                minFontSize = Math.Min(minFontSize, maxWidth / paint.MeasureText(text) * maxHeight);
            }
            return new Vector2(paint.MeasureText(text), minFontSize);
        }
    }
}