using System.Collections.Generic;

namespace ailib.Algorithms.Search
{
    internal class FifoFrontier<TState, TAction> : ISearchFrontier<TState, TAction>
    {
        private readonly Queue<SearchNode<TState, TAction>> _queue;
        private readonly HashSet<TState> _states;

        public FifoFrontier()
        {
            _queue = new Queue<SearchNode<TState, TAction>>();
            _states = new HashSet<TState>();
        }
        
        public void Push(SearchNode<TState, TAction> node)
        {
            _states.Add(node.State);
            _queue.Enqueue(node);
        }

        public SearchNode<TState, TAction> Pop()
        {
            var node = _queue.Dequeue();
            _states.Remove(node.State);
            return node;
        }

        public bool ContainsState(TState state)
        {
            return _states.Contains(state);
        }

        public bool IsEmpty()
        {
            return _queue.Count == 0;
        }
    }
}