using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.ExpressionRules;

namespace ThreeOneSevenBee.Model.Game
{
    public class GameModel
    {
        public CurrentPlayer User { get; }
        private GameAPI API;
        public ExpressionModel ExprModel { get; private set; }
        public ExpressionBase CurrentExpression { get { return ExprModel.Expression; } }
        public ExpressionBase StartExpression { get; private set; }
        public List<ExpressionBase> StarExpressions { get; private set; }
        public Action<GameModel> OnChanged;
        public ProgressbarStar Progress;

        public bool LevelCompleted
        {
            get
            {
                return Progress.GetStars() > 0;
            }
        }

        public bool CategoryCompleted
        {
            get
            {
                return LevelCompleted && User.CurrentLevel == User.Categories[User.CurrentCategory].Levels.Count - 1;
            }
        }

        public bool GameCompleted
        {
            get
            {
                return CategoryCompleted && User.CurrentCategory == User.Categories.Count - 1;
            }
        }

        public void SetLevel(int level, int category)
        {
            User.CurrentLevel = level;
            User.CurrentCategory = category;
            ExpressionSerializer serializer = new ExpressionSerializer();
            Progress = new ProgressbarStar(serializer.Deserialize(User.Categories[category].Levels[level].StarExpressions.Last()).Size, serializer.Deserialize(User.Categories[category].Levels[level].StartExpression).Size);
            StarExpressions = new List<ExpressionBase>();
            foreach (string starExpression in User.Categories[User.CurrentCategory].Levels[User.CurrentLevel].StarExpressions)
            {
                ExpressionBase starExpressionBase = serializer.Deserialize(starExpression);
                StarExpressions.Add(starExpressionBase);
                Progress.Add(starExpressionBase.Size);
            }
            ExprModel = new ExpressionModel(User.Categories[category].Levels[level].CurrentExpression, (m) => onExpressionChanged(m), 
                Rules.ExponentToProductRule, Rules.ProductToExponentRule, Rules.AddFractionsWithSameNumerators, 
                Rules.VariableWithNegativeExponent, Rules.ReverseVariableWithNegativeExponent, Rules.ExponentProduct);
            onExpressionChanged(ExprModel);
        }

        private void onExpressionChanged(ExpressionModel model)
        {
            Progress.Progress = model.Expression.Size;
            if (OnChanged != null)
            {
                OnChanged(this);
            }
        }

        public void NextLevel()
        {
            if (GameCompleted)
            {

            }
            else if (CategoryCompleted)
            {
                User.CurrentCategory++;
                User.CurrentLevel = 0;

            }
            else if (LevelCompleted)
            {
                User.CurrentLevel++;
            }
            SetLevel(User.CurrentLevel, User.CurrentCategory);
        }

        public GameModel(GameAPI api, Action<GameModel> onChanged)
        {
            OnChanged = onChanged;
            API = api;
            User = api.GetCurrentPlayer();
            SetLevel(User.CurrentLevel, User.CurrentCategory);
        }

        public GameModel(GameAPI api) : this(api, null)
        { }

    }
}
