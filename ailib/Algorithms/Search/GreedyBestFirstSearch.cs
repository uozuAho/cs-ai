using System;

namespace ailib.Algorithms.Search
{
    /// <summary>
    /// Greedy best first search. Simply uses the given heuristic as the priority function
    /// </summary>
    public class GreedyBestFirstSearch<TState, TAction> : BestFirstSearch<TState, TAction>
    {
        private readonly Func<TState, int> _heuristic;

        public GreedyBestFirstSearch(ISearchProblem<TState, TAction> problem, Func<TState, int> heuristic) : base(problem)
        {
            _heuristic = heuristic;
        }

        protected override int PriorityFunc(TState state)
        {
            return _heuristic(state);
        }
    }
}
