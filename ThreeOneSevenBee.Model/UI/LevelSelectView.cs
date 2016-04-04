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

        public int Category { get; private set; }

        public ImageView ArrowLeft { get; private set; }

        public ImageView ArrowRight { get; private set; }

        public FrameView Levels { get; private set; }

        public LabelView CategoryName { get; private set; }

        private void nextCategory()
        {
            Category++;
            if(OnChanged != null)
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
            CategoryName = new LabelView(user.Categories[Category].Name)
            {
                X = 200,
                Y = 20,
                Width = 200,
                Height = 40,
                FontSize = 25
            };

            ArrowLeft = new ImageView("arrow_left.png", 50, (Category == 0 ? 0 : 150))
            {
                X = 5,
                Y = 75,
                OnClick = previousCategory

            };

            ArrowRight = new ImageView("arrow_right.png", 50, (Category == user.Categories.Count - 1 ? 0 : 150))
            {
                X = 545,
                Y = 75,
                OnClick = nextCategory
            };

            Levels = new FrameView(Width - ArrowRight.Width - ArrowLeft.Width, Height - (CategoryName.Y + CategoryName.Height))
            {
                X = ArrowLeft.X + ArrowLeft.Width - 5,
                Y = CategoryName.Y + CategoryName.Height,
                BackgroundColor = "#ff0000"
            };
            
            Children.Add(CategoryName);
            Children.Add(ArrowLeft);
            Children.Add(ArrowRight);
            Children.Add(Levels);

            Update(user);
        }

        public void Update(CurrentPlayer user)
        {

            CategoryName.Text = user.Categories[Category].Name;
            ArrowLeft.Height = (Category == 0 ? 0 : ArrowLeft.Width * 3);
            ArrowRight.Height = (Category == user.Categories.Count - 1 ? 0 : ArrowRight.Width * 3);

            CompositeView levelButtons = new CompositeView(400, 400) { BackgroundColor = "#0000ff" };

            int levelNumber = 1;
            foreach (Level level in user.Categories[Category].Levels)
            {
                levelButtons.Add(
                    new ButtonView((levelNumber++).ToString(), () => OnLevelSelect(level))
                    {
                        Width = 20,
                        Height = 20,
                        X = levelNumber * 20,
                        BackgroundColor = "#00ff00"
                    });
            }

            Levels.setContent(levelButtons);
        }

        public LevelSelectView(CurrentPlayer user) : base(600, 300)
        {
            Category = user.CurrentCategory;
            BackgroundColor = "#efefef";
            Build(user);
        }
    }
}
