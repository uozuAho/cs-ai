namespace ailib.Algorithms.Search.NonDeterministic
{
    public class OrNode<TState, TAction> : IPlanNode<TState, TAction>
    {
        public TAction Action { get; }
        public IPlanNode<TState, TAction> Child { get; }

        public OrNode(TAction action, IPlanNode<TState, TAction> child)
        {
            Action = action;
            Child = child;
        }
    }
}