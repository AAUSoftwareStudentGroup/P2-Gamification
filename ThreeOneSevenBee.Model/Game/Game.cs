﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.UI;
#if BRIDGE
using Bridge.Html5;
#endif

namespace ThreeOneSevenBee.Model.Game
{
    public class Game
    {
        IGameAPI gameAPI;
        IContext context;
        GameModel gameModel;
        GameView gameView;

        public Game(IContext context, IGameAPI gameAPI)
        {
            this.gameAPI = gameAPI;
            this.context = context;
        }

        public void Start()
        {
            gameAPI.IsAuthenticated((isAuthenticated) =>
            {
                if (isAuthenticated == false)
                {
                    LoginView loginView = new LoginView(context.Width, context.Height);
                    context.SetContentView(loginView);
                    loginView.OnLogin = (username, password) =>
                    {
                        gameAPI.Authenticate(username, password, (authenticateSuccess) =>
                        {
                            if (authenticateSuccess)
                            {
                                loadGameData();
                            }
                            else
                            {
                                loginView.ShowLoginError();
                            }
                        });
                    };
                }
                else
                {
                    loadGameData();
                }
            });
        }

        private void loadGameData()
        {
            gameAPI.GetCurrentPlayer((u) =>
            {
                bool unlocked = true;
                for (int index = 0; index < u.Categories.Count; index++)
                {
                    for (int i = 0; i < u.Categories[index].Count; i++)
                    {
                        u.Categories[index][i].Unlocked = unlocked;
                        if(u.Categories[index][i].Stars == 0 && unlocked == true)
                        {
                            unlocked = false;
                            u.CurrentCategoryIndex = index;
                            u.CurrentLevelIndex = 0;
                        }
                    }
                }
                gameAPI.GetPlayers((p) =>
                {
                    gameModel = new GameModel(u, p)
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
                        OnBadgeAchieved = (badge) =>
                            gameAPI.UserAddBadge(
                                badge,
                                (IsAdded) => Console.WriteLine(IsAdded ? "Badge added" : "Badge not added")
                            )
                    };

                    gameView = new GameView(gameModel, context.Width, context.Height)
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
