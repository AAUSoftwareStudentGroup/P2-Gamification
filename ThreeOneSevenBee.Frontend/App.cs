﻿using Bridge;
using Bridge.Html5;
using ThreeOneSevenBee.Model;
using ThreeOneSevenBee.Model.UI;

namespace ThreeOneSevenBee.Frontend
{
    public class App
    {
        [Ready]
        public static void Main()
        {
            var t = new Template();
            Console.WriteLine(t.ToString());

            CanvasElement canvas = Document.GetElementById<CanvasElement>("canvas");

            CanvasContext context = new CanvasContext(canvas);

            View view = new CompositeView(600, 400)
            {
                new LabelView("Click on a Button!") { X = 100, Y = 60, Width = 40, Height = 20 },
                new ButtonView("Hello", () => Global.Alert("Hello")) { X = 100, Y = 100, Width = 40, Height = 20 },
                new ButtonView("World", () => Global.Alert("World")) { X = 200, Y = 100, Width = 40, Height = 20 }
            };

            context.SetContentView(view);
            context.Draw();
        }
    }
}