namespace ThreeOneSevenBee.Model.Euclidean
{
    /// <summary>
    /// Basic 2D vector.
    /// </summary>
    public struct Vector2
    {
        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X + right.X, left.Y + right.Y);
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X - right.X, left.Y - right.Y);
        }

        public static Vector2 operator *(double left, Vector2 right)
        {
            return new Vector2(left * right.X, left * right.Y);
        }

        public static Vector2 operator *(Vector2 left, double right)
        {
            return new Vector2(left.X * right, left.Y * right);
        }

        public static Vector2 operator /(double left, Vector2 right)
        {
            return new Vector2(left / right.X, left / right.Y);
        }

        public static Vector2 operator /(Vector2 left, double right)
        {
            return new Vector2(left.X / right, left.Y / right);
        }



        public double X;

        public double Y;

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "{" + X.ToString("0.00") + ", " + Y.ToString("0.00") + "}";
        }
    }
}
