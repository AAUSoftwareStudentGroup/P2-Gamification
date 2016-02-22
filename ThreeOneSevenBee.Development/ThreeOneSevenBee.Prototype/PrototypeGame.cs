using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using Bridge.jQuery2;
using ThreeOneSevenBee.Framework;
using ThreeOneSevenBee.Framework.Euclidean;
using ThreeOneSevenBee.Framework.Html;

namespace ThreeOneSevenBee.Prototype
{
    class PrototypeGame : Game
    {
        private Circle circleA;
        private Circle circleB;
        private Rectangle rectangleA;
        private Rectangle rectangleB;
        private bool isDraggingRectangle = false;
        private Vector2 rectangleDragOffset;
        private bool isDraggingCircle = false;
        private bool didIt = false;
        private Vector2 circleDragOffset;
        private int globalCounter = 0;

        public PrototypeGame(CanvasElement canvas) : base(canvas)
        {
            rectangleA = new Rectangle()
            {
                Location = new Vector2(60, 260),
                Width = 100,
                Height = 100
            };

            rectangleB = new Rectangle()
            {
                Location = new Vector2(10, 10),
                Width = 200,
                Height = 200
            };

            circleA = new Circle
            {
                Center = new Vector2(320, 320),
                Radius = 50
            };

            circleB = new Circle
            {
                Center = new Vector2(320, 110),
                Radius = 100
            };

            //Input.MouseDown += InputOnMouseDown;
            //Input.MouseUp += InputOnMouseUp;
        }

        /*private void InputOnMouseDown(MouseButton mouseButton)
        {
            if (rectangleA.Contains(Input.Mouse) && isDraggingRectangle == false)
            {
                rectangleDragOffset = rectangleA.Location - Input.Mouse;
                isDraggingRectangle = true;
            }

            if (circleA.Contains(Input.Mouse) && isDraggingCircle == false)
            {
                circleDragOffset = circleA.Center - Input.Mouse;
                isDraggingCircle = true;
            }
        }

        private void InputOnMouseUp(MouseButton mouseButton)
        {
            isDraggingRectangle = false;
            isDraggingCircle = false;

            if (circleB.Contains(Input.Mouse))
            {
                jQuery.Ajax(
                    new AjaxOptions()
                    {
                        Url = "/api/?func=AddCount",
                        Cache = false,
                        Success = (data, textStatus, request) =>
                        {
                            int tempCounter;
                            if (Int32.TryParse((string)data, out tempCounter))
                                globalCounter = tempCounter;
                        }
                    }
                );
            }
        }*/

        public override void Update(double deltaTime, double totalTime)
        {


            base.Update(deltaTime, totalTime);
        }

        public override void Draw(double deltaTime, double totalTime)
        {
            Context2D.Clear(HTMLColor.CornflowerBlue);

            if (isDraggingRectangle == true) // == true is needed because of issue #933 in Bridge.Net.
            {
                //rectangleA.Location = Input.Mouse + rectangleDragOffset;
            }

            if (isDraggingCircle == true) // == true is needed because of issue #933 in Bridge.Net.
            {
                //circleA.Center = Input.Mouse + circleDragOffset;
            }


            if (circleB.Contains(rectangleA))
                Context2D.FillCircle(circleB, HTMLColor.Lime);
            else
                Context2D.FillCircle(circleB, HTMLColor.Red);
            Context2D.DrawCircle(circleB, HTMLColor.Black);

            if (rectangleB.Contains(circleA))
                Context2D.FillRectangle(rectangleB, HTMLColor.Lime);
            else
                Context2D.FillRectangle(rectangleB, HTMLColor.Red);
            Context2D.DrawRectangle(rectangleB, HTMLColor.Black);

            /*if (circleA.Contains(Input.Mouse))
                Context2D.FillCircle(circleA, HTMLColor.Lime);
            else*/
                Context2D.FillCircle(circleA, HTMLColor.Yellow);
            Context2D.DrawCircle(circleA, HTMLColor.Black);

            /*if (rectangleA.Contains(Input.Mouse))
                Context2D.FillRectangle(rectangleA, HTMLColor.Lime);
            else*/
                Context2D.FillRectangle(rectangleA, HTMLColor.Yellow);
            Context2D.DrawRectangle(rectangleA, HTMLColor.Black);

            Context2D.DrawString(5, 200, "Counter: " + globalCounter, "20px Arial", HTMLColor.Black);
            Context2D.DrawString(5, 400, "Place the smaller shapes in the larger shapes of the opposite type.", "20px Arial", HTMLColor.Black);
            if (circleB.Contains(rectangleA) && rectangleB.Contains(circleA))
            {
                Context2D.DrawString(200, 420, "You did it!", "20px Arial", HTMLColor.Black);
                didIt = true;
            }
            else if (didIt == true)
            {
                Context2D.DrawString(200, 420, "You undid it! Tanner Helland you fuck!", "20px Arial", HTMLColor.Black);
            }

            var i = 1;
            Context2D.DrawString(2, 2 + 20 * i++, "Touches", "20px Arial", HTMLColor.Black);
            /*foreach (var id in Input.Identifier)
            {
                Context2D.DrawString(2, 2 + 20 * i++, id.ToString(), "20px Arial", HTMLColor.Black);
            }*/

            base.Draw(deltaTime, totalTime);
        }
    }
}
