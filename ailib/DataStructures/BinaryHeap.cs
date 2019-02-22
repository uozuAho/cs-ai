using System;
using System.Collections.Generic;

namespace ailib.DataStructures
{
    /// BinaryHeap, implemented with array storage
    public class BinaryHeap<T>
    {
        public int Size => _buf.Count;

        private readonly List<T> _buf;
        private readonly IComparer<T> _comparer;

        /**
         * @param compare Optional comparator. Default is a < b ? -1 : a > b ? 1 : 0.
         *                Reverse this to make a max heap.
         */
        public BinaryHeap(IComparer<T> comparer)
        {
            _buf = new List<T>();
            _comparer = comparer;
        }

        public void Add(T item)
        {
            _buf.Add(item);
            Swim(Size - 1);
        }

        public void Remove(T item)
        {
            if (Size == 0) throw new InvalidOperationException("cannot remove from empty");

            var idx = IndexOf(item, 0);

            if (idx == -1) throw new InvalidOperationException("cannot remove item not in heap");

            RemoveAtIdx(idx);
        }

        public bool Contains(T item)
        {
            return IndexOf(item, 0) >= 0;
        }

        /** Remove the minimum item */
        public T RemoveMin()
        {
            if (Size == 0) throw new InvalidOperationException("cannot remove from empty");
            return RemoveAtIdx(0);
        }

        /** Get the minimum item without removing it */
        public T PeekMin()
        {
            if (Size == 0) throw new InvalidOperationException("cannot peek when empty");
            return _buf[0];
        }

        private T RemoveAtIdx(int idx)
        {
            if (idx >= Size) throw new ArgumentOutOfRangeException();

            // swap item at idx and last item
            var temp = _buf[idx];
            var lastIdx = Size - 1;
            _buf[idx] = _buf[lastIdx];
            _buf.RemoveAt(lastIdx);
            // sink last item placed at idx
            Sink(idx);
            return temp;
        }

        /** Return the first found index of the given item, else -1
         *  @param subroot node to start the search.
         */
        private int IndexOf(T item, int subRoot)
        {
            if (subRoot >= Size) {
                // gone past leaf
                return -1;
            }
            if (_comparer.Compare(item, _buf[subRoot]) == -1)
            {
                // item is less than current node - will not be in this subtree
                return -1;
            }
            if (item.Equals(_buf[subRoot]))
            {
                return subRoot;
            }

            var idx = IndexOf(item, subRoot * 2 + 1);
            return idx >= 0
                ? idx
                : IndexOf(item, subRoot * 2 + 2);
        }

        private void Swim(int idx)
        {
            if (idx == 0) return;

            var parentIdx = (idx - 1) / 2;

            while (_comparer.Compare(_buf[idx], _buf[parentIdx]) == -1)
            {
                // swap if child < parent
                Swap(idx, parentIdx);
                if (parentIdx == 0) return;
                idx = parentIdx;
                parentIdx = (idx - 1) / 2;
            }
        }

        private void Sink(int idx)
        {
            while (true)
            {
                var leftIdx = 2 * idx + 1;
                var rightIdx = 2 * idx + 2;
                // stop if no children
                if (leftIdx >= Size) return;
                // get minimum child
                var minIdx = leftIdx;
                if (rightIdx < Size && _comparer.Compare(_buf[rightIdx], _buf[leftIdx]) == -1)
                {
                    minIdx = rightIdx;
                }
                // swap if parent > min child
                if (_comparer.Compare(_buf[idx], _buf[minIdx]) == 1)
                {
                    Swap(idx, minIdx);
                }
                else
                {
                    return;
                }
                idx = minIdx;
            }
        }

        /** swap items at given indexes */
        private void Swap(int idxA, int idxB)
        {
            var temp = _buf[idxA];
            _buf[idxA] = _buf[idxB];
            _buf[idxB] = temp;
        }
    }
}
