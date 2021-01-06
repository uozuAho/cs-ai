using RLCommon;

namespace dp.Examples.GridWorld
{
    public class NegativeAtNonTerminalStatesGridWorldRewarder :
        IGridWorldRewarder,
        IRewarder<GridWorldState, GridWorldAction>
    {
        public double Reward(GridWorldState state, GridWorldAction action)
        {
            if (state.IsTerminal) return 0;

            return -1;
        }

        public double Reward(GridWorldState state, GridWorldState nextState, GridWorldAction action)
        {
            if (state.IsTerminal) return 0;

            return -1;
        }
    }
}