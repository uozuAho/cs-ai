namespace dp.Examples.GridWorld
{
    public interface IGridWorldRewarder
    {
        double Reward(GridWorldState state, GridWorldAction action);
    }
}