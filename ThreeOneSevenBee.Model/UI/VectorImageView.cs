using ThreeOneSevenBee.Model.Euclidean;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

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

        public override void DrawWithContext(IContext context, double offsetX, double offsetY)
        {
            if(Visible == true) { 
                context.DrawPolygon(Path.Select((p) => new Vector2(X + p.X + offsetX, Y + p.Y + offsetY)).ToArray(), BackgroundColor);
			}
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