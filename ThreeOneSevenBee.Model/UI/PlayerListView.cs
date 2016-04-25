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
                Children.Add(
                    new CompositeView(Width, Height)
                    {
                        new LabelView(player.PlayerName)
                        {
                            X = 0,
                            Y = offsetY,
                            Width = Width - 20,
                            Height = 20,
                            BackgroundColor = new Color(239, 239, 239),
                        },
                        new ImageView("spildonebadge.png", 20, 20)
                        {
                            X = Width - 20
                        }
                    });
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
