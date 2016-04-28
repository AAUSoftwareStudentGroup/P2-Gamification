using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Game;

namespace ThreeOneSevenBee.Model.UI
{
    public class PlayerListView : CompositeView
    {
        public FrameView frameView;
        public Dictionary<BadgeName, string> badgeDictionary = new Dictionary<BadgeName, string>()
        {
            {BadgeName.brokBadge, "brøkbadge.png"},
            {BadgeName.masterOfAlgebra, "master_of_algebrabadge.png"},
            {BadgeName.potensBadge, "potens_badge.png"},
            {BadgeName.tutorialBadge, "tutorial_badge.png" },
            {BadgeName.parenthesisBadge, "parenthesis_badge.png"}

        };
        public void Build(IEnumerable<Player> players)
        {
            Children = new List<View>();
            int offsetY = 5;

            foreach (Player player in players.Take(10))
            {
                int badgesWidth = (badgeDictionary.Count - 1) * 15;
                double labelWidth = Width - badgesWidth - 25;
                CompositeView row = new CompositeView(Width, 20)
                    {
                        new LabelView(" " + player.PlayerName + " ")
                        {
                            Width = labelWidth,
                            Height = 10,
                            Align = TextAlignment.Left,
                            Y = 5
                        },
                    };
                double spacing = 0;
                foreach (var badge in player.Badges)
                {
                    row.Add(new ImageView(badgeDictionary[badge], 15, 15)
                    {
                        X = labelWidth + spacing,
                        Y = 2.5
                    });
                    spacing += 15;
                }
                frameView = new FrameView(Width, 20, row);

                row.X = 5;
                row.Y = offsetY;
                row.Width = Width - 10;
                row.BackgroundColor = new Color(239, 239, 239);
                Children.Add(row);

                offsetY += 25;
            }
        }

        public void Update(IEnumerable<Player> players)
        {
            Build(players);
        }

        public PlayerListView(IEnumerable<Player> players, double width, double height) : base(width, height)
        {
            Build(players);
            BackgroundColor = new Color(209, 209, 209);
        }

        public PlayerListView(double width, double height) : base(width, height)
        {

        }
    }
}
