using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Game;

namespace ThreeOneSevenBee.Model.UI
{
    public class PlayerListView : CompositeView
    {
        public void Build(IEnumerable<Player> players)
        {
            Children = new List<View>();
            int offsetY = 5;
            foreach (Player player in players)
            {
                View row = new CompositeView(Width, 20)
                    {
                        new LabelView(player.PlayerName)
                        {
                            Width = Width - 30,
                            Height = 20,
                        },
                        new ImageView("spildonebadge.png", 20, 20)
                        {
                            X = Width - 30
                        }
                    };
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
    }
}
