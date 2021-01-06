using System;
using System.Collections.Generic;
using RLCommon;

namespace dp.Examples.GridWorld
{
    /// <summary>
    /// Grid world problem of the DP chapter of the RL book. 4x4 grid, where 0,0
    /// and 3,3 are the terminal states.
    /// </summary>
    public class GridWorld : IProblem<GridWorldState, GridWorldAction>
    {
        public IEnumerable<GridWorldAction> AvailableActions(GridWorldState state)
        {
            return (GridWorldAction[])Enum.GetValues(typeof(GridWorldAction));
        }

        public IEnumerable<(GridWorldState, double)> PossibleStates(GridWorldState state, GridWorldAction action)
        {
            yield return (NextState(state, action), 1.0);
        }

        public IEnumerable<GridWorldState> AllStates()
        {
            for (var i = 0; i < 16; i++)
            {
                yield return new GridWorldState(i);
            }
        }

        public GridWorldState NextState(GridWorldState state, GridWorldAction action)
        {
            if (state.IsTerminal) return state;

            var x = state.Position1D % 4;
            var y = state.Position1D / 4;

            var newX = x;
            var newY = y;

            switch (action)
            {
                case GridWorldAction.Up:
                    if (y > 0) newY--;
                    break;
                case GridWorldAction.Down:
                    if (y < 3) newY++;
                    break;
                case GridWorldAction.Left:
                    if (x > 0) newX--;
                    break;
                case GridWorldAction.Right:
                    if (x < 3) newX++;
                    break;
            }

            return new GridWorldState(newY * 4 + newX);
        }
    }
}