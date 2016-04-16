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
        public CompositeView LevelButton;
        public CompositeView PlayButton;
        public PlayerListView PlayerList;
        public TitleView(CurrentPlayer user, IEnumerable<Player> players) : base(600, 300)
        {
            WelcomeText = new LabelView("Velkommen " + user.PlayerName)
            {
                X = 100,
                Y = 50,
                Align = "left",
                FontSize = 20,
                Font = "Segoe UI",
                Height = 50,
                Width = 220
            };

            VectorImageView playIcon = new VectorImageView(0, 0, 100, 100)
            {
                { 25,25 },
                { 75,50 },
                { 25,75 }
            };
            playIcon.BackgroundColor = "white";
            PlayButton = new CompositeView(100, 100)
            {
                playIcon,
            };
            PlayButton.BackgroundColor = "#27AE61";
            PlayButton.X = 100;
            PlayButton.Y = 100;

            VectorImageView levelIcon1 = new VectorImageView(0, 0, 100, 100)
            {
                { 20,20 },
                { 45,20 },
                { 45,45 },
                { 20,45 },
                { 20,20 },
            };
            VectorImageView levelIcon2 = new VectorImageView(0, 0, 100, 100)
            {
                { 80,20 },
                { 80,45 },
                { 55,45 },
                { 55,20 },
                { 80,20 },
            };
            VectorImageView levelIcon3 = new VectorImageView(0, 0, 100, 100)
            {
                { 80,80 },
                { 55,80 },
                { 55,55 },
                { 80,55 },
                { 80,80 },
            };
            VectorImageView levelIcon4 = new VectorImageView(0,0, 100, 100)
            {
                { 20,80 },
                { 20,55 },
                { 45,55 },
                { 45,80 },
                { 20,80 },
            };
            levelIcon1.BackgroundColor = "white";
            levelIcon2.BackgroundColor = "white";
            levelIcon3.BackgroundColor = "white";
            levelIcon4.BackgroundColor = "white";
            LevelButton = new CompositeView(100, 100)
            {
                levelIcon1,
                levelIcon2,
                levelIcon3,
                levelIcon4,
            };
            LevelButton.BackgroundColor = "#2A80B9";
            LevelButton.X = 220;
            LevelButton.Y = 100;

            PlayerList = new PlayerListView(players, 160, 200)
            {
                X = 340,
                Y = 50
            };
            Children = new List<View>(){
                WelcomeText,
                LevelButton,
                PlayerList,
                PlayButton
            };
        }
    }
}
