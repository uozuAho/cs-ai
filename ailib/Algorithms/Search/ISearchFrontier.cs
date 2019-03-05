namespace ailib.Algorithms.Search
{
    internal interface ISearchFrontier<TState, TAction>
    {
        void Push(SearchNode<TState, TAction> node);
        SearchNode<TState, TAction> Pop();
        bool ContainsState(TState state);
        bool IsEmpty();
    }
}
