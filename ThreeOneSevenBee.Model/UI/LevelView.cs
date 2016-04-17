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
                BackgroundColor = new Color(192, 57, 43),
                TextColor = new Color(255, 255, 255),
            };

            nextButton = new ButtonView("Næste", () => OnNextLevel())
            {
                X = Width - 100,
                Width = 100,
                Height = 50,
                BackgroundColor = game.IsLevelCompleted ? new Color(22, 160, 134) : new Color(190, 190, 190),
                TextColor = new Color(255, 255, 255),
            };

            progressbar = new ProgressbarStarView(game.ProgressBar, Width - 220, 30)
            {
                X = 110,
                Y = 10
            };

            identityMenu = new IdentityMenuView(game.ExprModel, Width, 100)
            {
                Y = Height - 125
            };

            expression = new ExpressionView(game.ExprModel, Width, Height - 175, 8)
            {
                X = 0,
                Y = 50,
            };

            toolTipView = new ToolTipView("Denne bar viser hvor langt du er nået.")
            {
                Visible = game.IsFirstLevel,
                TextColor = new Color(255, 255, 255),
                X = progressbar.X,
                Y = progressbar.Y + progressbar.Height + 10,
                Width = 400,
                Height = 75,
                BackgroundColor = new Color(40, 120, 130),
                Position = position.upperLeft
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
            nextButton.BackgroundColor = game.IsLevelCompleted ? new Color(40, 120, 130) : new Color(190, 190, 190);
            toolTipView.Visible = game.IsFirstLevel;

            if (OnChanged != null)
            {
                OnChanged();
            }
        }

        public LevelView(GameModel game, double width, double height) : base(width, height)
        {
            BackgroundColor = new Color(255, 255, 255);
            Build(game);
        }
    }
}
