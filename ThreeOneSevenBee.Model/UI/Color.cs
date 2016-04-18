using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class Color
    {
        public double Red;
        public double Green;
        public double Blue;
        public double Alpha;
        
        public Color() : this(0, 0, 0, 0)
        { }

        public Color(double red, double green, double blue) : this(red, green, blue, 1)
        { }

        public Color(double red, double green, double blue, double alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
    }
}
