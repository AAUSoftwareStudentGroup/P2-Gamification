using Bridge;
using Bridge.Html5;
using ThreeOneSevenBee.Model;
using ThreeOneSevenBee.Model.UI;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.ExpressionRules;
using ThreeOneSevenBee.Model.Geometry;
using System.Collections.Generic;
using ThreeOneSevenBee.Model.Euclidean;
using ThreeOneSevenBee.Model.Game;


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

            IContext context = new CanvasContext(canvas);

            JQueryGameAPI gameAPI = new JQueryGameAPI();

            GameModel gameModel;
            GameView gameView;

            gameAPI.GetCurrentPlayer((u) =>
            {
                u.AddCategory(new LevelCategory("wat") { new Level("a*(a+a)", "a*(a+a)", new string[] { "2*a^2" }) });
                gameAPI.GetPlayers((p) =>
                {
                    gameModel = new GameModel(u, p)
                    {
                        OnSaveLevel = (level) => 
                        gameAPI.SaveUserLevelProgress
                        (
                            level.LevelID,
                            level.CurrentExpression,
                            (success) => Console.WriteLine(success)
                        )
                    };
                    gameModel.CurrentExpression.PrettyPrint();
                    gameView = new GameView(gameModel, context);
                });
            });
        }
    }
}