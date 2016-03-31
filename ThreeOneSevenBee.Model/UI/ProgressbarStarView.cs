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
        public ProgressbarStarView(ProgressbarStar progressbar, double width, double height) : base(width, height)
        {
            PropagateClick = false;
            BackgroundColor = "#E2E2E2";
            Children = new List<View>()
            {
                new View(0, 0, Width * progressbar.Percentage, height) { BackgroundColor = "#27AE61" }
            };
            foreach (int star in progressbar.Stars)
            {
                Children.Add(
                    new ImageView(star < progressbar.Progress ? "star_activated.png" : "star.png", height, height)
                    {
                        X = (double)star / progressbar.MaxProgress * Width - Height
                    });
            }
        }
    }
}
