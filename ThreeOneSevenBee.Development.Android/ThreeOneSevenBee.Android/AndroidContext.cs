using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Views;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using ThreeOneSevenBee.Model.Euclidean;
using ThreeOneSevenBee.Model.UI;

namespace ThreeOneSevenBee.Android
{
    class AndroidContext : Android.Views.View, Model.UI.IContext 
    {
        

        public AndroidContext(double width, double height) : base(width, height)
        {

        }

        double IContext.Height
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        double IContext.Width
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        void IContext.Clear()
        {
            throw new NotImplementedException();
        }

        void IContext.Draw()
        {
            throw new NotImplementedException();
        }

        void IContext.DrawLine(Vector2 first, Vector2 second, Color lineColor, double lineWidth)
        {
            throw new NotImplementedException();
        }

        void IContext.DrawPolygon(Vector2[] path, Color fillColor)
        {
            throw new NotImplementedException();
        }

        void IContext.DrawPolygon(Vector2[] path, Color fillColor, Color lineColor, double lineWidth)
        {
            throw new NotImplementedException();
        }

        void IContext.DrawRectangle(double x, double y, double width, double height, Color fillColor)
        {
            throw new NotImplementedException();
        }

        void IContext.DrawRectangle(double x, double y, double width, double height, Color fillColor, Color lineColor, double lineWidth)
        {
            throw new NotImplementedException();
        }

        void IContext.DrawText(double x, double y, double width, double height, string text, Color textColor)
        {
            throw new NotImplementedException();
        }

        void IContext.SetContentView(Model.UI.View view)
        {
            throw new NotImplementedException();
        }
    }
}