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
            gameAPI.IsAuthenticated((isAuthenticated) =>
            {
                if(isAuthenticated == false)
                {
                    LoginView loginView = new LoginView(context.Width, context.Height);
                    context.SetContentView(loginView);
                    loginView.OnLogin = (username, password) =>
                    {
                        gameAPI.Authenticate(username, password, (authenticateSuccess) =>
                        {
                            if (authenticateSuccess)
                            {
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
                                                level.Stars,
                                                (IsSaved) => Console.WriteLine(IsSaved ? "Level saved" : "Could not save")
                                            ),
                                            OnBadgeAchieved = (badge) =>
                                            gameAPI.UserAddBadge(
                                                badge,
                                                (IsAdded) => Console.WriteLine(IsAdded ? "Badge added" : "Badge not added")
                                            )
                                        };
                                        gameView = new GameView(gameModel, context);
                                    });
                                });
                            }
                            else
                            {
                                loginView.ShowLoginError();
                            }
                        });
                    };
                    loginView.OnLogin("Morten RaskRask", "adminadmin");
                }
                else
                {
                    // Already authenticated dont show login view code here...
                }
            });
        }
    }
}