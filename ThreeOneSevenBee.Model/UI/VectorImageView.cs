using ThreeOneSevenBee.Model.Euclidean;
using System.Collections.Generic;
using System;
using System.Collections;

namespace ThreeOneSevenBee.Model.UI
{
    public class VectorImageView : View, IEnumerable<Vector2>
    {
        public VectorImageView(double x, double y, double width, double height) : base(x, y, width, height)
        {
            Path = new List<Vector2>();
        }

        public void Add(double x, double y)
        {
            Path.Add(new Vector2(x,y));
        }

        public override View Scale(double factor)
        {
            for (int i = 0; i < Path.Count; i++)
                Path[i] *= factor;
            return base.Scale(factor);
        }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX, offsetY);
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return Path.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<Vector2> Path { get; set; }
    }
}