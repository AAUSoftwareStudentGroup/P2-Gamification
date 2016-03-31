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
        public ExpressionModel ExprModel { get; private set; }
        public ExpressionBase CurrentExpression { get { return ExprModel.Expression; } }
        public ExpressionBase StartExpression { get; private set; }
        public List<ExpressionBase> StarExpressions { get; private set; }
        public event Action<GameModel> OnChanged;
        private ProgressbarStar progress;

        private int currentCategory;
        private int currentLevel;

        public bool LevelCompleted
        {
            get
            {
                if (currentLevel == -1)
                {
                    return false;
                }
                return progress.GetStars() > 0;
            }
        }

        public bool CategoryCompleted
        {
            get
            {
                if (currentCategory == -1)
                {
                    return false;
                }
                return LevelCompleted && currentLevel == Player.Categories[currentCategory].Levels.Count - 1;
            }
        }

        public bool GameCompleted
        {
            get
            {
                return CategoryCompleted && currentCategory == Player.Categories.Count - 1;
            }
        }

        public void SetLevel(int level, int category)
        {
            currentLevel = level;
            currentCategory = category;
            ExprModel = new ExpressionModel(Player.Categories[category].Levels[level].CurrentExpression, (m) => onExpressionChanged(m));
            progress = new ProgressbarStar(ExprModel.Expression.Size, ExprModel.Expression.Size);
            ExpressionSerializer serializer = new ExpressionSerializer();
            foreach (string starExpression in Player.Categories[category].Levels[level].StarExpressions)
            {
                ExpressionBase starExpressionBase = serializer.Deserialize(starExpression);
                StarExpressions.Add(starExpressionBase);
                progress.Add(starExpressionBase.Size);
            }
        }

        private void onExpressionChanged(ExpressionModel model)
        {
            progress.Progress = model.Expression.Size;
            OnChanged(this);
        }

        public void NextLevel()
        {
            if (GameCompleted)
            {

            }
            else if (CategoryCompleted)
            {
                currentCategory++;
                currentLevel = 0;

            }
            else if (LevelCompleted)
            {
                currentLevel++;
            }
        }

        public GameModel(GameAPI api)
        {
            API = api;
            Player = api.GetCurrentPlayer();
            currentLevel = -1;
            currentCategory = -1;
        }

    }
}
