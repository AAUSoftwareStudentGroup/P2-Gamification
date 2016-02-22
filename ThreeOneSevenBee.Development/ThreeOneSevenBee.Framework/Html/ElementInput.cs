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
        public const int MOUSE_ID = 0;

        public event Action<Pointer> PointerEnter;

        public event Action<Pointer> PointerDown;

        public event Action<Pointer> PointerMove;

        public event Action<Pointer> PointerUp;

        public event Action<Pointer> PointerLeave;

        private Dictionary<int, Pointer> pointers;

        public ElementInput(Element element)
        {
            pointers = new Dictionary<int, Pointer>();
            Element = element;
            Element.OnMouseEnter = OnMouseEnter;
            Element.OnMouseMove = OnMouseMove;
            Element.OnMouseLeave = OnMouseLeave;
            Element.OnMouseDown = OnMouseDown;
            Element.OnMouseUp = OnMouseUp;

            Element.AddEventListener("touchstart", OnTouchStart);
            Element.AddEventListener("touchmove", OnTouchMove);
            Element.AddEventListener("touchend", OnTouchEnd);
        }

        public Element Element { get; private set; }

        public IEnumerable<Pointer> Pointers { get { return pointers.Values; } }

        private void OnMouseEnter(MouseEvent e)
        {
            Pointer pointer;
            if (!pointers.TryGetValue(MOUSE_ID, out pointer))
                pointers[MOUSE_ID] = pointer = new Pointer(MOUSE_ID, PointerTypes.Mouse);
            pointer.Update(e, Element);

            var pointerEnter = PointerEnter;
            if (pointerEnter != null)
                pointerEnter(pointer);
        }

        private void OnMouseDown(MouseEvent e)
        {
            Pointer pointer;
            if (!pointers.TryGetValue(MOUSE_ID, out pointer))
                pointers[MOUSE_ID] = pointer = new Pointer(MOUSE_ID, PointerTypes.Mouse);
            pointer.Update(e, Element);

            var pointerDown = PointerDown;
            if (pointerDown != null)
                pointerDown(pointer);
        }

        private void OnMouseMove(MouseEvent e)
        {
            Pointer pointer;
            if (!pointers.TryGetValue(MOUSE_ID, out pointer))
                pointers[MOUSE_ID] = pointer = new Pointer(MOUSE_ID, PointerTypes.Mouse);
            pointer.Update(e, Element);

            var pointerMove = PointerMove;
            if (pointerMove != null)
                pointerMove(pointer);
        }

        private void OnMouseUp(MouseEvent e)
        {
            Pointer pointer;
            if (!pointers.TryGetValue(MOUSE_ID, out pointer))
                pointers[MOUSE_ID] = pointer = new Pointer(MOUSE_ID, PointerTypes.Mouse);
            pointer.Update(e, Element);

            var pointerUp = PointerUp;
            if (pointerUp != null)
                pointerUp(pointer);

            pointers.Remove(MOUSE_ID);
        }

        private void OnMouseLeave(MouseEvent e)
        {
            Pointer pointer;
            if (!pointers.TryGetValue(MOUSE_ID, out pointer))
                pointers[MOUSE_ID] = pointer = new Pointer(MOUSE_ID, PointerTypes.Mouse);
            pointer.Update(e, Element);

            var pointerLeave = PointerLeave;
            if (pointerLeave != null)
                pointerLeave(pointer);

            pointers.Remove(MOUSE_ID);
        }

        private void OnTouchStart(Event e)
        {
            var touchEvent = (e as TouchEvent);
            foreach (var touch in touchEvent.ChangedTouches)
            {
                Pointer pointer;
                if (!pointers.TryGetValue(touch.Identifier, out pointer))
                    pointers[touch.Identifier] = pointer = new Pointer(touch.Identifier, PointerTypes.Touch);
                pointer.Update(touch, Element);

                var pointerDown = PointerDown;
                if (pointerDown != null)
                    pointerDown(pointer);
            }
        }

        private void OnTouchMove(Event e)
        {
            var touchEvent = (e as TouchEvent);
            foreach (var touch in touchEvent.ChangedTouches)
            {
                Pointer pointer;
                if (!pointers.TryGetValue(touch.Identifier, out pointer))
                    pointers[touch.Identifier] = pointer = new Pointer(touch.Identifier, PointerTypes.Mouse);
                pointer.Update(touch, Element);

                var pointerMove = PointerMove;
                if (pointerMove != null)
                    pointerMove(pointer);
            }
        }

        private void OnTouchEnd(Event e)
        {
            var touchEvent = (e as TouchEvent);
            foreach (var touch in touchEvent.ChangedTouches)
            {
                Pointer pointer;
                if (!pointers.TryGetValue(touch.Identifier, out pointer))
                    pointers[touch.Identifier] = pointer = new Pointer(touch.Identifier, PointerTypes.Mouse);
                pointer.Update(touch, Element);

                var pointerUp = PointerUp;
                if (pointerUp != null)
                    pointerUp(pointer);

                pointers.Remove(touch.Identifier);
            }
        }
    }
}
