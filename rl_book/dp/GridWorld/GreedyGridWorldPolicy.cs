using System;
using System.Collections.Generic;
using System.Linq;

namespace dp.GridWorld
{
    internal class GreedyGridWorldPolicy : IGridWorldPolicy
    {
        private readonly Dictionary<GridWorldState, GridWorldAction> _actions;

        private GreedyGridWorldPolicy()
        {
            _actions = new Dictionary<GridWorldState, GridWorldAction>();
        }

        public static GreedyGridWorldPolicy Create(
            GridWorld world,
            GridWorldValueTable gridWorldValueTable,
            IGridWorldRewarder gridWorldRewarder)
        {
            var greedyPolicy = new GreedyGridWorldPolicy();

            foreach (var state in world.AllStates())
            {
                greedyPolicy._actions[state] = FindBestAction(world, state, gridWorldValueTable, gridWorldRewarder);
            }

            return greedyPolicy;
        }

        public double PAction(GridWorldState state, GridWorldAction action)
        {
            return action == _actions[state] ? 1 : 0;
        }

        public void Print()
        {
            for (var y = 0; y < 4; y++)
            {
                for (var x = 0; x < 4; x++)
                {
                    var idx = y * 4 + x;
                    var action = _actions.Single(a => a.Key.Position1D == idx).Value;
                    Console.Write(action.ToString().PadRight(6));
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static GridWorldAction FindBestAction(
            GridWorld world,
            GridWorldState state,
            GridWorldValueTable gridWorldValueTable,
            IGridWorldRewarder gridWorldRewarder)
        {
            var max = double.MinValue;
            var maxAction = GridWorldAction.Down;

            foreach (var action in world.AvailableActions(state))
            {
                var nextState = world.NextState(state, action);
                var nextStateValue = gridWorldValueTable.Value(nextState);
                var reward = gridWorldRewarder.Reward(state, action);

                if (reward + nextStateValue > max)
                {
                    max = reward + nextStateValue;
                    maxAction = action;
                }
            }

            return maxAction;
        }
    }
}