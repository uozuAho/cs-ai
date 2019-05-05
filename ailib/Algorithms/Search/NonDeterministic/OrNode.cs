namespace ailib.Algorithms.Search.NonDeterministic
{
    internal class OrNode<TState, TAction> : IPlanNode<TState, TAction>
    {
        public TAction Action { get; }
        public IPlanNode<TState, TAction> Child { get; set; }

        public OrNode(TAction action)
        {
            Action = action;
        }
    }
}