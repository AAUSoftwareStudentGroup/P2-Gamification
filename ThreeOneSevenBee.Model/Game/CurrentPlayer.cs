using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
    public class CurrentPlayer : Player
    {
        public CurrentPlayer(string player) : base(player){ }

        public int CurrentCategory;
        public int CurrentLevel;

        public List<LevelCategory> Categories = new List<LevelCategory>();
    }
}
