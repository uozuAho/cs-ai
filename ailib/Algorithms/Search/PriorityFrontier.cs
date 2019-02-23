using System.Collections.Generic;
using ailib.DataStructures;

namespace ailib.Algorithms.Search
{
    public class PriorityFrontier<TState, TAction> : ISearchFrontier<TState, TAction>
    {
        private readonly PriorityQueue<SearchNode<TState, TAction>> _queue;
        private readonly HashSet<TState> _states;
        
        public PriorityFrontier(IComparer<SearchNode<TState, TAction>> nodeComparer)
        {
            _queue = new PriorityQueue<SearchNode<TState, TAction>>(nodeComparer);
            _states = new HashSet<TState>();
        }
        
        public void Push(SearchNode<TState, TAction> node)
        {
            _states.Add(node.State);
            _queue.Push(node);
        }

        public SearchNode<TState, TAction> Pop()
        {
            var node = _queue.Pop();
            _states.Remove(node.State);
            return node;
        }

        public bool ContainsState(TState state)
        {
            return _states.Contains(state);
        }

        public bool IsEmpty()
        {
            return _states.Count == 0;
        }
    }
}
