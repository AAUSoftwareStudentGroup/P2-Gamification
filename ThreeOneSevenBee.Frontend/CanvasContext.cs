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
        InputElement input;
        public Vector2 lastClick { get; private set; }

        public CanvasContext(CanvasElement canvas, InputElement input) : base(canvas.Width, canvas.Height)
        {
            imageCache = new Dictionary<string, ImageElement>();

            this.input = input;
            input.Type = InputType.Text;
            input.Focus();
            input.OnInput = (e) =>
            {
                KeyPressed(input.Value.Substr(Math.Max(0, input.Value.Length - 1), input.Value.Length));
                input.SelectionStart = 1;
            };
            input.OnKeyDown = (e) =>
            {
                int keyCode = e.As<KeyboardEvent>().KeyCode;
                Console.WriteLine(keyCode);
                if(keyCode == 8)
                {
                    KeyPressed("Back");
                }
            };

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
            Document.Body.AddEventListener(EventType.Click,
                (e) =>
                {
                    if (contentView.Active == true)
                    {
                        input.Focus();
                    }
                });

            Window.OnResize = (e) => ResizeContent();
        }

        public void ResizeContent()
        {
            context.Canvas.Width = Document.DocumentElement.ClientWidth;
            context.Canvas.Height = Document.DocumentElement.ClientHeight;
            Width = context.Canvas.Width;
            Height = context.Canvas.Height;
            contentView.Width = Width;
            contentView.Height = Height;
            if(OnResize != null)
            {
                OnResize(Width, Height);
            }
            Draw();
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
            contentView.Click(x, y, this);
            Vector2 last = lastClick;
            last.X = x;
            last.Y = y;
            lastClick = last;
            Draw();
        }

        private void KeyPressed(string text)
        {
            contentView.KeyPressed(text, this);
            Draw();
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

        public void DrawText(double x, double y, double width, double height, string text, Color textColor)
        {
            string[] lines = text.Split('\n');

            context.TextBaseline = CanvasTypes.CanvasTextBaselineAlign.Middle;
            context.FillStyle = ColorToString(textColor);
            double minFontSize = height;

            foreach (string line in lines)
            {
                context.Font = height / lines.Length + "px Arial";
                context.TextAlign = CanvasTypes.CanvasTextAlign.Left;
                if (context.MeasureText(line).Width > width)
                {
                    minFontSize = Math.Min(minFontSize, width / context.MeasureText(line).Width * (height / lines.Length));
                }
            }

            for (int index = 0; index < lines.Length; index++)
            {
                context.Font = minFontSize + "px Arial";
                context.TextAlign = CanvasTypes.CanvasTextAlign.Left;
                context.FillText(lines[index], (int)(x), (int)(y + (index + 0.5) * (height / lines.Length)));
            }
        }

        public override void DrawPNGImage(string fileName, double x, double y, double width, double height)
        {

            if (imageCache.ContainsKey(fileName))
            {
                context.FillStyle = "transparent";
                context.DrawImage(imageCache[fileName], x, y, width, height);
                context.FillStyle = "#000000";
            }
            else
            {
                ImageElement img = new ImageElement();
                img.Src = "img/" + fileName;
                img.OnLoad = (e) =>
                {
                    context.FillStyle = "transparent";
                    context.DrawImage(img, x, y, width, height);
                    context.FillStyle = "#000000";
                    imageCache[fileName] = img;
                };
            }
        }

        public override Vector2 GetTextDimensions(string text, double maxWidth, double maxHeight)
        {
            double minFontSize = maxHeight;
            context.Font = maxHeight + "px Arial";
            context.TextAlign = CanvasTypes.CanvasTextAlign.Left;
            context.Font = minFontSize + "px Arial";
            if (context.MeasureText(text).Width > maxWidth)
            {
                minFontSize = Math.Min(minFontSize, maxWidth / context.MeasureText(text).Width * maxHeight);
            }
            return new Vector2(context.MeasureText(text).Width, minFontSize);
        }

        public override void DrawText(double x, double y, double width, double height, string text, Color textColor, TextAlignment alignment)
        {
            DrawText(x, y, width, height, text, textColor);
        }
    }
}
