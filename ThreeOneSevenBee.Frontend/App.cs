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

            ExpressionModel model = new ExpressionModel("4+5", Rules.ItselfRule);

            model.Expression.Clone();

            View view = new CompositeView(600, 400)
            {
                new ExpressionView(model, 600, 300) { X = 0, Y = 0 },
                new IdentityMenuView(model, 600, 100) { Y = 300 }
            };

            Console.WriteLine(view);

            context.SetContentView(view);
            model.OnChanged += (m) => context.Draw();
            model.OnChanged += (m) => { if (m.Selected != null) { m.Selected.PrettyPrint(); } };
            context.Draw();
        }
    }
}