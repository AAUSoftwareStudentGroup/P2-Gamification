﻿using System;
using Bridge.Html5;
using ThreeOneSevenBee.Model.UI;
using ThreeOneSevenBee.Model.Expression.Expressions;

namespace ThreeOneSevenBee.Frontend
{
    class CanvasContext : Context
    {
        CanvasRenderingContext2D context;

        public CanvasContext(CanvasElement canvas) : base(canvas.Width, canvas.Height)
        {
            context = canvas.GetContext(CanvasTypes.CanvasContext2DType.CanvasRenderingContext2D);
            context.FillStyle = "#000000";
            context.LineWidth = 2;
            context.TextBaseline = CanvasTypes.CanvasTextBaselineAlign.Middle;
            context.TextAlign = CanvasTypes.CanvasTextAlign.Center;
        }

        public override void SetContentView(View view)
        {
            double canvasLeft = context.Canvas.GetBoundingClientRect().Left;
            double canvasRight = context.Canvas.GetBoundingClientRect().Left;
            context.Canvas.AddEventListener(EventType.MouseDown, 
                (e) =>
                {
                    view.Click(e.As<MouseEvent>().ClientX + Document.Body.ScrollLeft - (int)canvasLeft, 
                        e.As<MouseEvent>().ClientY + Document.Body.ScrollTop - (int)canvasRight);
                });
            base.SetContentView(view);
        }

        public override void Clear()
        {
            context.ClearRect(0, 0, (int)Width, (int)Height);
        }

        public override void Draw(LabelView view, double offsetX, double offsetY)
        {
            Draw(view as View, offsetX, offsetY);
            context.Font = view.Height + "px Cambria Math";
            context.FillText(view.Text, (int)(view.X + offsetX + view.Width / 2), (int)(view.Y + offsetY + view.Height / 2));
        }

        public override void Draw(OperatorView view, double offsetX, double offsetY)
        {
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
                context.MoveTo(view.X + offsetX - view.Width / 3 + view.Width / 2, view.Y + offsetY + view.Height / 2);
                context.LineTo(view.X + offsetX + view.Width / 3 + view.Width / 2, view.Y + offsetY + view.Height / 2);
                context.Stroke();
            }
            else if (view.type == OperatorType.Subtract)
            {
                context.BeginPath();
                context.MoveTo(view.X + offsetX - view.Width / 3 + view.Width / 2, view.Y + offsetY + view.Height / 2);
                context.LineTo(view.X + offsetX + view.Width / 3 + view.Width / 2, view.Y + offsetY + view.Height / 2);
                context.Stroke();
            }
        }

        public override void Draw(ParenthesisView view, double offsetX, double offsetY)
        {
            if(view.Type == ParenthesisType.Left)
            {
                context.BeginPath();
                context.Ellipse(view.X + view.Width + offsetX, view.Y + view.Height / 2 + offsetY, view.Width, 1.1 * view.Height / 2, 0, -1.141096661 + Math.PI, 1.141096661 + Math.PI);
                context.Stroke();
            }
            else
            {
                context.BeginPath();
                context.Ellipse(view.X + offsetX, view.Y + view.Height / 2 + offsetY, view.Width, 1.1 * view.Height / 2, 0, -1.141096661, 1.141096661);
                context.Stroke();
            }
        }

        public override void Draw(SqrtView view, double offsetX, double offsetY)
        {
            context.BeginPath();
            context.MoveTo(view.X + offsetX, view.Y + offsetY + view.Height - view.SignWidth / 2);
            context.LineTo(view.X + offsetX + view.SignWidth / 4 - view.SignWidth / 8, view.Y + offsetY +   view.Height - view.SignWidth / 2 );
            context.LineTo(view.X + offsetX + view.SignWidth / 2 - view.SignWidth / 8, view.Y + offsetY + view.Height);
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
            ImageElement img = new ImageElement();
            img.Src = "img/" + view.Image;
            context.DrawImage(img, view.X + offsetX, view.Y + offsetY, view.Width, view.Height);
        }

    }
}
