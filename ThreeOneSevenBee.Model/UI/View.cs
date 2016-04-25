﻿using System;
using ThreeOneSevenBee.Model.Euclidean;

namespace ThreeOneSevenBee.Model.UI
{
    public class View
    {
        public virtual double Width { get; set; }
        public virtual double Height { get; set; }
        public virtual double X { get; set; }
        public virtual double Y { get; set; }
        public virtual bool Active { get; set; }

        public double Baseline { get; set; }

        public Color BackgroundColor { get; set; }

        public bool Visible { get; set; }

        public View(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Baseline = height / 2;
            BackgroundColor = new Color();
            Visible = true;
        }

        public virtual void DrawWithContext(IContext context, double offsetX, double offsetY)
        {
            context.DrawRectangle(X + offsetX, Y + offsetY, Width, Height, Active? new Color(0, 255, 0) : new Color(0, 255, 255));
        }

        public virtual void Click(double x, double y)
        {
            if (ContainsPoint(x, y))
            {
                if (OnClick != null)
                {
                    OnClick();
                }
                Active = true;
            } else
            {
                Active = false;
            }
        }
        public virtual void KeyPressed(int key)
        {
            if(OnKeyPressed != null && Active)
                OnKeyPressed(key);
        }

        public Action OnClick;
        public Action<int> OnKeyPressed;
        public Action OnChanged;
        
        public virtual View Scale(double factor)
        {
            X *= factor;
            Y *= factor;
            Width *= factor;
            Height *= factor;
            return this;
        }

        public virtual bool ContainsPoint(double x, double y)
        {
            return x >= X && y >= Y && x <= X + Width && y <= Y + Height;
        }
    }
}
