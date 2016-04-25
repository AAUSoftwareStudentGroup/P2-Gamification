using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
   public class Player {

        public Player(string playername)
        {
            this.PlayerName = playername;
        }

        public List<int> Badges = new List<int>();

        public string PlayerName { get; private set; }

        public string LastLoginTime { get; set; }
    }
}
