namespace ailib.Algorithms.Search
{
    internal interface ISearchFrontier<TState, TAction>
    {
        void Push(SearchNode<TState, TAction> state);
        SearchNode<TState, TAction> Pop();
        bool ContainsState(TState state);
        bool IsEmpty();
    }
}
