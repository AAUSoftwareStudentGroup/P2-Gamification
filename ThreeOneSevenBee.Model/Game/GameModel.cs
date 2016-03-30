using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Expression;

namespace ThreeOneSevenBee.Model.Game
{
    public class GameModel
    {
        public CurrentPlayer Player { get; }
        private GameAPI API;
        public ExpressionModel Expression { get; }

        private Level currentLevel;

        public void SetLevel(Level level)
        {
            currentLevel = level;
            Expression = new ExpressionModel(level.Expression)
        }

        public GameModel(GameAPI api)
        {
            API = api;
            Player = api.GetCurrentPlayer();
            currentLevel = null;
        }




    }
}
