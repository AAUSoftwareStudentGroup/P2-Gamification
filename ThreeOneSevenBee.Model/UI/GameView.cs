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
    /// <summary>
    /// GameView is a visual representation of the data in GameModel
    /// </summary>
    public class GameView : FrameView
    {
        TitleView titleView;
        LevelView levelView;
        CategoryCompletionView categoryCompletionView;
        LevelSelectView levelSelectView;

        public Action OnExit { get; set; }

        public Action ReloadGame { get; set; }

        public void Update(GameModel game)
        {
            levelView.Update(game);
            levelSelectView.Update(game.User);
            if(OnChanged != null)
            {
                OnChanged();
            }
        }

        public GameView(GameModel game, double width, double height) : base(width, height)
        {
            categoryCompletionView = null;

            game.OnCategoryCompleted += (c) =>
            {
                categoryCompletionView = new CategoryCompletionView(c, game.IsLastCategory)
                {
                    OnNext = () => 
                    {
                        if (game.IsLastCategory == false)
                        {
                            game.SetLevel(0, c.categoryIndex + 1);
                            SetContent(levelView);
                        }
                    },
                    OnExit = () => ReloadGame(),
                };
                SetContent(categoryCompletionView);
            };

            BackgroundColor = new Color(255, 255, 255);

            titleView = new TitleView(game);

            levelView = new LevelView(game)
            {
                OnExit = () =>
                {
                    game.SaveLevel();
                    SetContent(levelSelectView);
                },
                OnNextLevel = () =>
                {
                    if (game.IsLastLevel == false)
                    {
                        game.SaveLevel();
                        game.NextLevel();
                    }
                }
            };

            levelSelectView = new LevelSelectView(game.User)
            {
                OnChanged = () => {
                    SetContent(levelSelectView);
                    Update(game);
                    levelSelectView.Update(game.User);
                },
                OnLevelSelect = (level) =>
                {
                    if (level.Unlocked)
                    {
                        SetContent(levelView);
                        Update(game);
                        game.SetLevel(level.LevelIndex, level.CategoryIndex);
                    }
                },
                OnExit = () => ReloadGame()
            };

            titleView.PlayButton.OnClick = () => SetContent(levelView);

            titleView.LevelButton.OnClick = () => SetContent(levelSelectView);

            titleView.OnLogout = () =>
            {
                if (OnExit != null)
                {
                    OnExit();
                }
            };

            game.OnChanged = Update;

			SetContent(titleView);
        }

        public override void SetContent(View content)
        {
            base.SetContent(content);
            if(OnChanged != null)
            {
                OnChanged();
            }
        }
    }
}
