using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Framework.Html
{
    /// <summary>
    /// A unified Touch/Mouse input system.
    /// </summary>
    /// <remarks>Loosely based on https://developer.mozilla.org/en-US/docs/Web/API/PointerEvent. </remarks>
    public class Pointer
    {
        public Pointer(int id, PointerTypes type)
        {
            this.Id = id;
            this.Type = type;
        }

        internal Pointer(MouseEvent e, Element relativeTo)
            : this(0, PointerTypes.Touch)
        {
            // mouse id is always 0
            this.ScreenX = e.ScreenX;
            this.ScreenY = e.ScreenY;
            this.ClientX = e.ClientX;
            this.ClientY = e.ClientY;
            this.PageX = e.PageX;
            this.PageY = e.PageY;
            this.Target = e.Target;
            this.RelativeX = e.PageX - relativeTo.OffsetLeft;
            this.RelativeX = e.PageY - relativeTo.OffsetTop;
        }

        internal Pointer(Touch e, Element relativeTo)
            : this(e.Identifier, PointerTypes.Touch)
        {
            // touch id is browser controlled
            this.ScreenX = e.ScreenX;
            this.ScreenY = e.ScreenY;
            this.ClientX = e.ClientX;
            this.ClientY = e.ClientY;
            this.PageX = e.PageX;
            this.PageY = e.PageY;
            this.Target = e.Target;
            this.RelativeX = e.PageX - relativeTo.OffsetLeft;
            this.RelativeY = e.PageY - relativeTo.OffsetTop;
        }

        internal void Update(MouseEvent e, Element relativeTo)
        {
            if (!(this.Type == PointerTypes.Mouse))
                throw new InvalidOperationException("Tried to update " + this.Type + " pointer with mouse data.");

            this.ScreenX = e.ScreenX;
            this.ScreenY = e.ScreenY;
            this.ClientX = e.ClientX;
            this.ClientY = e.ClientY;
            this.PageX = e.PageX;
            this.PageY = e.PageY;
            this.Target = e.Target;
            this.RelativeX = e.PageX - relativeTo.OffsetLeft;
            this.RelativeY = e.PageY - relativeTo.OffsetTop;
        }

        internal void Update(Touch e, Element relativeTo)
        {
            if (!(this.Type == PointerTypes.Touch))
                throw new InvalidOperationException("Tried to update " + this.Type + " pointer with touch data.");

            this.ScreenX = e.ScreenX;
            this.ScreenY = e.ScreenY;
            this.ClientX = e.ClientX;
            this.ClientY = e.ClientY;
            this.PageX = e.PageX;
            this.PageY = e.PageY;
            this.Target = e.Target;
            this.RelativeX = e.PageX - relativeTo.OffsetLeft;
            this.RelativeY = e.PageY - relativeTo.OffsetTop;
        }

        public int Id { get; private set; }

        public PointerTypes Type { get; private set; }

        /// <summary>
        /// Returns the X coordinate of the pointer relative to the left edge of the screen.
        /// </summary>
        public int ScreenX { get; private set; }

        /// <summary>
        /// Returns the Y coordinate of the pointer relative to the top edge of the screen.
        /// </summary>
        public int ScreenY { get; private set; }

        /// <summary>
        /// Returns the X coordinate of the pointer relative to the left edge of the browser viewport, not including any scroll offset.
        /// </summary>
        public int ClientX { get; private set; }

        /// <summary>
        /// Returns the Y coordinate of the pointer relative to the top edge of the browser viewport, not including any scroll offset.
        /// </summary>
        public int ClientY { get; private set; }

        /// <summary>
        /// Returns the X coordinate of the pointer relative to the left edge of the document. Unlike clientX, this value includes the horizontal scroll offset, if any.
        /// </summary>
        public int PageX { get; private set; }

        /// <summary>
        /// Returns the Y coordinate of the pointer relative to the top of the document. Unlike clientY, this value includes the vertical scroll offset, if any.
        /// </summary>
        public int PageY { get; private set; }

        public int RelativeX { get; private set; }

        public int RelativeY { get; private set; }

        /// <summary>
        /// Returns the <see cref="Bridge.Html5.Element"/> on which the pointer started when it was first placed on the surface, even if the pointer has since moved outside the interactive area of that element or even been removed from the document.
        /// </summary>
        public Element Target { get; private set; }
    }

    public enum PointerTypes
    {
        Mouse,
        Touch
    }
}
