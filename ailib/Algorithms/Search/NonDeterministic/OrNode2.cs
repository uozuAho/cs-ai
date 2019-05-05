namespace ailib.Algorithms.Search.NonDeterministic
{
    public class OrNode2<TState, TAction> : IPlanNode<TState, TAction>
    {
        public TAction Action { get; }
        public IPlanNode<TState, TAction> Child { get; set; }

        public OrNode2(TAction action)
        {
            Action = action;
        }
    }
}