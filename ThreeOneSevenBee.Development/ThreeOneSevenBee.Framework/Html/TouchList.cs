// The documentation for this class (on <summary> tags) was extracted from:
// https://developer.mozilla.org/en-US/docs/Web/API/TouchList

using Bridge;
using System.Collections;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Framework.Html
{
    [External]
    [Name("Bridge.TouchList")]
    public class TouchList : IEnumerable<Touch>
    {
        /// <summary>
        /// The number of <see cref="Touch"/> objects in the TouchList.
        /// </summary>
        public readonly int Length;

        //public virtual extern Touch Item(int index);

        /// <summary>
        /// Returns the <see cref="Touch"/> object at the specified index in the list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public extern Touch this[int index] { get; set; }

        /// <summary>
        /// Returns enumerator of <see cref="Touch"/> objects in the TouchList
        /// </summary>
        /// <returns></returns>
        [Template("Bridge.getEnumerator({this})")]
        public extern IEnumerator<Touch> GetEnumerator();

        [Template("Bridge.getEnumerator({this})")]
        extern IEnumerator IEnumerable.GetEnumerator();
    }
}