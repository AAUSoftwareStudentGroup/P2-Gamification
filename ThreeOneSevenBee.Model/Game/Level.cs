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

        public int LevelID;
        public int LevelIndex;
        public int CategoryIndex;
        public int Stars;

        public Level(string startExpression, string currentExpression, int stars, string[] starExpressions) : this(-1, -1, -1, startExpression, stars, currentExpression, starExpressions)
        { }

        public Level(int levelID, string startExpression, int stars, string currentExpression, string[] starExpressions) : this(levelID, -1, -1, startExpression, stars, currentExpression, starExpressions)
        { }

        public Level(int levelID, int levelIndex, int categoryIndex, string startExpression, int stars, string currentExpression, params string[] starExpressions)
        {
            LevelID = levelID;
            LevelIndex = levelIndex;
            CategoryIndex = categoryIndex;
            StartExpression = startExpression;
            CurrentExpression = currentExpression;
            Stars = stars;
            StarExpressions = new List<string>();
            foreach (var star in starExpressions)
            {
                StarExpressions.Add(star);
            }
            for (int n = 0; n < 3 - starExpressions.Count(); n++)
            {
                StarExpressions.Add(starExpressions.Last());
            }
        }
    }
}
