using System;
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
            context.Font = "12px Arial Black";
        }

        public override void SetContentView(View view)
        {
            double canvasLeft = context.Canvas.GetBoundingClientRect().Left;
            double canvasRight = context.Canvas.GetBoundingClientRect().Left;
            context.Canvas.AddEventListener(EventType.MouseDown, 
                (e) =>
                {
                    view.Click(e.As<MouseEvent>().ClientX - (int)canvasLeft, e.As<MouseEvent>().ClientY - (int)canvasRight);
                });
            base.SetContentView(view);
        }

        public override void Clear()
        {
            context.ClearRect(0, 0, (int)Width, (int)Height);
        }

        public override void Draw(ProgressbarStarView view, double offsetX, double offsetY)
        {
            context.BeginPath();
            context.Rect(view.X + offsetX, view.Y + offsetY, view.Width, view.Height);
            context.Rect(view.X + offsetX, view.Y + offsetY, view.Width * view.progressbar.Percentage, view.Height);
            context.ClosePath();
            context.Stroke();
        }

        public override void Draw(LabelView view, double offsetX, double offsetY)
        {
            context.TextBaseline = CanvasTypes.CanvasTextBaselineAlign.Middle;
            context.TextAlign = CanvasTypes.CanvasTextAlign.Center;
            context.Font = view.Height/1.5 + "px Arial Black";
            context.FillText(view.Text, (int)(view.X + offsetX + view.Width / 2), (int)(view.Y + offsetY + view.Height / 2));
        }

        public override void Draw(ButtonView view, double offsetX, double offsetY)
        {
            context.FillStyle = view.Selected ? HTMLColor.LightBlue : "#eeeeee";
            context.FillRect((int)(view.X + offsetX), (int)(view.Y + offsetY), (int)view.Width, (int)view.Height);
            context.FillStyle = HTMLColor.Black;
            Draw(view as LabelView, offsetX, offsetY);
        }

        public override void Draw(OperatorButtonView view, double offsetX, double offsetY)
        {
            if (view.type == OperatorType.Divide)
            {
                context.BeginPath();
                context.LineCap = CanvasTypes.CanvasLineCapType.Round;
                context.LineWidth = view.Height/40;
                context.MoveTo(view.X + offsetX, view.Y + offsetY + view.Height / 2);
                context.LineTo(view.X + offsetX + view.Width, view.Y + offsetY + view.Height / 2);
                context.Stroke();
            }
            else if (view.type == OperatorType.Multiply)
            {
                context.BeginPath();
                //context.Rect(, view.Width / 10,);
                context.Arc(view.X + offsetX + view.Width / 2 - view.Height / 20, view.Y + offsetY + view.Height / 2 - view.Height / 20, view.Height / 10, 0, 2 * Math.PI);
                context.Fill();
                context.Stroke();
            }
            else if (view.type == OperatorType.Add)
            {
                context.BeginPath();
                context.LineWidth = view.Height / 20;
                context.MoveTo(view.X + offsetX + view.Width / 2, view.Y + offsetY - view.Height / 3 + view.Height / 2);
                context.LineTo(view.X + offsetX + view.Width / 2, view.Y + offsetY + view.Height / 3 + view.Height / 2);
                context.MoveTo(view.X + offsetX - view.Width / 3 + view.Width / 2, view.Y + offsetY + view.Height / 2);
                context.LineTo(view.X + offsetX + view.Width / 3 + view.Width / 2, view.Y + offsetY + view.Height / 2);
                context.Stroke();
                context.ClosePath();
            }
            else if (view.type == OperatorType.Subtract)
            {
                context.BeginPath();
                context.LineWidth = view.Height / 20;
                context.MoveTo(view.X + offsetX - view.Width / 3 + view.Width / 2, view.Y + offsetY + view.Height / 2);
                context.LineTo(view.X + offsetX + view.Width / 3 + view.Width / 2, view.Y + offsetY + view.Height / 2);
                context.Stroke();
                context.ClosePath();
            }
        }
    }
}
