using System;

namespace ailib.Algorithms.Search
{
    public class AStarSearch<TState, TAction> : BestFirstSearch<TState, TAction>
    {
        private readonly Func<TState, int> _scoreState;

        public AStarSearch(ISearchProblem<TState, TAction> problem, Func<TState, int> scoreState) : base(problem)
        {
            _scoreState = scoreState;
            Frontier.Push(new SearchNode<TState, TAction>(problem.InitialState, null, default(TAction), 0));
        }

        /// <summary>
        /// f(n)    priority func of n
        /// = g(n)  path cost of n
        /// + h(n)  plus heuristic of n
        /// </summary>
        protected override int PriorityFunc(SearchNode<TState, TAction> node)
        {
            // todo: should priority func be a double?
            return (int) node.PathCost + _scoreState(node.State);
        }
    }
}