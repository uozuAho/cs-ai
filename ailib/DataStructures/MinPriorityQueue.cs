using System.Collections.Generic;

namespace ailib.DataStructures
{
    /// <summary>
    /// Queue that dequeues min item first
    /// </summary>
    public class MinPriorityQueue<T>
    {
        private readonly BinaryMinHeap<T> _minHeap;

        public MinPriorityQueue(IComparer<T> comparer)
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
