using System.Collections.Generic;

namespace ailib.Algorithms.Search
{
    public class FifoFrontier<T> : ISearchFrontier<T>
    {
        private readonly Queue<T> _queue;
        private readonly HashSet<T> _set;

        public FifoFrontier()
        {
            _queue = new Queue<T>();
            _set = new HashSet<T>();
        }
        
        public void Push(T state)
        {
            _set.Add(state);
            _queue.Enqueue(state);
        }

        public T Pop()
        {
            var item = _queue.Dequeue();
            _set.Remove(item);
            return item;
        }

        public bool Contains(T state)
        {
            return _set.Contains(state);
        }

        public IEnumerable<T> GetStates()
        {
            using (var enumerator = _queue.GetEnumerator())
            {
                if (enumerator.Current == null) yield break;

                while (enumerator.Current != null)
                {
                    yield return enumerator.Current;
                    enumerator.MoveNext();
                }                
            }
        }

        public bool IsEmpty()
        {
            return _queue.Count == 0;
        }
    }
}