using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.UI;

using ThreeOneSevenBee.Model.Expression;
#if BRIDGE
using Bridge.Html5;
#endif

namespace ThreeOneSevenBee.Model.Game
{
    public class Game
    {
        IGameAPI gameAPI;
        IContext context;

        public Game(IContext context, IGameAPI gameAPI)
        {
            this.gameAPI = gameAPI;
            this.context = context;
        }

        /// <summary>
        /// Starts the game, making sure the user is authenticated and if so, loads up the game.
        /// </summary>
        public void Start()
        {
            // first we check if we are authenticated with the server
            gameAPI.IsAuthenticated((isAuthenticated) =>
            {
                if (isAuthenticated == false)
                {
                    // if we are not, we display the login view, and get the user to connect
                    LoginView loginView = new LoginView(context.Width, context.Height);
                    context.SetContentView(loginView);
                    loginView.OnLogin = (username, password) =>
                    {
                        gameAPI.Authenticate(username, password, (authenticateSuccess) =>
                        {
                            if (authenticateSuccess)
                            {
                                // on succesful login, we load up the game data
                                loadGameData();
                            }
                            else
                            {
                                loginView.ShowLoginError();
                                context.Draw();
                            }
                        });
                    };
                }
                else
                {
                    // if we are, we load up the game data
                    loadGameData();
                }
            });
        }

        private void loadGameData()
        {
            // TODO: Needs comments.
            gameAPI.GetCurrentPlayer((u) =>
            {
                bool unlocked = true;
                for (int index = 0; index < user.Categories.Count; index++)
                {
                    for (int i = 0; i < user.Categories[index].Count; i++)
                    {
                        user.Categories[index][i].Unlocked = unlocked;
                        if(user.Categories[index][i].Stars == 0 && unlocked == true)
                        {
                            unlocked = false;
                            user.CurrentCategoryIndex = index;
                            user.CurrentLevelIndex = 0;
                        }
                    }
                }
                gameAPI.GetPlayers((players) =>
                {
                    GameModel gameModel = new GameModel(user, players)
                    {
                        OnSaveLevel = (level) =>
                        {
                            gameAPI.SaveUserLevelProgress
                            (
                                level.LevelID,
                                level.CurrentExpression,
                                level.Stars,
                                (IsSaved) => 
                                {
                                    Console.WriteLine(IsSaved ? "Level saved" : "Could not save");
                                }
                            );
                        },
                        OnCategoryCompleted = (completedCategory) =>
                            gameAPI.UserAddBadge(
                                completedCategory.Badge,
                                (IsAdded) => Console.WriteLine(IsAdded ? "Badge added" : "Badge not added")
                            )
                    };

                    GameView gameView = new GameView(gameModel, context.Width, context.Height)
                    {
                        OnExit = () =>
                        {
                            gameAPI.logout((success) =>
                            {
                                Console.WriteLine(success ? "Logout success" : "Logout failed");
                                Start();
                            });
                        },
                        ReloadGame = () => loadGameData()
                    };
                    context.SetContentView(gameView);
                });
            });
        }
    }
}
