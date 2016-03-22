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
            canvas.Width = Document.DocumentElement.ClientWidth;
            canvas.Height = Document.DocumentElement.ClientHeight;

            CanvasContext context = new CanvasContext(canvas);

            ExpressionModel expressionModel = new ExpressionModel("(a+b/{e*f+q)", 
                Rules.DivideRule, Rules.ExponentToProductRule, Rules.ProductToExponentRule);

            View view = new CompositeView(canvas.Width, canvas.Height)
            {
                new ProgressbarStarView(new ProgressbarStar(50, 100, 30, 60, 75), canvas.Width, 20) { Y=30 },
                new ExpressionView(expressionModel, canvas.Width, canvas.Height - 70) { X = 0, Y = 50 },
                new IdentityMenuView(expressionModel, canvas.Width, 50) { Y = canvas.Height - 50 }
            };

            context.SetContentView(view);
            expressionModel.OnChanged += (m) => context.Draw();
            context.Draw();
        }
    }
}