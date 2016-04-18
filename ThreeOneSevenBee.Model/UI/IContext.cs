using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Euclidean;

namespace ThreeOneSevenBee.Model.UI
{
    public interface IContext
    {
        double Width { get; }
        double Height { get; }
        void Clear();
        void SetContentView(View view);
        void Draw();

        void DrawRectangle(double x, double y, double width, double height, Color fillColor);
        void DrawLine(Vector2 first, Vector2 second, Color lineColor, double lineWidth);
        void DrawRectangle(double x, double y, double width, double height, Color fillColor, Color lineColor, double lineWidth);
        void DrawPolygon(Vector2[] path, Color fillColor);
        void DrawPolygon(Vector2[] path, Color fillColor, Color lineColor, double lineWidth);
        void DrawText(double x, double y, double width, double height, string text, Color textColor);
    }
}
