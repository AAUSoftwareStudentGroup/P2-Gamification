using System;
using Bridge.Html5;
using ThreeOneSevenBee.Model.UI;
using ThreeOneSevenBee.Model.Expression.Expressions;
using System.Collections.Generic;

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

        public override void Draw(LabelView view, double offsetX, double offsetY)
        {
            Draw(view as View, offsetX, offsetY);
            context.Font = view.FontSize + "px " + view.Font;
            context.TextAlign = view.Align == "center" ? CanvasTypes.CanvasTextAlign.Center :
                CanvasTypes.CanvasTextAlign.Left;
            context.FillStyle = view.FontColor;
            context.FillText(view.Text, (int)(view.X + offsetX + (view.Align == "center" ? view.Width / 2 : 5)),
                (int)(view.Y + offsetY + view.Height / 2));
            context.FillStyle = "#000000";
        }

        public override void Draw(OperatorView view, double offsetX, double offsetY)
        {
            Draw(view as View, offsetX, offsetY);
            context.LineWidth = view.LineWidth;
            context.StrokeStyle = view.LineColor;
            if (view.type == OperatorType.Divide)
            {
                context.BeginPath();
                context.MoveTo(view.X + offsetX, view.Y + offsetY + view.Height / 2);
                context.LineTo(view.X + offsetX + view.Width, view.Y + offsetY + view.Height / 2);
                context.Stroke();
            }
            else if (view.type == OperatorType.Multiply)
            {
                context.BeginPath();
                context.Arc(view.X + offsetX + view.Width / 2, view.Y + offsetY + view.Height / 2, view.Height / 10, 0, 2 * Math.PI);
                context.Fill();
                context.Stroke();
            }
            else if (view.type == OperatorType.Add)
            {
                context.BeginPath();
                context.MoveTo(view.X + offsetX + view.Width / 2, view.Y + offsetY - view.Height / 3 + view.Height / 2);
                context.LineTo(view.X + offsetX + view.Width / 2, view.Y + offsetY + view.Height / 3 + view.Height / 2);
                context.MoveTo(view.X + offsetX + view.Width / 2 - view.Height / 3, view.Y + offsetY + view.Height / 2);
                context.LineTo(view.X + offsetX + view.Width / 2 + view.Height / 3, view.Y + offsetY + view.Height / 2);
                context.Stroke();
            }
            else if (view.type == OperatorType.Subtract)
            {
                context.BeginPath();
                context.MoveTo(view.X + offsetX + view.Width / 2 - view.Height / 3, view.Y + offsetY + view.Height / 2);
                context.LineTo(view.X + offsetX + view.Width / 2 + view.Height / 3, view.Y + offsetY + view.Height / 2);
                context.Stroke();
            }
            else if (view.type == OperatorType.Minus)
            {
                context.BeginPath();
                context.MoveTo(view.X + offsetX + view.Width / 3, view.Y + offsetY + view.Height / 2);
                context.LineTo(view.X + offsetX + view.Width, view.Y + offsetY + view.Height / 2);
                context.Stroke();
            }
            context.LineWidth = 2;
            context.StrokeStyle = "black";
        }

        public override void Draw(ParenthesisView view, double offsetX, double offsetY)
        {
            context.StrokeStyle = view.LineColor;
            context.LineWidth = view.LineWidth;
            if (view.Type == ParenthesisType.Left)
            {
                context.BeginPath();
                context.Ellipse(view.X + view.Width + offsetX + view.LineWidth / 2, view.Y + view.Height / 2 + offsetY, view.Width, 1.1 * view.Height / 2, 0, -1.141096661 + Math.PI, 1.141096661 + Math.PI);
                context.Stroke();
            }
            else
            {
                context.BeginPath();
                context.Ellipse(view.X + offsetX - view.LineWidth / 2, view.Y + view.Height / 2 + offsetY, view.Width, 1.1 * view.Height / 2, 0, -1.141096661, 1.141096661);
                context.Stroke();
            }
            context.LineWidth = 2;
            context.StrokeStyle = "black";
        }

        public override void Draw(SqrtView view, double offsetX, double offsetY)
        {
            context.BeginPath();
            context.MoveTo(view.X + offsetX + view.SignWidth / 8, view.Y + offsetY + view.Height - view.SignWidth / 2);
            context.LineTo(view.X + offsetX + view.SignWidth / 4, view.Y + offsetY + view.Height - view.SignWidth / 2);
            context.LineTo(view.X + offsetX + view.SignWidth / 2, view.Y + offsetY + view.Height);
            context.LineTo(view.X + offsetX + view.SignWidth, view.Y + offsetY + view.TopHeight / 2);
            context.LineTo(view.X + offsetX + view.Width, view.Y + offsetY + view.TopHeight / 2);
            context.Stroke();
        }

        public override void Draw(View view, double offsetX, double offsetY)
        {
            context.FillStyle = view.BackgroundColor;
            context.FillRect((int)(view.X + offsetX), (int)(view.Y + offsetY), (int)view.Width, (int)view.Height);
            context.FillStyle = "#000000";
        }

        public override void Draw(ImageView view, double offsetX, double offsetY)
        {
            Draw(view as View, offsetX, offsetY);

            if (imageCache.ContainsKey(view.Image))
            {
                context.FillStyle = "transparent";
                context.DrawImage(imageCache[view.Image], view.X + offsetX, view.Y + offsetY, view.Width, view.Height);
                context.FillStyle = "#000000";
            }
            else
            {
                imageCache[view.Image] = new ImageElement();
                imageCache[view.Image].Src = "img/" + view.Image;
                imageCache[view.Image].OnLoad = (e) =>
                {
                    context.FillStyle = "transparent";
                    context.DrawImage(imageCache[view.Image], view.X + offsetX, view.Y + offsetY, view.Width, view.Height);
                    context.FillStyle = "#000000";
                };
            }
        }

        public override void Draw(PolygonView view, double offsetX, double offsetY)
        {
            context.FillStyle = view.fillStyle;
            if (view.cornerPositions.Count < 3)
                throw new Exception("Polygon does not contain enough corners");
            context.BeginPath();
            context.MoveTo(view.cornerPositions[0].X + offsetX, view.cornerPositions[0].Y + offsetY);
            for (int i = 1; i < view.cornerPositions.Count; i++)
            {
                context.LineTo(view.cornerPositions[i].X + offsetX, view.cornerPositions[i].Y + offsetY);
            }
            context.ClosePath();
            context.Stroke();
            context.Fill();
        }

        public override void Draw(ToolTipView view, double offsetX, double offsety)
        {
            if (view.Visible == true)
            {
                context.FillStyle = view.BackgroundColor;
                if (view.Position == position.upperLeft)
                {
                    context.BeginPath();
                    context.MoveTo(view.X + offsetX, view.Y + offsety + 10);
                    context.LineTo(view.X + offsetX + 10, view.Y + offsety);
                    context.LineTo(view.X + offsetX + 20, view.Y + offsety + 10);
                    context.LineTo(view.X + offsetX + view.Width, view.Y + offsety + 10);
                    context.LineTo(view.X + offsety + view.Width, view.Y + offsety + view.Height);
                    context.LineTo(view.X + offsetX, view.Y + offsetX + view.Height);
                    context.ClosePath();
                    context.Fill();
                    context.Font = view.FontSize + "px " + view.Font;
                    context.FillStyle = view.FontColor;
                    context.FillText(view.Text, (int)(view.X + offsetX + (view.Align == "center" ? view.Width / 2 : 5)),
                        (int)(view.Y + offsety + view.Height / 2));
                }
                if (view.Position == position.upperRight)
                {
                    context.BeginPath();
                    context.MoveTo(view.X + offsetX, view.Y + offsety + 10);
                    context.LineTo(view.X + offsetX + 10, view.Y + offsety);
                    context.LineTo(view.X + offsetX + 20, view.Y + offsety + 10);
                    context.LineTo(view.X + offsetX + 20, view.Y + offsety + view.Height);
                    context.LineTo(view.X + offsetX - view.Width, view.Y + offsety + view.Height);
                    context.LineTo(view.X + offsetX - view.Width, view.Y + offsety + 10);
                    context.ClosePath();
                    context.Fill();
                    context.Font = view.FontSize + "px " + view.Font;
                    context.FillStyle = view.FontColor;
                    context.FillText(view.Text, (int)(view.X + offsetX - (view.Align == "center" ? view.Width / 2 : 5)),
                        (int)(view.Y + offsety + view.Height / 2));
                }
                if (view.Position == position.bottomMiddle)
                {
                    context.BeginPath();
                    context.MoveTo(view.X + offsetX, view.Y + offsety + 10);
                    context.LineTo(view.X + offsetX + view.Width / 2 - 10, view.Y + offsety + 10);
                    context.LineTo(view.X + offsetX + view.Width / 2, view.Y + offsety);
                    context.LineTo(view.X + offsetX + view.Width / 2 + 10, view.Y + offsety + 10);
                    context.LineTo(view.X + offsetX + view.Width, view.Y + offsety + 10);
                    context.LineTo(view.X + offsetX + view.Width, view.Y + offsety);
                    context.LineTo(view.X + offsetX + view.Width, view.Y + offsety + view.Height);
                    context.LineTo(view.X + offsetX, view.Y + offsety + view.Height);
                    context.ClosePath();
                    context.Fill();
                    context.Font = view.FontSize + "px " + view.Font;
                    context.FillStyle = view.FontColor;
                    context.FillText(view.Text, (int)(view.X + offsetX + (view.Align == "center" ? view.Width / 2 : 5)),
                        (int)(view.Y + offsety + view.Height / 4 + 10));
                    context.FillText(view.Description, (int)(view.X + offsetX + (view.Align == "center" ? view.Width / 2 : 5)),
                        (int)(view.Y + offsety + view.Height / 2 + 20));
                }
            }
        }
    }
}
