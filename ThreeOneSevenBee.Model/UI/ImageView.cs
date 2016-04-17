using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class ImageView : View
    {
        public string Image { get; set; }

        public ImageView(string image, double width, double height) : base(0, 0, width, height)
        {
            Image = image;
            BackgroundColor = new Color();
        }
    }
}
