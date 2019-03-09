using System;

namespace ailib.Algorithms.Search
{
    public class AStarSearch<TState, TAction> : BestFirstSearch<TState, TAction>
    {
        private readonly Func<TState, int> _scoreState;

        /// <summary>
        /// Initialise the search. Note that lower scores are searched first.
        /// todo: make this more intuitive. Higher score sounds better, but doesn't
        /// work as work this current implementation, since we want to minimise path cost
        /// </summary>
        public AStarSearch(ISearchProblem<TState, TAction> problem, Func<TState, int> scoreState) : base(problem)
        {
            _scoreState = scoreState;
            Frontier.Push(new SearchNode<TState, TAction>(problem.InitialState, null, default(TAction), 0));
        }

        /// <summary>
        /// f(n)    priority func of n
        /// = g(n)  path cost of n
        /// + h(n)  plus heuristic (score) of n
        /// </summary>
        protected override int PriorityFunc(SearchNode<TState, TAction> node)
        {
            // todo: should priority func be a double?
            return (int) node.PathCost + _scoreState(node.State);
        }
    }
}