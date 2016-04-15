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
    public class LevelView : CompositeView
    {
        public Action OnExit { get; set; }
        public Action OnNextLevel { get; set; }

        ButtonView menuButton;
        ButtonView nextButton;
        ProgressbarStarView progressbar;
        IdentityMenuView identityMenu;
        ExpressionView expression;
        ToolTipView toolTipView;

        public virtual void Build(GameModel game)
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

            nextButton = new ButtonView("Næste", () => OnNextLevel())
            {
                X = Width - 100,
                Width = 100,
                Height = 50,
                BackgroundColor = game.IsLevelCompleted ? "#16A086" : "#BEC3C7",
                FontColor = "#FFFFFF",
                Font = "Segoe UI",
                FontSize = 25
            };

            progressbar = new ProgressbarStarView(game.ProgressBar, Width - 220, 30)
            {
                X = 110,
                Y = 10
            };

            identityMenu = new IdentityMenuView(game.ExprModel, Width, 100)
            {
                Y = Height - 100
            };

            expression = new ExpressionView(game.ExprModel, Width, Height - 150, 4)
            {
                X = 0,
                Y = 50,
            };

            toolTipView = new ToolTipView("Dette er en progressbar")
            {
                X = progressbar.X,
                Y = progressbar.Y,
                Width = 100,
                Height = 20,
                Visible = game.IsFirstLevel
            };

            Children = new List<View>()
            {
                menuButton,
                nextButton,
                progressbar,
                identityMenu,
                expression,
                toolTipView
            };

            if(OnChanged != null)
            {
                OnChanged();
            }
        }

        public virtual void Update(GameModel game)
        {
            progressbar.Update(game.ProgressBar);
            identityMenu.Update(game.ExprModel.Identities, game.ExprModel);
            expression.Update(game.ExprModel);
            nextButton.BackgroundColor = game.IsLevelCompleted ? "#16A086" : "#BEC3C7";
            toolTipView.Visible = game.IsFirstLevel;

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
