using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
    public abstract class GameAPI
    {
        public abstract List<Player> GetPlayers();
        public abstract CurrentPlayer GetCurrentPlayer();
        public abstract void UpdateCurrentPlayer(CurrentPlayer currentPlayer);
    }
}
