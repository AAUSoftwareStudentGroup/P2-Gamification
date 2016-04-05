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

            StubGameAPI gameAPI = new JQueryGameAPI();

            GameModel gameModel;
            GameView gameView;

            gameAPI.GetCurrentPlayer((u) =>
            {
                Console.WriteLine("user loaded");
                gameAPI.GetPlayers((p) =>
                {
                    Console.WriteLine("players loaded");
                    gameModel = new GameModel(u, p);
                    gameView = new GameView(gameModel, context);
                });

            });

            

        }
    }
}