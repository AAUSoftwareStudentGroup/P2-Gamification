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
    public class GameView : FrameView
    {
        TitleView titleView;
        LevelView levelView;
        LevelSelectView levelSelectView;

        public Action OnExit { get; set; }

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
            BackgroundColor = new Color(255, 255, 255);

            titleView = new TitleView(game);

            levelView = new LevelView(game)
            {
                OnExit = () =>
                {
                    game.SaveLevel();
                    setContent(levelSelectView);
                },
                OnNextLevel = () =>
                {
                    game.SaveLevel();
                    game.NextLevel();
                }
            };

            levelSelectView = new LevelSelectView(game.User)
            {
                OnChanged = () => {
                    setContent(levelSelectView);
                    Update(game);
                    levelSelectView.Update(game.User);
                },
                OnLevelSelect = (level) =>
                {
                    setContent(levelView);
                    Update(game);
                    game.SetLevel(level.LevelIndex, level.CategoryIndex);
                },
                OnExit = () => setContent(titleView)
            };

            titleView.PlayButton.OnClick = () => setContent(levelView);

            titleView.LevelButton.OnClick = () => setContent(levelSelectView);

            titleView.OnLogout = () =>
            {
                if (OnExit != null)
                {
                    OnExit();
                }
            };

            game.OnChanged = Update;

			setContent(titleView);
        }

        public override void setContent(View content)
        {
            base.setContent(content);
            if(OnChanged != null)
            {
                OnChanged();
            }
        }
    }
}
