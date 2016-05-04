#if BRIDGE
using Bridge.Html5;
#endif
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
        public ButtonView LogoutButton;
        public Action OnLogout;
        public CompositeView BadgesView;
        public LabelView BadgeInfoText;
        public ImageView ShowBadges;
        public PlayerListView PlayerView = new PlayerListView(1, 1);

        public void Build(GameModel game)
        {
            CurrentPlayer user = game.User;
            IEnumerable<Player> players = game.Players;
            BackgroundColor = new Color(255, 255, 255);
            WelcomeText = new LabelView("Velkommen " + user.PlayerName)
            {
                X = 10,
                Y = 10,
                Height = 50,
                Width = 220
            };

            BadgesView = new CompositeView(220, 40)
            {
                X = WelcomeText.X,
                Y = WelcomeText.Y + WelcomeText.Height
            };

            BadgeInfoText = new LabelView("Badges: ")
            {
                Width = 100,
                Height = 20,
                X = -10
            };

            BadgesView.Add(BadgeInfoText);

            if (user.Badges != null)
            {
                // start spacing half the width of a badge
                int spacing = -10;
                foreach (BadgeName badge in user.Badges)
                {
                    if (PlayerView.badgeDictionary.ContainsKey(badge))
                    {
                        BadgesView.Add(new ImageView(PlayerView.badgeDictionary[badge], 25, 25)
                        {
                            X = BadgeInfoText.X + BadgeInfoText.Width + spacing,
                        });
                    }
                    spacing += 30;
                }
            }

            VectorImageView playIcon = new VectorImageView(0, 0, 100, 100)
            {
                { 25,25 },
                { 75,50 },
                { 25,75 }
            };

            LogoutButton = new ButtonView("Log ud", () =>
            {
                if (OnLogout != null)
                {
                    OnLogout();
                }
            })
            {
                Width = 50,
                Height = 15,
                TextColor = new Color(255, 255, 255),
                BackgroundColor = new Color(193, 57, 43)
            };

            playIcon.BackgroundColor = new Color(255, 255, 255);
            PlayButton = new CompositeView(100, 100)
            {
                playIcon,
            };
            PlayButton.BackgroundColor = new Color(39, 174, 97);
            PlayButton.X = 10;
            PlayButton.Y = BadgesView.Y + BadgesView.Height;
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
            VectorImageView levelIcon4 = new VectorImageView(0, 0, 100, 100)
            {
                { 20,80 },
                { 20,55 },
                { 45,55 },
                { 45,80 },
                { 20,80 },
            };
            levelIcon1.BackgroundColor = new Color(255, 255, 255);
            levelIcon2.BackgroundColor = new Color(255, 255, 255);
            levelIcon3.BackgroundColor = new Color(255, 255, 255);
            levelIcon4.BackgroundColor = new Color(255, 255, 255);

            LevelButton = new CompositeView(100, 100)
            {
                levelIcon1,
                levelIcon2,
                levelIcon3,
                levelIcon4,
            };
            LevelButton.BackgroundColor = new Color(42, 128, 185);
            LevelButton.X = PlayButton.X + PlayButton.Width + 20;
            LevelButton.Y = BadgesView.Y + BadgesView.Height;

            PlayerList = new PlayerListView(players, 160, 200)
            {
                X = LevelButton.X + LevelButton.Width + 20,
                Y = WelcomeText.Y
            };

            Children = new List<View>(){
                LogoutButton,
                WelcomeText,
                LevelButton,
                PlayerList,
                PlayButton,
                BadgesView
            };
        }

        public TitleView(GameModel game) : base(420, 220)
        {
            Build(game);
        }
    }
}
