using System.Collections.Generic;

namespace ailib.DataStructures
{
    /// <summary>
    /// Queue that dequeues min item first
    /// </summary>
    public class PriorityQueue<T>
    {
        private readonly BinaryMinHeap<T> _minHeap;

        public PriorityQueue(IComparer<T> comparer)
        {
            _minHeap = new BinaryMinHeap<T>(comparer);
        }

        public void Push(T item)
        {
            _minHeap.Add(item);
        }

        public T Pop()
        {
            return _minHeap.RemoveMin();
        }
    }
}
