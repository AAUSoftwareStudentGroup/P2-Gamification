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
            CanvasElement canvas = Document.GetElementById<CanvasElement>("canvas");

            CanvasContext context = new CanvasContext(canvas);

            ExpressionModel model = new ExpressionModel("{b^3*a^2}/{a^5*b^2}", 
                Rules.DivideRule, Rules.ExponentToProductRule, Rules.ProductToExponentRule);

            View view = new CompositeView(600, 400)
            {
                new ProgressbarStarView(new ProgressbarStar(50, 100, 30, 60, 75), 600, 20) { Y=30 },
                new ExpressionView(model, 600, 300) { X = 0, Y = 50 },
                new IdentityMenuView(model, 600, 50) { Y = 350 }
            };
            
            context.SetContentView(view);
            model.OnChanged += (m) => context.Draw();
            context.Draw();
        }
    }
}