using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Game;
#if BRIDGE
using Bridge.Html5;
#endif

namespace ThreeOneSevenBee.Model.UI
{
    class LevelView : CompositeView
    {
        public Action OnExit { get; set; }
        public Action OnNextLevel { get; set; }

        public void Build(GameModel game)
        {
            Console.WriteLine("Building LevelView..." + " Progress: " + game.Progress.Progress + "...");
            Children = new List<View>()
            {
                new ButtonView("Tilbage", () => OnExit())
                {
                    Width = 100,
                    Height = 50,
                    BackgroundColor = "#C1392B",
                    FontColor = "#FFFFFF",
                    Font = "Segoe UI",
                    FontSize = 25
                },
                new ButtonView("Næste", () => game.NextLevel())
                {
                    X = Width - 100,
                    Width = 100,
                    Height = 50,
                    BackgroundColor = game.LevelCompleted ? "#16A086" : "#BEC3C7",
                    FontColor = "#FFFFFF",
                    Font = "Segoe UI",
                    FontSize = 25
                },
                new ProgressbarStarView(game.Progress, Width - 220, 30)
                {
                    X = 110,
                    Y = 10
                },
                new IdentityMenuView(game.ExprModel, Width, 100)
                {
                    Y = Height - 100
                },
                new ExpressionView(game.ExprModel, Width, Height - 150)
                {
                    OnChanged = () => Build(game),
                    X = 0,
                    Y = 50
                }
            };
            if(OnChanged != null)
            {
                OnChanged();
            }
        }

        public LevelView(GameModel game, double width, double height) : base(width, height)
        {
            game.OnChanged = Build;
            Build(game);
        }
    }
}
