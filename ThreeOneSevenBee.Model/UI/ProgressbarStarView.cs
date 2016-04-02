using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeOneSevenBee.Model.Game;

namespace ThreeOneSevenBee.Model.UI
{
    public class ProgressbarStarView : CompositeView
    {
        View progress;
        List<ImageView> stars;

        public void Build(ProgressbarStar progressbar)
        {
            progress = new View(0, 0, Width * progressbar.Percentage, Height) { BackgroundColor = "#27AE61" };
            stars = new List<ImageView>();
            stars.AddRange(progressbar.Stars.Select(
                (star) =>
                {
                    return new ImageView(star < progressbar.Progress ? "star_activated.png" : "star.png", Height, Height)
                    {
                        X = (double)star / progressbar.MaxProgress * Width - Height
                    };
                }
                ));
            Children.Add(progress);
            Children.AddRange(stars);
        }

        public void Update(ProgressbarStar progressbar)
        {
            for (int index = 0; index < progressbar.Stars.Count; index++)
            {
                stars[index].Image = progressbar.Stars[index] < progressbar.Progress ? "star_activated.png" : "star.png";
            }
            progress.Width = Width * progressbar.Percentage;
        }

        public ProgressbarStarView(ProgressbarStar progressbar, double width, double height) : base(width, height)
        {
            PropagateClick = false;
            BackgroundColor = "#E2E2E2";
            Build(progressbar);
        }
    }
}
