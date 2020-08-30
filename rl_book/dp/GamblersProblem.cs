using System;
using System.Collections.Generic;
using System.Linq;

namespace dp
{
    class GamblersProblem
    {
        public static void Run()
        {
            Test();
            RunImpl();
        }

        private static void RunImpl()
        {
            var world = new GamblersWorld();
            IGamblersPolicy policy = new UniformRandomGamblersPolicy();
            var rewarder = new GamblersWorldRewarder();
            
            var values = new GamblersValueTable(world);

            values.Evaluate(policy, rewarder, 10);
            Console.WriteLine("Values:");
            values.Print();
            values.Evaluate(policy, rewarder);
            Console.WriteLine("Values:");
            values.Print();

            policy = GreedyGamblersPolicy.Create(world, values, rewarder);
            Console.WriteLine("Greedy policy:");
            ((GreedyGamblersPolicy) policy)?.Print();

            values.Evaluate(policy, rewarder);
            Console.WriteLine("Values:");
            values.Print();

            policy = GreedyGamblersPolicy.Create(world, values, rewarder);
            Console.WriteLine("Greedy policy:");
            ((GreedyGamblersPolicy) policy)?.Print();

            // for (var i = 0; i < 100; i++)
            // {
            //     // todo: this currently evaluates with many iterations. change to value iteration,
            //     // see how it affects convergence rate
            //     values.Evaluate(policy, rewarder);
            //     policy = GreedyGamblersPolicy.Create(world, values, rewarder);
            // }
            //
            // Console.WriteLine("Policy:");
            // (policy as GreedyGamblersPolicy)?.Print();
            // Console.WriteLine();
            // Console.WriteLine("Values:");
            // values.Print();
        }

        private static void Test()
        {
            var world = new GamblersWorld();
            Assert("world has 101 states", world.AllStates().Count() == 101);
            Assert("0 possible actions when $0 in hand", !world.AvailableActions(new GamblersWorldState(0)).Any());
            Assert("6 possible actions when $5 in hand", world.AvailableActions(new GamblersWorldState(5)).Count() == 6);
            Assert("0 possible actions when $100 in hand", !world.AvailableActions(new GamblersWorldState(100)).Any());

            for (var i = 0; i < 101; i++)
            {
                var possibleStates = world.PossibleStates(new GamblersWorldState(i), new GamblersWorldAction(1)).ToList();
                Assert("2 possible states on an action", possibleStates.Count == 2);
                Assert("probability of states sums to 1", possibleStates.Sum(s => s.Item2) == 1.0);
                Assert("probability of losing is 0.6", possibleStates.Single(s => s.Item1.DollarsInHand == i - 1).Item2 == 0.6);
            }
        }

        private static void Assert(string description, bool condition)
        {
            if (!condition)
            {
                Console.WriteLine("Failed: " + description);
            }
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
            if (state.IsTerminal) return Enumerable.Empty<GamblersWorldAction>();

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

        public bool Equals(GamblersWorldAction other)
        {
            return Stake == other.Stake;
        }

        public override bool Equals(object obj)
        {
            return obj is GamblersWorldAction other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Stake;
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

    internal class GreedyGamblersPolicy : IGamblersPolicy
    {
        // private readonly Dictionary<GamblersWorldState, GamblersWorldAction> _actions;
        private readonly int[] _actions;

        private GreedyGamblersPolicy()
        {
            // _actions = new Dictionary<GamblersWorldState, GamblersWorldAction>();
            _actions = new int[101];
        }

        public static GreedyGamblersPolicy Create(
            GamblersWorld world,
            GamblersValueTable valueTable,
            IGamblersWorldRewarder rewarder)
        {
            var greedyPolicy = new GreedyGamblersPolicy();

            foreach (var state in world.AllStates())
            {
                // greedyPolicy._actions[state] = FindBestAction(world, state, valueTable, rewarder);
                var bestAction = FindBestAction(world, state, valueTable, rewarder);
                greedyPolicy._actions[state.DollarsInHand] = bestAction.Stake;
            }

            return greedyPolicy;
        }

        public double PAction(in GamblersWorldState state, in GamblersWorldAction action)
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

            foreach (var action in world.AvailableActions(state))
            {
                var actionValue = 0.0;

                foreach (var (nextState, pNextState) in world.PossibleStates(state, action))
                {
                    var nextStateValue = valueTable.Value(nextState);
                    var reward = rewarder.Reward(state, nextState, action);

                    actionValue += pNextState * (reward + nextStateValue);
                }

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
                Console.WriteLine($"{i:000}: {_actions[i]}");
            }
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
            _values = new double[101];
            _values[100] = 1.0;
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
            if (state.DollarsInHand == 0) return 0.0;
            if (state.DollarsInHand == 100) return 1.0;

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
                Console.WriteLine($"{i:000}: {_values[i]}");
            }
        }
    }
}
