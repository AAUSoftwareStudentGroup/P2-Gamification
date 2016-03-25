using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Euclidean;
using ThreeOneSevenBee.Model.Geometry;

namespace ThreeOneSevenBee.Model.UI
{
    public class PolygonView : View
    {
        public PolygonModel model { private set; get; }
        public List<Vector2> cornerPositions { private set; get; }
        public PolygonView(PolygonModel model, double x, double y, double width, double height) : base(x, y, width, height)
        {
            // Draw model as is
            cornerPositions = new List<Vector2>();
            Vector2 vector = new Vector2(0,0);
            double angle = 2 * Math.PI / (model.corners.Count() - 2);
            cornerPositions.Add(vector);
            for (int i = 1; i < model.corners.Count(); i++) {
                vector.X = Math.Cos(angle * i);
                vector.Y = Math.Sin(angle * i);
                vector.Normalize();
                cornerPositions.Add(vector + cornerPositions[i - 1]);
            }
            this.cornerPositions = cornerPositions;
            CenterAndScale(width, height);
        }

        public PolygonView(PolygonModel model, List<Vector2> cornerPositions/*Relative to x, y*/, double x, double y, double width, double height) : base(x, y, width, height)
        {
            // Draw model per specifications
            if(model.corners.Count() != cornerPositions.Count()) // Then you gonna have a baaaad time
                throw new ArgumentException("Non-equal amounts of cornerpositions(" + cornerPositions.Count() + ") and corners(" + model.corners.Count() + ")!");

            this.cornerPositions = cornerPositions;
            CenterAndScale(width, height);
        }

        private void CenterAndScale(double width, double height)
        {
            Vector2 min = new Vector2(double.MaxValue, double.MaxValue);
            Vector2 max = new Vector2(double.MinValue, double.MinValue);
            foreach (var corner in cornerPositions)
            {
                if (corner.X < min.X)
                    min.X = corner.X;
                if (corner.Y < min.Y)
                    min.Y = corner.Y;
                if (corner.X > max.X)
                    max.X = corner.X;
                if (corner.Y > max.Y)
                    max.Y = corner.Y;
            }
            max += min;
            double scale = (max.X - width < max.Y - height) ? width / max.X : height / max.Y;

            for (int i = 0; i < cornerPositions.Count();i++) {
                cornerPositions[i] += min;
                cornerPositions[i] *= scale; //  Take the biggest offset and scale accordingly
            }
            
        }

        public override void DrawWithContext(Context context, double offsetX, double offsetY)
        {
            context.Draw(this, offsetX + X, offsetY + X);
        }
    }
}
