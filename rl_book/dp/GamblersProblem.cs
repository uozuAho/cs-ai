using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace dp
{
    class GamblersProblem
    {
        public static void Run()
        {
            var world = new GamblersWorld();
            var randomPolicy = new UniformRandomGamblersPolicy();
            var rewarder = new GamblersWorldRewarder();
            
            var values = new GamblersValueTable(world);
            
            // manually iterate a couple of times - optimal policy is greedy wrt
            // initial random policy values
            
            values.Evaluate(randomPolicy, rewarder);
            // values.Print();
            //
            // var greedyPolicy = GreedyPolicy.Create(world, values, rewarder);
            //
            // values.Evaluate(greedyPolicy, rewarder);
            // values.Print();
            //
            // greedyPolicy = GreedyPolicy.Create(world, values, rewarder);
            //
            // values.Evaluate(greedyPolicy, rewarder);
            // values.Print();
            //
            // greedyPolicy.Print();
        }
    }

    internal class GamblersWorld
    {
        private const double ProbabilityOfHeads = 0.4;
        private List<GamblersWorldState> _allStates;

        public IEnumerable<GamblersWorldState> AllStates()
        {
            return _allStates ??= Enumerable
                .Range(0, 101)
                .Select(i => new GamblersWorldState(i))
                .ToList();
        }

        public IEnumerable<GamblersWorldAction> AvailableActions(GamblersWorldState state)
        {
            var maxStake = Math.Min(state.DollarsInHand, 100 - state.DollarsInHand);

            return Enumerable.Range(0, maxStake + 1).Select(i => new GamblersWorldAction(i));
        }

        public IEnumerable<Tuple<GamblersWorldState, double>>
            PossibleStates(GamblersWorldState state, GamblersWorldAction action)
        {
            yield return new Tuple<GamblersWorldState, double>(
                new GamblersWorldState(state.DollarsInHand + action.Stake), ProbabilityOfHeads);

            yield return new Tuple<GamblersWorldState, double>(
                new GamblersWorldState(state.DollarsInHand - action.Stake), 1.0 - ProbabilityOfHeads);
        }
    }

    internal readonly struct GamblersWorldState
    {
        public int DollarsInHand { get; }

        public bool IsTerminal => DollarsInHand == 0 || DollarsInHand == 100;

        public GamblersWorldState(int dollarsInHand)
        {
            DollarsInHand = dollarsInHand;
        }
    }

    internal readonly struct GamblersWorldAction
    {
        public int Stake { get; }

        public GamblersWorldAction(int stake)
        {
            Stake = stake;
        }
    }

    internal interface IGamblersPolicy
    {
    }

    internal class UniformRandomGamblersPolicy : IGamblersPolicy
    {

    }

    internal interface IGamblersWorldRewarder
    {
    }

    internal class GamblersWorldRewarder
    {
    }

    // todo: extract generic value table from this and grid world
    internal class GamblersValueTable
    {
        private readonly GamblersWorld _world;
        private readonly double[] _values;

        public GamblersValueTable(GamblersWorld world)
        {
            _world = world;
            _values = new double[world.AllStates().Count()];
        }

        // todo: change this to value iteration (interleave evaluation and improvement)
        public void Evaluate(IGamblersPolicy randomPolicy, IGamblersWorldRewarder rewarder)
        {
            var largestValueChange = 0.0;

            do
            {
                largestValueChange = 0.0;

                foreach (var state in _world.AllStates())
                {
                    var originalValue = Value(state);
                    var newValue = CalculateValue(state, policy, rewarder);

                    _values[state.Position1D] = newValue;

                    var valueChange = Math.Abs(originalValue - newValue);
                    if (valueChange > largestValueChange) largestValueChange = valueChange;
                }

            } while (largestValueChange > 0.0001);
        }

        private double CalculateValue(
            GamblersWorldState state, IGamblersPolicy policy, IGamblersWorldRewarder rewarder)
        {
            var newValue = 0.0;

            foreach (var action in _world.AvailableActions(state))
            {
                // todo: handle multiple possible states
                var nextState = _world.PossibleStates(state, action);
                var reward = rewarder.Reward(state, action);
                newValue += policy.PAction(state, action) * (reward + Value(nextState));
            }

            return newValue;
        }

        private double Value(GamblersWorldState state)
        {
            return _values[state.DollarsInHand];
        }
    }
}
