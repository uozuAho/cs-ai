using System;
using System.Linq;

namespace dp.Examples.GamblersProblem
{
    public class GamblersValueTable
    {
        private readonly GamblersWorld _world;
        private readonly double[] _values;

        public GamblersValueTable(GamblersWorld world)
        {
            _world = world;
            _values = new double[_world.AllStates().Count()];
            _values[_world.DollarsToWin] = 1.0;
        }

        // todo: change this to value iteration (interleave evaluation and improvement)
        public void Evaluate(IGamblersPolicy policy, IGamblersWorldRewarder rewarder, int sweepLimit = -1)
        {
            var numSweeps = 0;
            var largestValueChange = 0.0;

            do
            {
                largestValueChange = 0.0;

                foreach (var state in _world.AllStates())
                {
                    var originalValue = Value(state);
                    var newValue = CalculateValue(state, policy, rewarder);

                    _values[state.DollarsInHand] = newValue;

                    var valueChange = Math.Abs(originalValue - newValue);
                    if (valueChange > largestValueChange) largestValueChange = valueChange;
                }

                if (sweepLimit > 0 && ++numSweeps == sweepLimit) break;

            } while (largestValueChange > 0.000001);
        }

        private double CalculateValue(
            GamblersWorldState state, IGamblersPolicy policy, IGamblersWorldRewarder rewarder)
        {
            var newValue = 0.0;

            foreach (var action in _world.AvailableActions(state))
            {
                foreach (var (nextState, pNextState) in _world.PossibleStates(state, action))
                {
                    var reward = rewarder.Reward(state, nextState, action);
                    newValue += 
                        policy.PAction(state, action)
                        * pNextState
                        * (reward + Value(nextState));
                }
            }

            return newValue;
        }

        public double Value(GamblersWorldState state)
        {
            return _values[state.DollarsInHand];
        }

        public void Print()
        {
            for (var i = 0; i < _values.Length; i++)
            {
                Console.WriteLine($"{i}: {_values[i]}");
            }
        }
    }
}