using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Polygon
{
    class PolygonCorner
    {
        public char identifier { get; private set; }
        public PolygonCorner(char identifier)
        {
            this.identifier = identifier;
        }
    }
}
