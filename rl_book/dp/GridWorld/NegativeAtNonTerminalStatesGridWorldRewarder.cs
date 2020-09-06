namespace dp.GridWorld
{
    public class NegativeAtNonTerminalStatesGridWorldRewarder :
        IGridWorldRewarder,
        IGenericRewarder<GridWorldState, GridWorldAction>
    {
        public double Reward(GridWorldState state, GridWorldAction action)
        {
            if (state.IsTerminal) return 0;

            return -1;
        }

        public double Reward(GridWorldState state, GridWorldState nextState, GridWorldAction action)
        {
            if (nextState.IsTerminal) return 0;

            return -1;
        }
    }
}