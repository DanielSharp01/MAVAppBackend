using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Debugger
{
    public class RollingList<T> : IList<T>
    {
        private readonly List<T> implementation = new List<T>();

        public RollingList(int maxCount)
        {
            MaxCount = maxCount;
        }

        public T this[int index] { get => implementation[index]; set => implementation[index] = value; }

        public int Count => implementation.Count;

        public int MaxCount { get; }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            implementation.Clear();
        }

        public bool Contains(T item)
        {
            return implementation.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            implementation.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return implementation.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return implementation.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            implementation.Insert(index, item);
            if (implementation.Count > MaxCount)
            {
                implementation.RemoveAt(0);
            }
        }

        public bool Remove(T item)
        {
            return implementation.Remove(item);
        }

        public void RemoveAt(int index)
        {
            implementation.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return implementation.GetEnumerator();
        }
    }
}
