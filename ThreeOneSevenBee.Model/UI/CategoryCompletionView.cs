using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Game;

namespace ThreeOneSevenBee.Model.UI
{
    public class CategoryCompletionView : CompositeView
    {
        public LevelCategory Category { get; private set; }
        public Action NextCategory { get; set; }
        public Action OnExit { get; set; }
        public Action OnNext { get; set; }

        private LabelView congratulationView;
        private LabelView descriptionView;
        private ImageView badgeView;
        private ButtonView MenuButton;
        private ButtonView nextCategory;

        public CategoryCompletionView(LevelCategory category) : base(600, 400)
        {
            this.Category = category;
            Build();
        }

        public void Build()
        {
            int offSetY = 5;
            Children = new List<View>();
            congratulationView = new LabelView("Tillykke !!!")
            {
                X = (Width * 0.5) - ((this.Width * 0.65) / 2),
                Y = offSetY,
                Width = this.Width * 0.65,
                Height = this.Height * 0.20
            };

            descriptionView = new LabelView("Du har gennemført kategorien: " + Category.Name)
            {
                X = (Width * 0.5) - ((congratulationView.Width * 0.75) * 0.5),
                Y = offSetY + congratulationView.Height,
                Width = congratulationView.Width * 0.75,
                Height = congratulationView.Height / 2
            };
            Children.Add(congratulationView);
            Children.Add(descriptionView);



            PlayerListView badgeDic = new PlayerListView(Width, Height);

            badgeView = null;
            if (badgeDic.badgeDictionary.ContainsKey(Category.Badge))
            {
                badgeView = new ImageView(badgeDic.badgeDictionary[Category.Badge], Width * 0.25, Width * 0.25)
                {
                    X = Width / 2 - ((Width * 0.25) / 2),
                    Y = Height * 0.40
                };
                Children.Add(badgeView);
            }

            MenuButton = new ButtonView("Menu", () => { if (OnExit != null) OnExit(); })
            {
                X = badgeView.X + (badgeView.Width / 2) - 115,
                Y = Height - 50,
                Width = 105,
                Height = 35,
                BackgroundColor = new Color(192, 57, 43),
                TextColor = new Color(255, 255, 255)
            };
            nextCategory = new ButtonView("Næste", () => { if (OnNext != null) OnNext(); })
            {
                X = badgeView.X + (badgeView.Width / 2) + 10,
                Y = Height - 50,
                Width = 105,
                Height = 35,
                TextColor = new Color(255, 255, 255),
                BackgroundColor = new Color(40, 120, 130)
            };
            Children.Add(MenuButton);
            Children.Add(nextCategory);
        }
    }
}
