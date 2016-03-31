using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            currentPlayer = new CurrentPlayer("morten");
            currentPlayer.Badges = new List<string> { "FractionBadge", "ExponentBadge" };
            currentPlayer.CurrentCategory = 0;
            currentPlayer.CurrentLevel = 0;
            currentPlayer.Categories = new List<LevelCategory>() {
                new LevelCategory("Numbers") {
                    Levels = new List<Level>()
                        {
                            new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7")
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
            return new List<Player>() { currentPlayer };
        }

        public override void UpdateCurrentPlayer(CurrentPlayer currentPlayer)
        {
            this.currentPlayer = currentPlayer;
        }
    }
}
