using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Game;
namespace ThreeOneSevenBee.Model.UI
{
    public class TitleView : CompositeView
    {
        public LabelView WelcomeText;
        public ImageView PlayButton;
        public ImageView LevelButton;
        public PlayerListView PlayerList;
        public TitleView(CurrentPlayer user, IEnumerable<Player> players) : base(600, 300)
        {
            BackgroundColor = new Color(255, 255, 255);
            WelcomeText = new LabelView("Velkommen " + user.PlayerName)
            {
                X = 100,
                Y = 50,
                Height = 50,
                Width = 220
            };
            PlayButton = new ImageView("playbutton.png", 100, 100)
            {
                X = 100,
                Y = 100,
                BackgroundColor = new Color(39, 174, 97)
            };
            LevelButton = new ImageView("levelbutton.png", 100, 100)
            {
                X = 220,
                Y = 100,
                BackgroundColor = new Color(42, 128, 185)
            };
            PlayerList = new PlayerListView(players, 160, 200)
            {
                X = 340,
                Y = 50
            };
            Children = new List<View>(){
                WelcomeText,
                PlayButton,
                LevelButton,
                PlayerList
            };
        }
    }
}
