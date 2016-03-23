namespace threeonesevenbee.Model.Euclidean
{
    /// <summary>
    /// A basic circle, represented as a point and a radius.
    /// </summary>
    public struct Circle
    {
        public Vector2 Center;

        public double Radius;

        public double Left { get { return Center.X - Radius; } }

        public double Top { get { return Center.Y - Radius; } }

        public double Right { get { return Center.X + Radius; } }

        public double Bottom { get { return Center.Y + Radius; } }

        public bool Contains(Vector2 point)
        {
            var dx = point.X - Center.X;
            var dy = point.Y - Center.Y;

            var squareDist = dx * dx + dy * dy;
            var squareRadius = Radius * Radius;

            return squareDist <= squareRadius;
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
