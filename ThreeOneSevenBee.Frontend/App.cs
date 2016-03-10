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

            ExpressionModel model = new ExpressionModel("a+b", Rules.ItselfRule, Rules.CommutativeRule, Rules.FractionVariableMultiplyRule);

            View view = new CompositeView(600, 400)
            {
                new IdentityMenuView(model, 600, 20) { Y = 60 },
                new ExpressionView(model, 220, 100) { X = 20, Y = 20 },
                new ButtonView("Hello", () => Global.Alert("Hello")) { X = 100, Y = 100, Width = 40, Height = 20 },
                new ButtonView("World", () => Global.Alert("World")) { X = 200, Y = 100, Width = 40, Height = 20 }
            };

            Console.WriteLine(view);

            context.SetContentView(view);
            model.OnChanged += (m) => context.Draw();
            context.Draw();
        }
    }
}