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
            values.Print();
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
        double PAction(in GamblersWorldState state, in GamblersWorldAction action);
    }

    internal class UniformRandomGamblersPolicy : IGamblersPolicy
    {
        public double PAction(in GamblersWorldState state, in GamblersWorldAction action)
        {
            if (action.Stake > state.DollarsInHand) return 0.0;

            return 1.0 / (state.DollarsInHand + 1);
        }
    }

    internal interface IGamblersWorldRewarder
    {
        double Reward(
            in GamblersWorldState oldState,
            in GamblersWorldState newState,
            in GamblersWorldAction action);
    }

    internal class GamblersWorldRewarder : IGamblersWorldRewarder
    {
        public double Reward(
            in GamblersWorldState oldState,
            in GamblersWorldState newState,
            in GamblersWorldAction action)
        {
            if (oldState.IsTerminal) return 0.0;
            if (newState.DollarsInHand == 100) return 1;
            return 0;
        }
    }

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
        public void Evaluate(IGamblersPolicy policy, IGamblersWorldRewarder rewarder)
        {
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

            } while (largestValueChange > 0.0001);
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

        private double Value(GamblersWorldState state)
        {
            return _values[state.DollarsInHand];
        }

        public void Print()
        {
            for (var i = 0; i < _values.Length; i++)
            {
                Console.WriteLine($"{i:00}: {_values[i]}");
            }
        }
    }
}
