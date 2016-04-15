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
        Context context;

        public void Update(GameModel game)
        {
            levelView.Update(game);
            levelSelectView.Update(game.User);
            context.Draw();
        }

        public GameView(GameModel game, Context context) : base(context.Width, context.Height)
        {
            this.context = context;

            titleView = new TitleView(game.User, game.Players);

            levelView = new LevelView(game, context.Width, context.Height)
            {
                OnExit = () =>
                {
                    game.SaveLevel();
                    setContent(titleView);
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

            game.OnChanged = Update;

            setContent(levelView);
        }

        public override void setContent(View content)
        {
            base.setContent(content);
            context.SetContentView(Content);
        }
    }
}
