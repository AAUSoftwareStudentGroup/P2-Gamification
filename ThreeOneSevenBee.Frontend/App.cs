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

            LevelCategory testCategory = new LevelCategory("test");
            testCategory.Add(new Level("(a^2)^3", "(a^2)^3", new string[] { "44" }));
            testCategory.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategory.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategory.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategory.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategory.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategory.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategory.Add(new Level("4+44", "4+44", new string[] { "48" }));
            testCategory.Add(new Level("4+44", "4+44", new string[] { "48" }));

            LevelCategory tutorialCategory = new LevelCategory("Tutorial");
            tutorialCategory.Add(new Level("a*a", "a*a", new string[] { "a^2" }));

            GameModel gameModel;
            GameView gameView;

            gameAPI.GetCurrentPlayer((u) =>
            {
                u.AddCategory(tutorialCategory);
                u.AddCategory(testCategory);
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
                    
                    gameView = new GameView(gameModel, context);
                });
            });
        }
    }
}