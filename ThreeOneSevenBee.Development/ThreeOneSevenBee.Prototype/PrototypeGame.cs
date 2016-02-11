using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using ThreeOneSevenBee.Framework;

namespace ThreeOneSevenBee.Prototype
{
    class PrototypeGame : Game
    {
        public PrototypeGame(CanvasElement canvas)
            : base(canvas)
        {

        }

        public override void Draw(double deltaTime, double totalTime)
        {
            Context2D.Clear(HTMLColor.CornflowerBlue);

            Context2D.DrawString(50, 50, "Hello, World!", "20px Arial", HTMLColor.Black);

            Context2D.DrawString(50, 70, deltaTime.ToString("0.00"), "20px Arial", HTMLColor.Black);

            Context2D.DrawString(50, 90, totalTime.ToString("0.00"), "20px Arial", HTMLColor.Black);

            base.Draw(deltaTime, totalTime);
        }
    }
}
