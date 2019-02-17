namespace ailib.Algorithms.Search
{
    public class BreadthFirstSearch<TState, TAction> : GenericSearch<TState, TAction>
    {
        public BreadthFirstSearch(ISearchProblem<TState, TAction> problem) : base(problem)
        {
            _frontier = new FifoFrontier<SearchNode<TState, TAction>>();
            _frontier.Push(new SearchNode<TState, TAction>(problem.InitialState, null, default(TAction), 0));
        }
    }
}
