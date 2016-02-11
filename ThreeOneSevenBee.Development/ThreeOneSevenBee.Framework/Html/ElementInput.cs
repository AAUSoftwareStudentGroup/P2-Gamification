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
        public ElementInput(Element element)
        {
            Element = element;
            Element.OnMouseEnter = OnMouseEnter;
            Element.OnMouseMove = OnMouseMove;
            Element.OnMouseLeave = OnMouseLeave;
        }

        private void OnMouseEnter(MouseEvent mouseEvent)
        {
            Console.WriteLine("OnMouseEnter");
            IsMouseOver = true;
        }

        private void OnMouseMove(MouseEvent mouseEvent)
        {
            Console.WriteLine("OnMouseMove");
            var pageX = (int)mouseEvent["pageX"];
            var pageY = (int)mouseEvent["pageY"];
            var relativeX = pageX - Element.OffsetLeft;
            var relativeY = pageY - Element.OffsetTop;
            Mouse = new Vector2(relativeX, relativeY);
        }

        private void OnMouseLeave(MouseEvent mouseEvent)
        {
            Console.WriteLine("OnMouseLeave");
            IsMouseOver = false;
        }

        public Element Element { get; private set; }

        public bool IsMouseOver { get; private set; }

        public Vector2 Mouse { get; private set; }
    }
}
