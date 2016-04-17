using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ThreeOneSevenBee.Model.Game;
using ThreeOneSevenBee.Model.UI;
using System.Net;
using System.IO;
using System.Linq;
using Android.Util;
using Org.Json;
using System.Collections.Generic;

namespace ThreeOneSevenBee.AndroidFrontend
{
    [Activity(Label = "ThreeOneSevenBee.AndroidFrontend", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            AndroidContext context = new AndroidContext(this, Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);

            IGameAPI gameAPI = new AndroidGameAPI();

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

                    gameView = new GameView(gameModel, context);
                });
            });

            RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            

            SetContentView(context);
            
        }
    }
}

