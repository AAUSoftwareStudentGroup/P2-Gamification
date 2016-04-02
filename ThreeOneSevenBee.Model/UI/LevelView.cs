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

        ButtonView menuButton;
        ButtonView nextButton;
        ProgressbarStarView progressbar;
        IdentityMenuView identityMenu;
        ExpressionView expression;

        public void Build(GameModel game)
        {
            menuButton = new ButtonView("Menu", () => OnExit())
            {
                Width = 100,
                Height = 50,
                BackgroundColor = "#C1392B",
                FontColor = "#FFFFFF",
                Font = "Segoe UI",
                FontSize = 25
            };

            nextButton = new ButtonView("Næste", () => game.NextLevel())
            {
                X = Width - 100,
                Width = 100,
                Height = 50,
                BackgroundColor = game.LevelCompleted ? "#16A086" : "#BEC3C7",
                FontColor = "#FFFFFF",
                Font = "Segoe UI",
                FontSize = 25
            };

            progressbar = new ProgressbarStarView(game.Progress, Width - 220, 30)
            {
                X = 110,
                Y = 10
            };

            identityMenu = new IdentityMenuView(game.ExprModel, Width, 100)
            {
                Y = Height - 100
            };

            expression = new ExpressionView(game.ExprModel, Width, Height - 150)
            {
                X = 0,
                Y = 50
            };

            Children = new List<View>()
            {
                menuButton,
                nextButton,
                progressbar,
                identityMenu,
                expression
            };

            if(OnChanged != null)
            {
                OnChanged();
            }
        }

        public void Update(GameModel game)
        {
            progressbar.Update(game.Progress);
            identityMenu.Update(game.ExprModel.Identities, game.ExprModel);
            expression.Update(game.ExprModel);

            if (OnChanged != null)
            {
                OnChanged();
            }
        }

        public LevelView(GameModel game, double width, double height) : base(width, height)
        {
            Build(game);
        }
    }
}
