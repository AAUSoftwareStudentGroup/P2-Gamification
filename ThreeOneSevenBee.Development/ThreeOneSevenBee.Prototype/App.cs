using Bridge;
using Bridge.Html5;
using ThreeOneSevenBee.Framework;

namespace ThreeOneSevenBee.Prototype
{
    public class App
    {
        [Ready]
        public static void Main()
        {
            var canvas = Document.GetElementById<CanvasElement>("canvas");
            var game = new PrototypeGame(canvas);
            game.Run();
        }
    }
}