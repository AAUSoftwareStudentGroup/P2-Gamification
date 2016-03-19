using Bridge;
using Bridge.Html5;
using ThreeOneSevenBee.Model;
using ThreeOneSevenBee.Model.UI;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.ExpressionRules;

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

            ExpressionModel model = new ExpressionModel("a--b");

            View view = new CompositeView(600, 400)
            {
                new IdentityMenuView(model, 600, 200) { Y = 100 },
                new ExpressionView(model, 600, 400) { X = 0, Y = 0 }
            };

            Console.WriteLine(view);

            context.SetContentView(view);
            model.OnChanged += (m) => context.Draw();
            model.OnChanged += (m) => { if (m.Selected != null) { m.Selected.PrettyPrint(); } };
            context.Draw();
        }
    }
}