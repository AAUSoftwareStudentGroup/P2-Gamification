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

        public GameView(GameModel game, Context context) : base(context.Width, context.Height)
        {
            this.context = context;
            titleView = new TitleView();
            levelView = new LevelView(game, context.Width, context.Height)
            {
                OnChanged = () => context.Draw(),
                OnExit = () => setContent(titleView)
            };
            titleView.PlayButton.OnClick = () => setContent(levelView);
            setContent(titleView);
        }

        public override void setContent(View content)
        {
            base.setContent(content);
            context.SetContentView(Content);
        }
    }
}
