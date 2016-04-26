using System;
#if BRIDGE
using Bridge.Html5;
#endif
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
        public IEnumerable<Player> Players { get; private set; }
        public ExpressionModel ExprModel { get; private set; }
        public ExpressionBase CurrentExpression { get { return ExprModel.Expression; } }
        public ExpressionBase StartExpression { get; private set; }
        public List<ExpressionBase> StarExpressions { get; private set; }
        public Action<GameModel> OnChanged;
        public Action<Level> OnSaveLevel;
        public ProgressbarStar ProgressBar;

        public bool IsFirstLevel
        {
            get
            {
                return User.CurrentCategoryIndex == 0 && User.CurrentLevelIndex == 0;
            }
        }

        public bool IsLevelCompleted
        {
            get
            {
                return ProgressBar.ActivatedStarPercentages().Count() >= 1;
            }
        }

        public bool IsCategoryCompleted
        {
            get
            {
                return IsLevelCompleted && User.CurrentLevelIndex == User.Categories[User.CurrentCategoryIndex].Count - 1;
            }
        }

        public bool IsGameCompleted
        {
            get
            {
                return IsCategoryCompleted && User.CurrentCategoryIndex == User.Categories.Count - 1;
            }
        }

        public void SetLevel(int level, int category)
        {
            User.CurrentLevelIndex = level;
            User.CurrentCategoryIndex = category;
            ExpressionSerializer serializer = new ExpressionSerializer();
            int endValue = serializer.Deserialize(User.Categories[category][level].StarExpressions.Last()).Size;
            int currentValue = serializer.Deserialize(User.Categories[category][level].StartExpression).Size;
            ProgressBar = new ProgressbarStar(currentValue, endValue, currentValue);
            StarExpressions = new List<ExpressionBase>();

            foreach (string starExpression in User.Categories[User.CurrentCategoryIndex][User.CurrentLevelIndex].StarExpressions)
            {
                ExpressionBase starExpressionBase = serializer.Deserialize(starExpression);
                StarExpressions.Add(starExpressionBase);
                ProgressBar.Add(starExpressionBase.Size);
            }
            ExprModel = new ExpressionModel(User.Categories[category][level].CurrentExpression, (m) => onExpressionChanged(m),
                Rules.ExponentToProductRule, Rules.ProductToExponentRule,
                Rules.VariableWithNegativeExponent, Rules.ReverseVariableWithNegativeExponent, Rules.ExponentProduct,
                Rules.NumericCalculateRule, Rules.CommonPowerParenthesisRule,
                Rules.ReverseCommonPowerParenthesisRule, Rules.SplittingFractions, Rules.ProductParenthesis,
                Rules.ReverseProductParenthesis, Rules.ParenthesisPowerRule, Rules.FractionToProductRule, Rules.SquareRootRule,
                Rules.RemoveParenthesisRule, Rules.ProductOfConstantAndFraction, Rules.FactorizeUnaryMinus, Rules.FactorizationRule,
                Rules.MultiplyOneRule, Rules.AddFractionWithCommonDenominatorRule, Rules.RemoveNull, Rules.MultiplyByNull);
            onExpressionChanged(ExprModel);
        }

        public void RestartLevel()
        {
            User.CurrentLevel.CurrentExpression = User.CurrentLevel.StartExpression;
            SetLevel(User.CurrentLevelIndex, User.CurrentCategoryIndex);
        }

        private void onExpressionChanged(ExpressionModel model)
        {
            ProgressBar.CurrentValue = model.Expression.Size;
            User.CurrentLevel.CurrentExpression = model.Expression.ToString();
            User.CurrentLevel.Stars = Math.Max(User.CurrentLevel.Stars, ProgressBar.ActivatedStarPercentages().Count());
            if (OnChanged != null)
            {
                OnChanged(this);
            }
        }

        public void NextLevel()
        {
            if (IsGameCompleted)
            {
                
            }
            else if (IsCategoryCompleted)
            {
                User.CurrentCategoryIndex++;
                User.CurrentLevelIndex = 0;

            }
            else if (IsLevelCompleted)
            {
                User.CurrentLevelIndex++;
            }
            else
            {
                return;
            }
            SetLevel(User.CurrentLevelIndex, User.CurrentCategoryIndex);
        }

        public void SaveLevel()
        {
            if(OnSaveLevel != null)
        {
                OnSaveLevel(User.CurrentLevel);
            }
        }

        public GameModel(CurrentPlayer user, List<Player> players)
        {
            User = user;
            Players = players;
            SetLevel(User.CurrentLevelIndex, User.CurrentCategoryIndex);
        }
    }
}
