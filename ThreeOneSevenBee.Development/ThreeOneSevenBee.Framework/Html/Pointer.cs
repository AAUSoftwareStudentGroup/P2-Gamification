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

        public int Id { get; private set; }

        public PointerTypes Type { get; private set; }
    }

    public enum PointerTypes
    {
        Mouse,
        Touch
    }
}
