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
            Document.Body.AddEventListener(EventType.TouchStart, (e) => { e.PreventDefault(); });
            Document.Body.AddEventListener(EventType.TouchMove, (e) => { e.PreventDefault(); });
            Document.Body.AddEventListener(EventType.TouchEnd, (e) => { e.PreventDefault(); });
            Document.Body.AddEventListener(EventType.TouchCancel, (e) => { e.PreventDefault(); });

            var canvas = Document.GetElementById<CanvasElement>("canvas");
            var game = new PrototypeGame(canvas);

            var 

            game.Run();
        }
    }
}