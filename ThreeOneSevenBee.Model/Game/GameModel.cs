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
        public IEnumerable<Player> Players { get; set; }
        public ExpressionModel ExprModel { get; private set; }
        public ExpressionBase CurrentExpression { get { return ExprModel.Expression; } }
        public ExpressionBase StartExpression { get; private set; }
        public List<ExpressionBase> StarExpressions { get; private set; }
        public Action<GameModel> OnChanged;
        public Action<LevelCategory> OnCategoryCompleted;
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

        public bool IsLastCategory
        {
            get
            {
                return User.CurrentCategoryIndex == User.Categories.Count - 1;
            }
        }

        public bool IsLastLevel
        {
            get
            {
                return IsLastCategory && User.CurrentLevelIndex == User.Categories[User.CurrentCategoryIndex].Count - 1;
            }
        }

        /// <summary>
        /// Gets the next level based on the users current progress.
        /// </summary>
        public Level GetNextLevel()
        {
            if(IsGameCompleted == false)
            {
                if(IsCategoryCompleted == true)
                {
                    return User.Categories[User.CurrentCategoryIndex + 1][0];
                }
                else
                {
                    return User.Categories[User.CurrentCategoryIndex][User.CurrentLevelIndex + 1];
                }
            }
            return null;
        }

        public bool IsGameCompleted
        {
            get
            {
                return IsCategoryCompleted && User.CurrentCategoryIndex == User.Categories.Count - 1;
            }
        }

        /// <summary>
        /// Sets up the model to complete the level.
        /// </summary>
        /// <param name="level">The desired level.</param>
        /// <param name="category">The desired category.</param>
        public void SetLevel(int level, int category)
        {
            User.CurrentLevelIndex = level;
            User.CurrentCategoryIndex = category;
            ExpressionSerializer serializer = new ExpressionSerializer();
            int endValue = serializer.Deserialize(User.Categories[category][level].StarExpressions.Last()).Size;
            int startValue = serializer.Deserialize(User.Categories[category][level].StartExpression).Size;
            int currentValue = serializer.Deserialize(User.Categories[category][level].CurrentExpression).Size;
            ProgressBar = new ProgressbarStar(startValue, endValue, currentValue);
            StarExpressions = new List<ExpressionBase>();

            foreach (string starExpression in User.CurrentLevel.StarExpressions)
            {
                ExpressionBase starExpressionBase = serializer.Deserialize(starExpression);
                StarExpressions.Add(starExpressionBase);
                ProgressBar.Add(starExpressionBase.Size);
            }
            ExprModel = new ExpressionModel(User.Categories[category][level].CurrentExpression, (m) => onExpressionChanged(m),
                Rules.ExponentToProductRule, Rules.ProductToExponentRule,
                Rules.VariableWithNegativeExponent, Rules.ReverseVariableWithNegativeExponent, Rules.ExponentProduct,
                Rules.CommonPowerParenthesisRule, Rules.ReverseCommonPowerParenthesisRule, Rules.SplittingFractions, Rules.ProductParenthesis,
                Rules.ReverseProductParenthesis, Rules.ParenthesisPowerRule, Rules.FractionToProductRule, Rules.SquareRootRule,
                Rules.RemoveParenthesisRule, Rules.ProductOfConstantAndFraction, Rules.FactorizeUnaryMinus, Rules.FactorizationRule,
                Rules.MultiplyOneRule, Rules.AddFractionWithCommonDenominatorRule, Rules.RemoveNull, Rules.MultiplyByNull,
                Rules.CalculateVariadicRule, Rules.CalculateBinaryRule, Rules.MultiplyMinusRule, Rules.DivisionEqualsOneRule, Rules.ProductOfFractions, Rules.VariablesEqualNull);
            UpdateLevelData();
            onExpressionChanged(ExprModel);
        }

        /// <summary>
        /// Restarts the current level.
        /// </summary>
        public void RestartLevel()
        {
            User.CurrentLevel.CurrentExpression = User.CurrentLevel.StartExpression;
            SetLevel(User.CurrentLevelIndex, User.CurrentCategoryIndex);
        }

        private void onExpressionChanged(ExpressionModel model)
        {
            ProgressBar.CurrentValue = model.Expression.Size;
            User.CurrentLevel.CurrentExpression = model.Expression.ToString();
            if (ProgressBar.ActivatedStarPercentages().Count() > User.CurrentLevel.Stars)
            {
                UpdateLevelData();
            }
            if (OnChanged != null)
            {
                OnChanged(this);
            }
        }

        private void UpdateLevelData()
        {
            if (ProgressBar.ActivatedStarPercentages().Count() > User.CurrentLevel.Stars)
            {
            User.CurrentLevel.Stars = ProgressBar.ActivatedStarPercentages().Count();
            if(GetNextLevel() != null)
            {
                    GetNextLevel().Unlocked = true;
            }
            
            if (User.CurrentLevel.Stars == 3)
            {
                int numberOfStars = 0;
                foreach (Level level in User.Categories[User.CurrentCategoryIndex])
                {
                    numberOfStars += level.Stars;
                }
                if (numberOfStars == User.Categories[User.CurrentCategoryIndex].Count * 3)
                {
                        if (OnCategoryCompleted != null)
                    {
                        BadgeName achievedBadge = User.Categories[User.CurrentCategoryIndex].Badge;
                        if (User.Badges.Contains(achievedBadge) == false)
                        {
                            User.Badges.Add(achievedBadge);
                            Players.First((p) => p.PlayerName == User.PlayerName).Badges.Add(achievedBadge);
                                OnSaveLevel(User.CurrentLevel);
                                OnCategoryCompleted(User.Categories[User.CurrentCategoryIndex]);
                            }
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// Swithces to the next level.
        /// </summary>
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

        /// <summary>
        /// Saves the state of the current level.
        /// </summary>
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
			players.Sort(delegate(Player p1, Player p2) {
				return p2.Badges.Count.CompareTo(p1.Badges.Count);
			});

            SetLevel(User.CurrentLevelIndex, User.CurrentCategoryIndex);

        }
    }
}
