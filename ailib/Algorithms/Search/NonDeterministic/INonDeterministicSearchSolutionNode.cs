namespace ailib.Algorithms.Search.NonDeterministic
{
    public interface INonDeterministicSearchSolutionNode<out TAction>
    {
        /// <summary>
        /// Action to take at this node
        /// </summary>
        TAction Action { get; }
    }
}