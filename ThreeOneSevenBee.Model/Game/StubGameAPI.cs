using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if BRIDGE
using Bridge.Html5;
#endif

namespace ThreeOneSevenBee.Model.Game
{
    public class StubGameAPI : GameAPI
    {
        CurrentPlayer currentPlayer;

        public override bool Ready
        {
            get
            {
                return true;
            }
        }

        public StubGameAPI()
        {
            currentPlayer = new CurrentPlayer("Morten");
            currentPlayer.AddCategory(new LevelCategory("Numbers") {
                            new Level(0, 0, "a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
                            new Level(1, 0, "4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
                            new Level(2, 0, "4+5*5", "4+10*5", "4+50", "54"),
                            new Level(3, 0, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                });
            currentPlayer.AddCategory(new LevelCategory("Variables") {
                            new Level(0, 1, "a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
                            new Level(1, 1, "4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
                            new Level(2, 1, "4+5*5", "4+10*5", "4+50", "54"),
                            new Level(3, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(4, 1, "a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
                            new Level(5, 1, "4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
                            new Level(6, 1, "4+5*5", "4+10*5", "4+50", "54"),
                            new Level(7, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(8, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(9, 1, "a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
                            new Level(10, 1, "4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
                            new Level(11, 1, "4+5*5", "4+10*5", "4+50", "54"),
                            new Level(12, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(13, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(14, 1, "a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
                            new Level(15, 1, "4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
                            new Level(16, 1, "4+5*5", "4+10*5", "4+50", "54"),
                            new Level(17, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(18, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(19, 1, "a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
                            new Level(20, 1, "4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
                            new Level(21, 1, "4+5*5", "4+10*5", "4+50", "54"),
                            new Level(22, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(23, 1, "a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
                            new Level(24, 1, "4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
                            new Level(25, 1, "4+5*5", "4+10*5", "4+50", "54"),
                            new Level(26, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(27, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(28, 1, "a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
                            new Level(29, 1, "4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
                            new Level(30, 1, "4+5*5", "4+10*5", "4+50", "54"),
                            new Level(31, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(32, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(33, 1, "4+5*5", "4+10*5", "4+50", "54"),
                            new Level(34, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                            new Level(35, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                        });
            currentPlayer.Badges = new List<string> { "FractionBadge", "ExponentBadge" };
            currentPlayer.CurrentCategory = 0;
            currentPlayer.CurrentLevel = 0;
        }

        public override void GetCurrentPlayer(Action<CurrentPlayer> callback)
        {
            callback(currentPlayer);
        }

        public override void GetPlayers(Action<List<Player>> callback)
        {
            callback(new List<Player>() {
                currentPlayer,
                new Player("Anton"),
                new Player("Christian"),
                new Player("Lasse"),
                new Player("Mathias P."),
                new Player("Mathias I."),
                new Player("Nikolaj")
            });
        }

        public override void UpdateCurrentPlayer(CurrentPlayer currentPlayer)
        {
            this.currentPlayer = currentPlayer;
        }
    }
}
