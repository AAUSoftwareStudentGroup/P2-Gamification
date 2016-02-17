using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using ThreeOneSevenBee.Framework.Euclidean;

namespace ThreeOneSevenBee.Framework.Html
{
    public class ElementInput
    {
        public event Action<MouseButton> MouseDown;

        public event Action<MouseButton> MouseUp;

        public ElementInput(Element element)
        {
            Element = element;
            Element.OnMouseEnter = OnMouseEnter;
            Element.OnMouseMove = OnMouseMove;
            Element.OnMouseLeave = OnMouseLeave;
            Element.OnMouseDown = OnMouseDown;
            Element.OnMouseUp = OnMouseUp;
        }

        private void OnMouseEnter(MouseEvent mouseEvent)
        {
            IsMouseOver = true;
        }

        private void OnMouseMove(MouseEvent mouseEvent)
        {
            var pageX = (Double)mouseEvent["pageX"];
            var pageY = (Double)mouseEvent["pageY"];
            var relativeX = pageX - Element.OffsetLeft;
            var relativeY = pageY - Element.OffsetTop;
            Mouse = new Vector2(relativeX, relativeY);
        }

        private void OnMouseLeave(MouseEvent mouseEvent)
        {
            IsMouseOver = false;
        }

        private void OnMouseDown(MouseEvent mouseEvent)
        {
            MouseButtonState = (MouseButtons)mouseEvent.Buttons;
            var mouseDown = this.MouseDown;
            if (mouseDown != null)
                mouseDown((MouseButton)mouseEvent.Button);
        }

        private void OnMouseUp(MouseEvent mouseEvent)
        {
            MouseButtonState = (MouseButtons)mouseEvent.Buttons;
            var mouseUp = this.MouseUp;
            if (mouseUp != null)
                mouseUp((MouseButton)mouseEvent.Button);
        }

        public Element Element { get; private set; }

        public bool IsMouseOver { get; private set; }

        public Vector2 Mouse { get; private set; }

        public MouseButtons MouseButtonState;
    }

    public enum MouseButton
    {
        Left = 0,
        Middle = 1,
        Right = 2
    }

    [Flags]
    public enum MouseButtons
    {
        Left = 1 << 0,
        Right = 1 << 1,
        Middle = 1 << 2,
        X1 = 1 << 3,
        X2 = 1 << 4
    }
}
