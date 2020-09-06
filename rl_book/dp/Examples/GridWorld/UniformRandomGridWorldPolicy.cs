namespace dp.Examples.GridWorld
{
    public class UniformRandomGridWorldPolicy : IGridWorldPolicy, IPolicy<GridWorldState, GridWorldAction>
    {
        public double PAction(GridWorldState state, GridWorldAction action)
        {
            return 0.25;
        }
    }
}