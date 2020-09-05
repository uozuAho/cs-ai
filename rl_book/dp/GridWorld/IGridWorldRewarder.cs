namespace dp.GridWorld
{
    interface IGridWorldRewarder
    {
        double Reward(GridWorldState state, GridWorldAction action);
    }
}