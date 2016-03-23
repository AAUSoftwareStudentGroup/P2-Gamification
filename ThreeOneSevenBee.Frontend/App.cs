using Bridge;
using Bridge.Html5;
using ThreeOneSevenBee.Model;
using ThreeOneSevenBee.Model.UI;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.ExpressionRules;
using ThreeOneSevenBee.Model.Geometry;
using System.Collections.Generic;
using ThreeOneSevenBee.Model.Euclidean;

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

            ExpressionModel expressionModel = new ExpressionModel("(sqrt{sqrt{a}^b/b^3+b-(b*b)^b})", 
                Rules.DivideRule, Rules.ExponentToProductRule, Rules.ProductToExponentRule);

            PolygonModel polygon = new PolygonModel(4);
            List<Vector2> cornerpos = new List<Vector2>();
            cornerpos.Add(new Vector2(0, 0));
            cornerpos.Add(new Vector2(0, 1));
            cornerpos.Add(new Vector2(1, 1));
            cornerpos.Add(new Vector2(1, 0));

            View view = new CompositeView(canvas.Width, canvas.Height)
            {
                new ProgressbarStarView(new ProgressbarStar(50, 100, 30, 60, 75), canvas.Width, 20) { Y=30 },
                new ExpressionView(expressionModel, canvas.Width, canvas.Height - 150) { X = 0, Y = 50 },
                new IdentityMenuView(expressionModel, canvas.Width, 100) { Y = canvas.Height - 100 },
                new PolygonView(polygon, cornerpos, 200, 200, 100, 100)
            };
            context.SetContentView(view);
            expressionModel.OnChanged += (m) => context.Draw();
            context.Draw();
        }
    }
}