namespace ailib.Algorithms.Search
{
    public class SearchNode<TState, TAction>
    {
        /// <summary>
        /// State at this node
        /// </summary>
        public TState State { get; }

        /// <summary>
        /// Action that resulted in this state
        /// </summary>
        public TAction Action { get; }
        
        /// <summary>
        /// Previous state
        /// </summary>
        public SearchNode<TState, TAction> Parent { get; }
        
        /// Path cost at this node = parent.path_cost + step_cost(parent, action) */
        public double PathCost;

        public SearchNode(TState state,
            SearchNode<TState, TAction> parent,
            TAction action,
            double pathCost)
        {
            State = state;
            Parent = parent;
            Action = action;
            PathCost = pathCost;
        }
    }
}