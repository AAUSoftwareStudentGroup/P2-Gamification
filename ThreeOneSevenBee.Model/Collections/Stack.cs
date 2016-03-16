using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Collections
{
    public class Stack<T> : IEnumerable<T>
    {
        private List<T> list;

        public Stack()
        {
            list = new List<T>();
        }

        public int Count { get { return list.Count; } }

        public void Push(T item)
        {
            list.Add(item);
        }

        public T Peek()
        {
            if (list.Count == 0)
                throw new InvalidOperationException("The Queue is empty.");

            return list[list.Count - 1];
        }

        public T Pop()
        {
            if (list.Count == 0)
                throw new InvalidOperationException("The Queue is empty.");

            var item = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
