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
        ToolTipView toolTipView2;
        ToolTipView toolTipView3;

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

            toolTipView = new ToolTipView("Denne bar viser hvor langt du er nået.")
            {
                FontSize = 15,
                Visible = game.IsFirstLevel,
                FontColor = "#ffffff",
                X = progressbar.X,
                Y = progressbar.Y + progressbar.Height + 10,
                Width = 300,
                Height = 75,
                BackgroundColor = "#297782",
                Position = position.upperLeft
            };

            toolTipView2 = new ToolTipView("Når knappen bliver grøn kan du gå videre til næste bane")
            {
                FontSize = 15,
                Visible = game.IsFirstLevel,
                FontColor = "#ffffff",
                X = nextButton.X,
                Y = nextButton.Y + nextButton.Height + 10,
                Width = 400,
                Height = 75,
                BackgroundColor = "#297782",
                Position = position.upperRight
            };

            toolTipView3 = new ToolTipView("Dit mål er at reducere ovenstående udtryk. Dette gøres ved at markere de dele i udtrykket som skal reduceres.")
            {
                Description = "Markér [a] og [a]. Klik derefter på den ønskede omskrivning nedenfor for at reducere udtrykket",
                FontSize = 15,
                Visible = game.IsFirstLevel,
                FontColor = "#ffffff",
                X = Width / 2 - 400,
                Y = Height / 2 + 30,
                Width = 800,
                Height = 90,
                BackgroundColor = "#297782",
                Position = position.bottomMiddle
                
            };

            Children = new List<View>()
            {
                menuButton,
                nextButton,
                progressbar,
                identityMenu,
                expression,
                toolTipView,
                toolTipView2,
                toolTipView3
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
            toolTipView2.Visible = game.IsFirstLevel;

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
