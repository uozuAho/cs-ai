namespace ailib.Algorithms.Search.NonDeterministic
{
    public class NonDeterministicSearchSolutionNode<TAction> : INonDeterministicSearchSolutionNode<TAction>
    {
        public TAction Action { get; }
        
        public NonDeterministicSearchSolutionNode(TAction action)
        {
            Action = action;
        }
    }
}