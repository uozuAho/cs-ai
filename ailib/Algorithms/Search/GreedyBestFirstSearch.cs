using System;

namespace ailib.Algorithms.Search
{
    /// <summary>
    /// Greedy best first search. Expands nodes in minimum order given by the heuristic
    /// </summary>
    public class GreedyBestFirstSearch<TState, TAction> : BestFirstSearch<TState, TAction>
    {
        private readonly Func<TState, int> _heuristic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="problem"></param>
        /// <param name="heuristic">An estimate of the cost to reach the goal from the given state</param>
        public GreedyBestFirstSearch(ISearchProblem<TState, TAction> problem, Func<TState, int> heuristic) : base(problem)
        {
            _heuristic = heuristic;
            // todo: this should be in BestFirst Init function.. or something. have to call same frontier push
            //       in all subclasses
            Frontier.Push(new SearchNode<TState, TAction>(problem.InitialState, null, default(TAction), 0));
        }

        protected override double PriorityFunc(SearchNode<TState, TAction> node)
        {
            return _heuristic(node.State);
        }
    }
}
