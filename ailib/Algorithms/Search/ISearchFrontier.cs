using System.Collections.Generic;

namespace ailib.Algorithms.Search
{
    public interface ISearchFrontier<T>
    {
        void Push(T state);
        T Pop();
        bool Contains(T state);
        IEnumerable<T> GetStates();
        bool IsEmpty();
    }
}
