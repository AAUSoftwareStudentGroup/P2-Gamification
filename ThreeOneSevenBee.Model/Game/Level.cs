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

        public Level(string startExpression, string currentExpression, params string[] starExpressions)
        {
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
