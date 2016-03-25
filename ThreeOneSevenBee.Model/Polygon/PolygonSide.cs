using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Polygon
{
    public class PolygonSide
    {
        public PolygonCorner corner1 { get; private set; }
        public PolygonCorner corner2 { get; private set; }
        private char identifier;
        public PolygonSide(PolygonCorner corner1, PolygonCorner corner2, char identifier)
        {
            this.corner1 = corner1;
            this.corner2 = corner2;
            this.identifier = identifier;
        }
        public PolygonCorner GetOtherCorner(PolygonCorner corner)
        {
            if (corner == corner1)
                return corner2;
            else if (corner == corner2)
                return corner1;
            else
                return null;
        }
    }
}
