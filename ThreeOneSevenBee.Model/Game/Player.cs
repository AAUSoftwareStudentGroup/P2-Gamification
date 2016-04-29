using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
   public class Player {
        public List<BadgeName> Badges;

        public Player(string playername)
        {
            this.PlayerName = playername;
            Badges = new List<BadgeName>();
        }

        public string PlayerName { get; private set; }

        public string LastLoginTime { get; set; }
    }

    public enum BadgeName
    {
        brokBadge,
        masterOfAlgebra, 
        potensBadge,
        parenthesisBadge,
        tutorialBadge
    }
}
