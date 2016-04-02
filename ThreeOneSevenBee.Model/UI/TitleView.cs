using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Game;
namespace ThreeOneSevenBee.Model.UI
{
    public class TitleView : CompositeView
    {
        public ImageView PlayButton;
        public ImageView LevelButton;
        public PlayerListView PlayerList;
        public TitleView(IEnumerable<Player> players) : base(600, 300)
        {
            PlayButton = new ImageView("playbutton.png", 100, 100) { X = 100, Y = 100, BackgroundColor = "#27AE61" };
            LevelButton = new ImageView("levelbutton.png", 100, 100) { X = 220, Y = 100, BackgroundColor = "#2A80B9" };
            PlayerList = new PlayerListView(players, 160, 200) { X = 340, Y = 50 };
            Children = new List<View>(){
                PlayButton,
                LevelButton,
                PlayerList
            };
        }
    }
}
