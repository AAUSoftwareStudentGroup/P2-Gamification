namespace ThreeOneSevenBee.Framework.Euclidean
{
    /// <summary>
    /// A basic circle, represented as a point and a radius.
    /// </summary>
    public struct Circle
    {
        public Vector2 Center;

        public double Radius;

        public bool Contains(Vector2 point)
        {
            var dx = point.X - Center.X;
            var dy = point.Y - Center.Y;

            var squareDist = dx * dx + dy * dy;
            var squareRadius = Radius * Radius;

            return squareDist < squareRadius;
        }

        public bool Contains(Rectangle rectangle)
        {
            return
                Contains(new Vector2(rectangle.Top, rectangle.Left)) &&
                Contains(new Vector2(rectangle.Top, rectangle.Right)) &&
                Contains(new Vector2(rectangle.Bottom, rectangle.Right)) &&
                Contains(new Vector2(rectangle.Bottom, rectangle.Left));
        }
    }
}
