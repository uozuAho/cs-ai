using System.Collections.Generic;

namespace ailib.DataStructures
{
    public class PriorityQueue<T>
    {
        private readonly BinaryHeap<T> _heap;

        public PriorityQueue(IComparer<T> comparer)
        {
            _heap = new BinaryHeap<T>(comparer);
        }

        public void Push(T item)
        {
            _heap.Add(item);
        }

        public T Pop()
        {
            return _heap.RemoveMin();
        }
    }
}
