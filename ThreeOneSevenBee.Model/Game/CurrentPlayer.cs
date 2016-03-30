using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
    class CurrentPlayer : Player
    {
        public CurrentPlayer(string player) : base(player){ }

        public List<Level> CompletedLevels = new List<Level>();


    }
}
