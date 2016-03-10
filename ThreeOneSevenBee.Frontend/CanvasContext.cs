using System;
using Bridge.Html5;
using ThreeOneSevenBee.Model.UI;

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
            context.FillText(view.Text, (int)(view.X + offsetX + view.Width / 2), (int)(view.Y + offsetY + view.Height / 2));
        }

        public override void Draw(ButtonView view, double offsetX, double offsetY)
        {
            context.FillStyle = view.Selected ? HTMLColor.Skyblue : HTMLColor.White;
            context.FillRect((int)(view.X + offsetX), (int)(view.Y + offsetY), (int)view.Width, (int)view.Height);
            context.FillStyle = HTMLColor.Black;
            Draw(view as LabelView, offsetX, offsetY);
        }
    }
}
