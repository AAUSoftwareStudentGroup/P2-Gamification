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
        Context context;

        public void Update(GameModel game)
        {
            levelView.Update(game);

            context.Draw();
        }

        public GameView(GameModel game, Context context) : base(context.Width, context.Height)
        {
            this.context = context;

            titleView = new TitleView(game.Players);

            levelView = new LevelView(game, context.Width, context.Height)
            {
                OnExit = () =>
                {
                    game.Save();
                    setContent(titleView);
                },
                OnNextLevel = () =>
                {
                    game.Save();
                    game.NextLevel();
                }
                
            };

            titleView.PlayButton.OnClick = () => setContent(levelView);

            titleView.LevelButton.OnClick = () =>
            {
                game.SetLevel(int.Parse(Console.ReadLine()) - 1, 0); setContent(levelView);
            };

            game.OnChanged = Update;

            setContent(titleView);
        }

        public override void setContent(View content)
        {
            base.setContent(content);
            context.SetContentView(Content);
        }
    }
}
