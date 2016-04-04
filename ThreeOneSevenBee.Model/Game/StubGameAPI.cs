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
            currentPlayer.Badges = new List<string> { "FractionBadge", "ExponentBadge" };
            currentPlayer.CurrentCategory = 0;
            currentPlayer.CurrentLevel = 0;
            currentPlayer.Categories = new List<LevelCategory>() {
                new LevelCategory("Numbers") {
                    Levels = new List<Level>()
                        {
                            new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
                            new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
                            new Level("4+5*5", "4+10*5", "4+50", "54"),
                            new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
                        }
                }
            };
        }

        public override CurrentPlayer GetCurrentPlayer()
        {
            return currentPlayer;
        }

        public override List<Player> GetPlayers()
        {
            return new List<Player>() {
                currentPlayer,
                new Player("Anton"),
                new Player("Christian"),
                new Player("Lasse"),
                new Player("Mathias P."),
                new Player("Mathias I."),
                new Player("Nikolaj")
            };
        }

        public override void UpdateCurrentPlayer(CurrentPlayer currentPlayer)
        {
            this.currentPlayer = currentPlayer;
        }
    }
}
