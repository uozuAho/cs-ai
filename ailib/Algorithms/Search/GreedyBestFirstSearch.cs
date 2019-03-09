using System;

namespace ailib.Algorithms.Search
{
    /// <summary>
    /// Greedy best first search. Simply uses the given state scorer to pick the highest score
    /// state to search next.
    /// </summary>
    public class GreedyBestFirstSearch<TState, TAction> : BestFirstSearch<TState, TAction>
    {
        private readonly Func<TState, int> _scoreState;

        public GreedyBestFirstSearch(ISearchProblem<TState, TAction> problem, Func<TState, int> scoreState) : base(problem)
        {
            _scoreState = scoreState;
            // todo: this should be in BestFirst Init function.. or something. have to call same frontier push
            //       in all subclasses
            Frontier.Push(new SearchNode<TState, TAction>(problem.InitialState, null, default(TAction), 0));
        }

        protected override double PriorityFunc(SearchNode<TState, TAction> node)
        {
            return _scoreState(node.State);
        }
    }
}
