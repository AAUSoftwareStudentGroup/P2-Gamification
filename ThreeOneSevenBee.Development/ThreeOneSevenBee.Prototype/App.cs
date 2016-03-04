using Bridge;
using Bridge.Html5;
using ThreeOneSevenBee.Framework;
using ThreeOneSevenBee.Framework.UI;
using Bridge.Linq;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Prototype
{
    public class App
    {
        [Ready]
        public static void Main()
        {
            var canvas = Document.GetElementById<CanvasElement>("canvas");
            var canvasContext = new CanvasContext(canvas);

            View view = new CompositeView(1000, 1000)
            {
                Children = new List<View>()
                {
                    new LabelView("Hello") { X = 30, Y = 40, Width = 40, Height = 20 },
                    new ButtonView("World") { X = 50, Y = 40, Width = 40, Height = 20 },
                    new ProgressbarStarView(new ProgressbarStar(50, 100)) { X = 100, Y = 100, Width = 400, Height = 40 }
                }
            };

            canvasContext.SetContentView(view);

            canvasContext.Draw();
        }
    }
}