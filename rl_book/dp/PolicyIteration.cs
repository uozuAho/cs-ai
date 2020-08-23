using System;
using System.Collections.Generic;
using System.Linq;

namespace dp
{
    class PolicyIteration
    {
        public static void Run()
        {
            var world = new GridWorld();
            var randomPolicy = new UniformRandomPolicy();
            var rewarder = new NegativeAtNonTerminalStatesRewarder();

            var values = new ValueTable(world);

            // manually iterate a couple of times - optimal policy is greedy wrt
            // initial random policy values

            values.Evaluate(randomPolicy, rewarder);
            values.Print();

            var greedyPolicy = GreedyPolicy.Create(world, values, rewarder);

            values.Evaluate(greedyPolicy, rewarder);
            values.Print();

            greedyPolicy = GreedyPolicy.Create(world, values, rewarder);

            values.Evaluate(greedyPolicy, rewarder);
            values.Print();

            greedyPolicy.Print();
        }
    }

    internal class GreedyPolicy : IGridWorldPolicy
    {
        private readonly Dictionary<GridWorldState, GridWorld.Action> _actions;

        private GreedyPolicy()
        {
            _actions = new Dictionary<GridWorldState, GridWorld.Action>();
        }

        public static GreedyPolicy Create(
            GridWorld world,
            ValueTable valueTable,
            IRewarder rewarder)
        {
            var greedyPolicy = new GreedyPolicy();

            foreach (var state in world.AllStates())
            {
                greedyPolicy._actions[state] = FindBestAction(world, state, valueTable, rewarder);
            }

            return greedyPolicy;
        }

        public double PAction(GridWorldState state, GridWorld.Action action)
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

        private static GridWorld.Action FindBestAction(
            GridWorld world,
            GridWorldState state,
            ValueTable valueTable,
            IRewarder rewarder)
        {
            var max = double.MinValue;
            var maxAction = GridWorld.Action.Down;

            foreach (var action in world.AvailableActions(state))
            {
                var nextState = world.NextState(state, action);
                var nextStateValue = valueTable.Value(nextState);
                var reward = rewarder.Reward(state, action);

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
