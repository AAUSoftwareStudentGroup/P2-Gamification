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

            CanvasContext context = new CanvasContext(canvas);

            JQueryGameAPI gameAPI = new JQueryGameAPI();

            LevelCategory testCategories = new LevelCategory("test");
            testCategories.Add(new Level("-4-40+5-9", "-4-40+5-9", new string[] { "44" }));
            testCategories.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategories.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategories.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategories.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategories.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategories.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategories.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategories.Add(new Level("4+44", "4+44", new string[] { "48" }));

            GameModel gameModel;
            GameView gameView;

            gameAPI.GetCurrentPlayer((u) =>
            {
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
                    gameModel.User.AddCategory(testCategories);
                    gameView = new GameView(gameModel, context);
                });
            });
        }
    }
}