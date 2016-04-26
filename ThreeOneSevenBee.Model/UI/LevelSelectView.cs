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


        public void Build(CurrentPlayer user)
        {
            MenuButton = new ButtonView("Menu", () => { if (OnExit != null) { OnExit(); } })
            {
                Width = 75,
                Height = 30,
                BackgroundColor = new Color(193, 57, 43),
                TextColor = new Color(255, 255, 255),
            };

            CategoryName = new LabelView(user.Categories[Category].Name)
            {
                X = 100,
                Y = 20,
                Width = 200,
                Height = 40,
            };

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
                { 50,0    },
                { 25,75/2 },
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
            Children.Add(CategoryName);
            Children.Add(ArrowLeft);
            Children.Add(ArrowRight);
            Children.Add(Levels);

            Update(user);
        }

        public void Update(CurrentPlayer user)
        {
            CategoryName.Text = user.Categories[Category].Name;
            ArrowLeft.Visible = Category > 0;
            ArrowRight.Visible = Category < user.Categories.Count - 1;

            CompositeView levelButtons = new CompositeView(400, 400);

            int levelNumber = 0;
            int numberOfLevels = user.Categories[Category].Count;
            foreach (Level level in user.Categories[Category])
            {
                CompositeView levelButton = new CompositeView(40, 40)
                {
                    X = levelNumber % (int)Math.Sqrt(numberOfLevels) * 50 + 5,
                    Y = levelNumber / (int)Math.Sqrt(numberOfLevels) * 50 + 5,
                    BackgroundColor = new Color(40, 130, 120)
                };
                levelButton.Add(
                    new ButtonView((levelNumber + 1).ToString(), () => OnLevelSelect(level))
                    {
                        Width = levelButton.Width,
                        Height = levelButton.Width * 0.75,
                        BackgroundColor = new Color(40, 130, 120),
                        TextColor = new Color(255, 255, 255),
                    });
                for (int n = 0; n < level.Stars; n++)
                {
                    double temp = (levelButton.Width - (levelButton.Width * 0.25) / 2);
                    levelButton.Add(new ImageView("star_activated.png", levelButton.Width * 0.25, levelButton.Width * 0.25)
                    {
                        Y = levelButton.Height - levelButton.Width*0.25,     
                        X = temp                   
                    });
                }
                // imageviews
                levelButtons.Add(levelButton);

                levelNumber += 1;

            }
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
