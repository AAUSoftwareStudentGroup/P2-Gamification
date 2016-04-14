using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Expression;

namespace ThreeOneSevenBee.Model.Game
{
    public class Level
    {
        public string StartExpression;
        public List<string> StarExpressions;
        public string CurrentExpression;

        public int LevelIndex;
        public int CategoryIndex;


        public Level(string startExpression, string currentExpression, string[] starExpressions) : this(-1, -1, startExpression, currentExpression, starExpressions)
        { }

        public Level(int levelIndex, int categoryIndex, string startExpression, string currentExpression, params string[] starExpressions)
        {
            LevelIndex = levelIndex;
            CategoryIndex = categoryIndex;
            StartExpression = startExpression;
            CurrentExpression = currentExpression;
            StarExpressions = new List<string>();
            foreach (var star in starExpressions)
            {
                StarExpressions.Add(star);
            }
        }
    }
}
