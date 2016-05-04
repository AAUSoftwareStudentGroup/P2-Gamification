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
using Android.Views.InputMethods;

namespace ThreeOneSevenBee.AndroidFrontend
{
    [Activity(Label = "ThreeOneSevenBee.AndroidFrontend", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            Console.WriteLine("wat");
            return base.OnKeyDown(keyCode, e);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);

            InputMethodManager input = (InputMethodManager)GetSystemService(Activity.InputMethodService);

            AndroidContext context = new AndroidContext(this, input, Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);

            IGameAPI gameAPI = new AndroidGameAPI();

            Game game = new Game(context, gameAPI);

            game.Start();

            SetContentView(context);
        }
    }
}

