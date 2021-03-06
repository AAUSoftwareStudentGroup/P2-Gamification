﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeOneSevenBee.Model.Game;

namespace ThreeOneSevenBee.Model.UI
{
    /// <summary>
    /// A view of the stars based on a progressbar
    /// </summary>
    public class ProgressbarStarView : CompositeView
    {
        View progress;
        List<ImageView> stars;

        public void Build(ProgressbarStar progressbar)
        {
            progress = new View(0, 0, Math.Min(Width, Math.Max(0, Width * progressbar.Percentage)), Height) { BackgroundColor = new Color(40, 175, 100) };
            stars = new List<ImageView>();
            stars.AddRange(progressbar.ActivatedStarPercentages().Select(
                (starPercentage) =>
                {
                    return new ImageView("star_activated.png", Height, Height)
                    {
                        X = (double)starPercentage * Width - Height
                    };
                }
                ));
            stars.AddRange(progressbar.DeactivatedStarPercentages().Select(
                (starPercentage) =>
                {
                    return new ImageView("star.png", Height, Height)
                    {
                        X = (double)starPercentage * Width - Height
                    };
                }
                ));
            double? lastX = null;
            int numberOfEqualX = 1;
            for (int index = 0; index < stars.Count; index++)
            {
                if (stars[index].X - lastX < double.Epsilon)
                {
                    stars[index].X -= numberOfEqualX * (stars[index].Width / 2);
                    numberOfEqualX++;
                }
                else
                {
                    lastX = stars[index].X;
                }
            }
            Children = new List<View>();
            Children.Add(progress);
            Children.AddRange(stars);
        }

        public void Update(ProgressbarStar progressbar)
        {
            Build(progressbar);
        }

        public ProgressbarStarView(ProgressbarStar progressbar, double width, double height) : base(width, height)
        {
            PropagateClick = false;
            BackgroundColor = new Color(225, 225, 225);
            Build(progressbar);
        }
    }
}
