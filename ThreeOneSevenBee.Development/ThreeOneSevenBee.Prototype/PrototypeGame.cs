﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using ThreeOneSevenBee.Framework;
using ThreeOneSevenBee.Framework.Euclidean;
using ThreeOneSevenBee.Framework.Html;

namespace ThreeOneSevenBee.Prototype
{
    class PrototypeGame : Game
    {
        private Circle circleA;

        private Vector2 circleVelocity;

        private Circle circleB;

        private ImageElement mormot;

        private Rectangle rectangleA;

        private bool isDraggingRectangle = false;

        private Vector2 rectangleDragOffset;

        public PrototypeGame(CanvasElement canvas)
            : base(canvas)
        {
            circleVelocity = new Vector2(0.4, 0.4);

            circleA = new Circle
            {
                Center = new Vector2(200, 200),
                Radius = 48
            };

            circleB = new Circle
            {
                Center = new Vector2(400, 400),
                Radius = 128
            };

            rectangleA = new Rectangle()
            {
                Location = new Vector2(400, 400),
                Width = 96,
                Height = 96
            };

            mormot = new ImageElement
            {
                Src = "http://i.imgur.com/5RNWLm5.jpg"
            };

            Input.MouseDown += InputOnMouseDown;
            Input.MouseUp += InputOnMouseUp;
        }

        private void InputOnMouseDown(MouseButton mouseButton)
        {
            if (rectangleA.Contains(Input.Mouse) && isDraggingRectangle == false)
            {
                Console.WriteLine("Drag begin");
                rectangleDragOffset = rectangleA.Location - Input.Mouse;
                isDraggingRectangle = true;
            }
        }

        private void InputOnMouseUp(MouseButton mouseButton)
        {
            if (isDraggingRectangle == true)
            {
                Console.WriteLine("Drag end");
                isDraggingRectangle = false;
            }
        }

        public override void Update(double deltaTime, double totalTime)
        {
            if (circleA.Center.X >= Canvas.ClientWidth - circleA.Radius || circleA.Center.X <= circleA.Radius)
                circleVelocity.X *= -1;

            if (circleA.Center.Y >= Canvas.ClientHeight - circleA.Radius || circleA.Center.Y <= circleA.Radius)
                circleVelocity.Y *= -1;

            circleA.Center += circleVelocity * deltaTime;

            base.Update(deltaTime, totalTime);
        }

        public override void Draw(double deltaTime, double totalTime)
        {
            Context2D.Clear(HTMLColor.Aquamarine);

            Context2D.DrawImage(mormot, Canvas.Width - (mormot.Width / 2), Canvas.Height - (mormot.Height / 2));

            Context2D.DrawString(50, 50, "Hello, World!", "20px Arial", HTMLColor.Black);

            Context2D.DrawString(50, 70, "Delta: " + deltaTime.ToString("0.00") + "ms", "20px Arial", HTMLColor.Black);

            Context2D.DrawString(50, 90, "Total: " + totalTime.ToString("0.00") + "ms", "20px Arial", HTMLColor.Black);

            if (Input.IsMouseOver == true) // == true is needed because of issue #933 in Bridge.Net.
                Context2D.DrawString(50, 110, Input.Mouse.ToString(), "20px Arial", HTMLColor.Black);
            else
                Context2D.DrawString(50, 110, Input.Mouse.ToString(), "20px Arial", HTMLColor.LightGray);

            if (circleA.Contains(Input.Mouse))
                Context2D.FillCircle(circleA, HTMLColor.Lime);
            Context2D.DrawCircle(circleA, HTMLColor.Black);

            if (circleB.Contains(rectangleA))
                Context2D.FillCircle(circleB, HTMLColor.Lime);
            else
                Context2D.FillCircle(circleB, HTMLColor.Red);
            Context2D.DrawCircle(circleB, HTMLColor.Black);

            if (isDraggingRectangle == true) // == true is needed because of issue #933 in Bridge.Net.
            {
                rectangleA.Location = Input.Mouse + rectangleDragOffset;
                Console.WriteLine("dragging");
            }

            if (rectangleA.Contains(Input.Mouse))
                Context2D.FillRectangle(rectangleA, HTMLColor.Lime);
            else
                Context2D.FillRectangle(rectangleA, HTMLColor.Yellow);
            Context2D.DrawRectangle(rectangleA, HTMLColor.Black);

            base.Draw(deltaTime, totalTime);
        }
    }
}
