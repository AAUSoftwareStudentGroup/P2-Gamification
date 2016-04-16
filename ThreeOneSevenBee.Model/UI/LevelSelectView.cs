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

        private void nextCategory()
        {
            Category++;
            if (OnChanged != null)
            {
                OnChanged();
            }
        }
        private void previousCategory()
        {
            Category--;
            if (OnChanged != null)
            {
                OnChanged();
            }
        }

        public void Build(CurrentPlayer user)
        {
            MenuButton = new ButtonView("Menu", () => { if (OnExit != null) { OnExit(); } })
            {
                Width = 75,
                Height = 30,
                BackgroundColor = "#C1392B",
                FontColor = "#FFFFFF",
                Font = "Segoe UI",
                FontSize = 18
            };

            CategoryName = new LabelView(user.Categories[Category].Name)
            {
                X = 100,
                Y = 20,
                Width = 200,
                Height = 40,
                FontSize = 25
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
            ArrowRight.BackgroundColor = "#297782";
            ArrowRight.OnClick = nextCategory;

            ArrowLeft = new VectorImageView(5, Levels.Y + Levels.Height / 2 - 75 / 2, 50, 75)
            {
                { 50,0    },
                { 25,75/2 },
                { 50,75   },
            };
            ArrowLeft.BackgroundColor = "#297782";
            ArrowLeft.OnClick = previousCategory;
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
                levelButtons.Add(
                    new ButtonView((levelNumber + 1).ToString(), () => OnLevelSelect(level))
                    {
                        Width = 50 - 10,
                        Height = 50 - 10,
                        X = levelNumber % (int)Math.Sqrt(numberOfLevels) * 50 + 5,
                        Y = levelNumber / (int)Math.Sqrt(numberOfLevels) * 50 + 5,
                        BackgroundColor = "#297782",
                        FontColor = "#ffffff",
                        FontSize = 25
                    });
                levelNumber += 1;

            }
            levelButtons.Width = (int)Math.Sqrt(numberOfLevels) * 50;
            levelButtons.Height = levelNumber / (int)Math.Sqrt(numberOfLevels) * 50;
            Levels.setContent(levelButtons);
        }

        public LevelSelectView(CurrentPlayer user) : base(400, 300)
        {
            Category = user.CurrentCategoryIndex;
            Build(user);
        }
    }
}
