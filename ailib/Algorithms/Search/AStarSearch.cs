using System;

namespace ailib.Algorithms.Search
{
    public class AStarSearch<TState, TAction> : BestFirstSearch<TState, TAction>
    {
        private readonly Func<TState, int> _heuristic;

        /// <summary>
        /// Initialise the search. Note that lower scores are searched first.
        /// </summary>
        /// <param name="problem">The search problem</param>
        /// <param name="heuristic">An estimate of the cost to reach the goal from the given state</param>
        public AStarSearch(ISearchProblem<TState, TAction> problem, Func<TState, int> heuristic) : base(problem)
        {
            _heuristic = heuristic;
            Frontier.Push(new SearchNode<TState, TAction>(problem.InitialState, null, default(TAction), 0));
        }

        /// <summary>
        /// f(n)    priority func of n
        /// = g(n)  path cost of n
        /// + h(n)  plus heuristic of n
        /// </summary>
        protected override double PriorityFunc(SearchNode<TState, TAction> node)
        {
            return node.PathCost + _heuristic(node.State);
        }
    }
}