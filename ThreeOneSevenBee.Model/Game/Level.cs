using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Expression;

namespace ThreeOneSevenBee.Model.Game
{
    class Level
    {
        public ProgressbarStar Progress;
        public ExpressionBase Expression;

        public Level(string expression, params string[] expressionGoals)
        {
            ExpressionSerializer serializer = new ExpressionSerializer();
            Expression = serializer.Deserialize(expression);
            Progress = new ProgressbarStar(Expression.Size, Expression.Size);
            foreach (string expressionGoal in expressionGoals)
            {
                Progress.Add(serializer.Deserialize(expressionGoal).Size);
            }
        }
    }
}
