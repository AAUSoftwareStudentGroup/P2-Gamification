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
        /// <summary>
        ///  Builds the view necessary to play a leve
        /// </summary>
        public Action OnExit { get; set; }
        public Action OnNextLevel { get; set; }

        ButtonView menuButton;
        ButtonView nextButton;
        ImageView restartButton;
        ButtonView helpButton;
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
                BackgroundColor = game.IsLastLevel || game.IsLevelCompleted == false ? new Color(190, 190, 190) : new Color(40, 120, 130),
                TextColor = new Color(255, 255, 255),
            };

            restartButton = new ImageView("restart.png", 50, 50)
            {
                OnClick = () => game.RestartLevel(),
                X = 110,
                Y = 0,
                BackgroundColor = new Color(192, 57, 43)
            };

            toolTipView = new ToolTipView(game.User.CurrentLevel.Description, 300, 100)
            {
                X = Width - 300,
                Y = 50,
                Visible = false,
                
                OnClick = () =>
                {
                    if (helpButton.OnClick != null && toolTipView.Visible == true)
                    {
                        helpButton.OnClick();
                    }
                }
            };
            toolTipView.ArrowPosition = 155;

            helpButton = new ButtonView(
                "?",
                () =>
                {
                    if(game.User.CurrentLevel.Description != "")
                    {
                        if(toolTipView.Visible == true)
                        {
                            helpButton.Text = "?";
                            toolTipView.Visible = false;
                        }
                        else
                        {
                            helpButton.Text = " Luk ";
                            toolTipView.Visible = true;
                        }
                    }
                }
            )
            {
                X = Width - 160,
                Y = 0,
                Width = 50,
                Height = 50,
                BackgroundColor = game.User.CurrentLevel.Description == "" ? new Color(190, 190, 190) : new Color(40, 120, 130),
                TextColor = new Color(255, 255, 255),
            };

            progressbar = new ProgressbarStarView(game.ProgressBar, Width - 340, 30)
            {
                X = 170,
                Y = 10
            };

            identityMenu = new IdentityMenuView(game.ExprModel, Width, 100)
            {
                Y = Height - 125
            };

            expression = new ExpressionView(game.ExprModel, Width - 100, Height - 275, 8)
            {
                X = 50,
                Y = 100,
            };

            Children = new List<View>()
            {
                menuButton,
                nextButton,
                restartButton,
                helpButton,
                progressbar,
                identityMenu,
                expression,
                toolTipView,
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
            nextButton.BackgroundColor = game.IsLastLevel || game.IsLevelCompleted == false ? new Color(190, 190, 190) : new Color(40, 120, 130);
            helpButton.BackgroundColor = game.User.CurrentLevel.Description == "" ? new Color(190, 190, 190) : new Color(40, 120, 130);
            toolTipView.Text = game.User.CurrentLevel.Description;
            toolTipView.Visible = false;
            helpButton.Text = "?";
            if (OnChanged != null)
            {
                OnChanged();
            }
        }

        public LevelView(GameModel game) : base(700, 400)
        {
            BackgroundColor = new Color(255, 255, 255);
            Build(game);
        }
    }
}
