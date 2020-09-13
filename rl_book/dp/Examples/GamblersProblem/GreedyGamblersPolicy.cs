using System;
using System.Collections.Generic;
using System.Linq;

namespace dp.Examples.GamblersProblem
{
    internal class GreedyGamblersPolicy : IGamblersPolicy
    {
        private readonly int[] _actions;

        private GreedyGamblersPolicy(GamblersWorld world)
        {
            _actions = new int[world.AllStates().Count()];
        }

        public static GreedyGamblersPolicy Create(
            GamblersWorld world,
            GamblersValueTable valueTable,
            IGamblersWorldRewarder rewarder)
        {
            var greedyPolicy = new GreedyGamblersPolicy(world);

            foreach (var state in world.AllStates())
            {
                var bestAction = FindBestAction(world, state, valueTable, rewarder);
                greedyPolicy._actions[state.DollarsInHand] = bestAction.Stake;
            }

            return greedyPolicy;
        }

        public double PAction(GamblersWorldState state, GamblersWorldAction action)
        {
            return action.Stake == _actions[state.DollarsInHand] ? 1 : 0;
        }

        private static GamblersWorldAction FindBestAction(
            GamblersWorld world,
            GamblersWorldState state,
            GamblersValueTable valueTable,
            IGamblersWorldRewarder rewarder)
        {
            var maxActionValue = double.MinValue;
            var maxAction = new GamblersWorldAction(0);

            var actionValues = new List<(GamblersWorldAction, double)>();

            foreach (var action in world.AvailableActions(state))
            {
                var actionValue = 0.0;

                foreach (var (nextState, pNextState) in world.PossibleStates(state, action))
                {
                    var nextStateValue = valueTable.Value(nextState);
                    var reward = rewarder.Reward(state, nextState, action);

                    actionValue += pNextState * (reward + nextStateValue);
                }

                actionValues.Add((action, actionValue));

                if (actionValue > maxActionValue)
                {
                    maxActionValue = actionValue;
                    maxAction = action;
                }
            }

            return maxAction;
        }

        public void Print()
        {
            for (var i = 0; i < _actions.Length; i++)
            {
                Console.WriteLine($"{i}: {_actions[i]}");
            }
        }
    }
}