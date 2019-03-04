using System.Collections.Generic;

namespace ailib.DataStructures
{
    /// <summary>
    /// Queue that dequeues max item first
    /// </summary>
    public class PriorityQueue<T>
    {
        /// <summary>
        /// Min heap is turned into a max heap by reversing the given comparer
        /// </summary>
        private readonly BinaryMinHeap<T> _maxHeap;

        public PriorityQueue(IComparer<T> comparer)
        {
            _maxHeap = new BinaryMinHeap<T>(new ReverseComparer<T>(comparer));
        }

        public void Push(T item)
        {
            _maxHeap.Add(item);
        }

        public T Pop()
        {
            return _maxHeap.RemoveMin();
        }

        private class ReverseComparer<T2> : IComparer<T2>
        {
            private readonly IComparer<T2> _comparer;

            public ReverseComparer(IComparer<T2> comparer)
            {
                _comparer = comparer;
            }
            
            public int Compare(T2 x, T2 y)
            {
                var val = _comparer.Compare(x, y);
                return val == -1 ? 1 : val == 1 ? -1 : 0;
            }
        }
    }
}
