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
            Context2D.ClearRect(0, 0, 800, 600);

            Context2D.FillStyle = HTMLColor.CornflowerBlue;
            Context2D.FillRect(0, 0, 800, 600);

            Context2D.FillStyle = HTMLColor.Black;
            Context2D.Font = "20px Arial";
            Context2D.FillText("Hello, World!", 50, 50);

            base.Draw(deltaTime, totalTime);
        }
    }
}
