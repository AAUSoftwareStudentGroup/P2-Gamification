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

            IGameAPI gameAPI = new JQueryGameAPI();

            GameModel gameModel;
            GameView gameView;

            gameAPI.GetCurrentPlayer((u) =>
            {
                u.AddCategory(new LevelCategory("wat") { new Level("4/3+0", "4/2+0", 0, new string[] { "4/2" }) });
                gameAPI.GetPlayers((p) =>
                {
                    gameModel = new GameModel(u, p)
                    {
                        OnSaveLevel = (level) => 
                        gameAPI.SaveUserLevelProgress
                        (
                            level.LevelID,
                            level.CurrentExpression,
                            level.Stars,
                            (success) => Console.WriteLine(success)
                        )
                    };
                    gameView = new GameView(gameModel, context);
                });
            });
        }
    }
}