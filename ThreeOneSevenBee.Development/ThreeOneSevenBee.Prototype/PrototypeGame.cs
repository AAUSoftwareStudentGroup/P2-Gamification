using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using ThreeOneSevenBee.Framework;
using ThreeOneSevenBee.Framework.Euclidean;

namespace ThreeOneSevenBee.Prototype
{
    class PrototypeGame : Game
    {
        private Circle circleA;

        private Rectangle rectangleA;

        public PrototypeGame(CanvasElement canvas)
            : base(canvas)
        {
            circleA = new Circle
            {
                Center = new Vector2(200, 200),
                Radius = 48
            };

            rectangleA = new Rectangle()
            {
                Location = new Vector2(400, 400),
                Width = 96,
                Height = 96
            };
        }

        public override void Draw(double deltaTime, double totalTime)
        {
            Context2D.Clear(HTMLColor.CornflowerBlue);

            Context2D.DrawString(50, 50, "Hello, World!", "20px Arial", HTMLColor.Black);

            Context2D.DrawString(50, 70, "Delta: " + deltaTime.ToString("0.00") + "ms", "20px Arial", HTMLColor.Black);

            Context2D.DrawString(50, 90, "Total: " + totalTime.ToString("0.00") + "ms", "20px Arial", HTMLColor.Black);

            if (Input.IsMouseOver == true) // == true is needed because of issue #933 in Bridge.Net.
                Context2D.DrawString(50, 110, Input.Mouse.ToString(), "20px Arial", HTMLColor.Black);
            else
                Context2D.DrawString(50, 110, Input.Mouse.ToString(), "20px Arial", HTMLColor.LightGray);

            if (circleA.Contains(Input.Mouse))
                Context2D.FillCircle(circleA, HTMLColor.Red);
            Context2D.DrawCircle(circleA, HTMLColor.Black);

            if (rectangleA.Contains(Input.Mouse))
                Context2D.FillRectangle(rectangleA, HTMLColor.Red);
            Context2D.DrawRectangle(rectangleA, HTMLColor.Black);

            base.Draw(deltaTime, totalTime);
        }
    }
}
