using System;
using System.Collections.Generic;

namespace ailib.Algorithms.Search
{
    public abstract class BestFirstSearch<TState, TAction> : GenericSearch<TState, TAction>
    {
        protected BestFirstSearch(ISearchProblem<TState, TAction> problem) : base(problem)
        {
            var nodeComparer = new SearchNodeComparer(CompareStates);
            Frontier = new PriorityFrontier<TState, TAction>(nodeComparer);
        }
        
        protected abstract int PriorityFunc(SearchNode<TState, TAction> node);

        private int CompareStates(SearchNode<TState, TAction> a, SearchNode<TState, TAction> b)
        {
            var priorityA = PriorityFunc(a);
            var priorityB = PriorityFunc(b);
            return priorityA < priorityB ? -1 : priorityA > priorityB ? 1 : 0;
        }
        
        private class SearchNodeComparer : IComparer<SearchNode<TState, TAction>>
        {
            private readonly Func<SearchNode<TState, TAction>, SearchNode<TState, TAction>, int> _compare;

            public SearchNodeComparer(Func<SearchNode<TState, TAction>, SearchNode<TState, TAction>, int> compare)
            {
                _compare = compare;
            }
            
            public int Compare(SearchNode<TState, TAction> x, SearchNode<TState, TAction> y)
            {
                if (x == null) throw new NullReferenceException(nameof(x));
                if (y == null) throw new NullReferenceException(nameof(y));
                
                return _compare(x, y);
            }
        }
    }
}
