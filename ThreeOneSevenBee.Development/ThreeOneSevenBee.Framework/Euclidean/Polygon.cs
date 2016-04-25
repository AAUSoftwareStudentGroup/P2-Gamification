using System.Collections.Generic;
using System;

namespace ThreeOneSevenBee.Framework.Euclidean
{
    /// <summary>
    /// A basic polygon, represented as a list of points.
    /// </summary>
    public struct Polygon
    {
        public int sides { get { return points.Count-1; } }

        public List<Vector2> points;
        
        public bool Contains(Vector2 point)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Rectangle rectangle)
        {
            return
                Contains(new Vector2(rectangle.Left, rectangle.Top)) &&
                Contains(new Vector2(rectangle.Right, rectangle.Top)) &&
                Contains(new Vector2(rectangle.Right, rectangle.Bottom)) &&
                Contains(new Vector2(rectangle.Left, rectangle.Bottom));
        }
    }
}
