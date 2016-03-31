using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
    class StubGameAPI : GameAPI
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
            currentPlayer.Categories = new List<LevelCategory>() {
                new LevelCategory("Numbers") {
                    Levels = new List<Level>()
                        {
                            new Level("5*5*5", "5*5*5", "5^2*5", "5^3", "125")
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
