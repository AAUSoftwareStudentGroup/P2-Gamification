using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Game;

#if BRIDGE
using Bridge.Html5;
#endif

namespace ThreeOneSevenBee.Model.UI
{
    /// <summary>
    /// Builds a view for selecting levels in the selected category
    /// </summary>
    class LevelSelectView : CompositeView
    {
        public Action<Level> OnLevelSelect;
        public Action OnExit { get; set; }

        public int Category { get; private set; }

        public ButtonView MenuButton { get; private set; }

        public VectorImageView ArrowLeft { get; private set; }

        public VectorImageView ArrowRight { get; private set; }

        public FrameView Levels { get; private set; }

        public LabelView CategoryName { get; private set; }

        public CompositeView TitelView { get; private set; }

        public LabelView StarTextView { get; private set; }

        public ImageView StarView { get; private set; }

        public ImageView BadgeView { get; private set; }


        public void Build(CurrentPlayer user)
        {
            MenuButton = new ButtonView("Menu", () => { if (OnExit != null) { OnExit(); } })
            {
                Width = 75,
                Height = 30,
                BackgroundColor = new Color(193, 57, 43),
                TextColor = new Color(255, 255, 255),
            };

            TitelView = new CompositeView(200, 40)
            {
                X = 100,
                Y = 10,
            };
            CategoryName = new LabelView(user.Categories[Category].Name)
            {
                Width = TitelView.Width * 0.6,
                Height = TitelView.Height,
                X = TitelView.X - (TitelView.Width * 0.6) / 2
            };

            StarTextView = new LabelView("")
            {
                Width = TitelView.Width * 0.15, 
                Height = 15,
                X = CategoryName.X + CategoryName.Width + 7.5,
                Y = CategoryName.Y + ((CategoryName.Height) / 2) - 7.5
            };

            StarView = new ImageView("star_activated.png", TitelView.Width * 0.10, TitelView.Width * 0.10)
            {
                X = CategoryName.X + CategoryName.Width + StarTextView.Width + 10,
                Y = CategoryName.Y + 0.5 * (TitelView.Width * 0.10)
            };

            BadgeView = new ImageView("tutorialbadge.png", TitelView.Width * 0.15, TitelView.Width * 0.15)
            {
                X = 0 - ((TitelView.Width * 0.10) / 2) + 5,
                Y = CategoryName.Y + (0.5 * (TitelView.Width * 0.15)) - 10
            };

            TitelView.Add(StarTextView);
            TitelView.Add(CategoryName);
            TitelView.Add(StarView);
            TitelView.Add(BadgeView);

            Levels = new FrameView(Width - 100, Height - (CategoryName.Y + CategoryName.Height))
            {
                X = 50,
                Y = CategoryName.Y + CategoryName.Height,
            };

            ArrowRight = new VectorImageView(345, Levels.Y + Levels.Height / 2 - 75 / 2, 50, 75)
            {
                { 0,0    },
                { 25,75/2 },
                { 0,75   },
            };

            ArrowRight.Visible = Category < user.Categories.Count - 1;
            ArrowRight.BackgroundColor = new Color(44, 119, 130);
            ArrowRight.OnClick = () =>
            {
                if (Category < user.Categories.Count - 1)
                {
                    Category++;
                    OnChanged();
                }
            };

            ArrowLeft = new VectorImageView(5, Levels.Y + Levels.Height / 2 - 75 / 2, 50, 75)
            {
				{ 25,75/2 },
                { 50,0    },
                { 50,75   },
            };

            ArrowLeft.BackgroundColor = new Color(44, 119, 130);
            ArrowLeft.OnClick = () =>
            {
                if (Category > 0)
                {
                    Category--;
                    OnChanged();
                }
            };
            ArrowLeft.Visible = Category > 0;

            Children.Add(MenuButton);
            Children.Add(ArrowLeft);
            Children.Add(ArrowRight);
            Children.Add(Levels);
            Children.Add(TitelView);

            Update(user);
        }
        /// <summary>
        /// Updates the view based on the selected category. 
        /// </summary>
        /// <param name="user"></param>
        public void Update(CurrentPlayer user)
        {
            CategoryName.Text = user.Categories[Category].Name;
            ArrowLeft.Visible = Category > 0;
            ArrowRight.Visible = Category < user.Categories.Count - 1;
            int totalStars = user.Categories[Category].Count * 3;
            int userStarsInCategory = 0;

            CompositeView levelButtons = new CompositeView(400, 400);

            int levelNumber = 0;
            int numberOfLevels = user.Categories[Category].Count;
            foreach (Level level in user.Categories[Category])
            {
                // adds a button for each level in category
                userStarsInCategory += level.Stars;
                CompositeView levelButton = new CompositeView(40, 40)
                {   OnClick = () => OnLevelSelect(level),
                    X = levelNumber % (int)Math.Sqrt(numberOfLevels) * 50 + 5,
                    Y = levelNumber / (int)Math.Sqrt(numberOfLevels) * 50 + 5,
                    BackgroundColor = level.Unlocked ? new Color(40, 130, 120) : new Color(190, 190, 190) 
                };
                // gives each levelbutton a level number
                levelButton.Add(
                    new LabelView((levelNumber + 1).ToString())
                    {
                        Width = levelButton.Width,
                        Height = levelButton.Width * 0.75,
                        BackgroundColor = level.Unlocked ? new Color(40, 130, 120) : new Color(190, 190, 190),
                        TextColor = new Color(255, 255, 255),
                    });
                double starsize = levelButton.Width * 0.25;
                // Centeres the stars horizontally
                double startPostition = (levelButton.Width - (starsize)) / 2;
                if (level.Stars == 2)
                {
                    startPostition = levelButton.Width / 2 - starsize;
                }
                else if (level.Stars == 3)
                {
                    startPostition = (levelButton.Width - (starsize)) / 2 - starsize;
                }
                for (int n = 0; n < level.Stars; n++)
                {
                    levelButton.Add(new ImageView("star_activated.png", starsize, starsize)
                    {
                        Y = levelButton.Height - starsize,     
                        X = startPostition
                    });
                    startPostition += starsize;
                }
                levelButtons.Add(levelButton);
                levelNumber += 1;
                // finds the badge associated with the specific category
                switch (user.Categories[Category].Name)
                {
                    case "Tutorial":
                        BadgeView.Image = "tutorial_badge.png";
                        break;

                    case "Potenser":
                        BadgeView.Image = "potens_badge.png";
                        break;

                    case "Brøker":
                        BadgeView.Image = "brøkbadge.png";
                        break;

                    case "Parenteser":
                        BadgeView.Image = "parenthesis_badge.png";
                        break;

                    case "Master of Algebra":
                        BadgeView.Image = "master_of_algebrabadge.png";
                        break;
                    default:
                        break;
                }

            }
            // shows how many start the user have out of the total amount of start possible
            StarTextView.Text = userStarsInCategory + " / " + totalStars;
            levelButtons.Width = (int)Math.Sqrt(numberOfLevels) * 50;
            levelButtons.Height = levelNumber / (int)Math.Sqrt(numberOfLevels) * 50;
            Levels.setContent(levelButtons);

        }

        public LevelSelectView(CurrentPlayer user) : base(400, 300)
        {
            BackgroundColor = new Color(255, 255, 255);
            Category = user.CurrentCategoryIndex;
            Build(user);
        }
    }
}
