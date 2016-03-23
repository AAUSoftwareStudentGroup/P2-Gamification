using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace threeonesevenbee.Model.Collections
{
    public class Queue<T> : IEnumerable<T>
    {
        private List<T> list;

        public Queue()
        {
            list = new List<T>();
        }

        public int Count { get { return list.Count; } }

        public void Enqueue(T item)
        {
            list.Add(item);
        }

        public T Dequeue()
        {
            if (list.Count == 0)
                throw new InvalidOperationException("The Queue is empty.");
            var item = list[0];
            list.RemoveAt(0);
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
