namespace ThreeOneSevenBee.Framework.Euclidean
{
    /// <summary>
    /// A basic rectangle represented as the location of the left top point and a width and height.
    /// </summary>
    public struct Rectangle
    {
        public Vector2 Location;

        public double Width;

        public double Height;

        public double Left { get { return Location.X; } }

        public double Top { get { return Location.Y; } }

        public double Right { get { return Location.X + Width; } }

        public double Bottom { get { return Location.Y + Height; } }

        public bool Contains(Vector2 point)
        {
            return (point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom);
        }

        public bool Contains(Circle circle)
        {
            return (circle.Left >= Left && circle.Right <= Right && circle.Top >= Top && circle.Bottom <= Bottom);
        }
    }
}
