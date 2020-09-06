namespace dp.GridWorld
{
    public interface IGridWorldPolicy
    {
        /// <summary>
        /// probability of the action from the given state
        /// </summary>
        double PAction(GridWorldState state, GridWorldAction action);
    }
}