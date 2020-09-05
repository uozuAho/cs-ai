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
            const double probabilityOfHeads = 0.4;
            const int dollarsToWin = 3;

            var world = new GamblersWorld(probabilityOfHeads, dollarsToWin);
            var rewarder = new GamblersWorldRewarder(world);
            var values = new GamblersValueTable(world);

            Console.WriteLine("Random policy");
            IGamblersPolicy policy = new UniformRandomGamblersPolicy();
            EvaluatePolicy(world, policy);

            Console.WriteLine("Always $1 policy");
            policy = new AlwaysStake1DollarPolicy();
            EvaluatePolicy(world, policy);

            // policy = GreedyGamblersPolicy.Create(world, values, rewarder);
            // Console.WriteLine("Greedy policy:");
            // ((GreedyGamblersPolicy) policy)?.Print();
            //
            // values.Evaluate(policy, rewarder);
            // Console.WriteLine("Values:");
            // values.Print();
            //
            // policy = GreedyGamblersPolicy.Create(world, values, rewarder);
            // Console.WriteLine("Greedy policy:");
            // ((GreedyGamblersPolicy) policy)?.Print();

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

        private static void EvaluatePolicy(GamblersWorld world, IGamblersPolicy policy)
        {
            var rewarder = new GamblersWorldRewarder(world);
            var values = new GamblersValueTable(world);

            values.Evaluate(policy, rewarder, 1);
            Console.WriteLine("Values:");
            values.Print();
            values.Evaluate(policy, rewarder, 1);
            Console.WriteLine("Values:");
            values.Print();
            values.Evaluate(policy, rewarder, 1);
            Console.WriteLine("Values:");
            values.Print();
        }

        private static void Test()
        {
            const double probabilityOfHeads = 0.4;

            for (var dollarsToWin = 5; dollarsToWin < 10; dollarsToWin++)
            {
                var world = new GamblersWorld(probabilityOfHeads, dollarsToWin);
                Assert($"world has {dollarsToWin + 1} states", world.AllStates().Count() == dollarsToWin + 1);
                Assert("0 possible actions when $0 in hand", !world.AvailableActions(new GamblersWorldState(0)).Any());
                Assert("4 possible actions when $3 in hand", world.AvailableActions(new GamblersWorldState(3)).Count() == 4);
                Assert("0 possible actions when $goal in hand", !world.AvailableActions(new GamblersWorldState(dollarsToWin)).Any());
            }

            var world2 = new GamblersWorld(probabilityOfHeads, 100);

            for (int i = 0; i < 100; i++)
            {
                var possibleStates = world2.PossibleStates(new GamblersWorldState(i), new GamblersWorldAction(1)).ToList();
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

    internal class GamblersWorld : IProblem<GamblersWorldState, GamblersWorldAction>
    {
        public readonly int DollarsToWin;

        private readonly double _probabilityOfHeads;
        private List<GamblersWorldState> _allStates;

        public GamblersWorld(double probabilityOfHeads, int dollarsToWin)
        {
            _probabilityOfHeads = probabilityOfHeads;
            DollarsToWin = dollarsToWin;
        }

        public bool IsTerminal(GamblersWorldState state) =>
            state.DollarsInHand == 0 || state.DollarsInHand == DollarsToWin;

        public IEnumerable<GamblersWorldState> AllStates()
        {
            return _allStates ??= Enumerable
                .Range(0, DollarsToWin + 1)
                .Select(i => new GamblersWorldState(i))
                .ToList();
        }

        public IEnumerable<GamblersWorldAction> AvailableActions(GamblersWorldState state)
        {
            if (IsTerminal(state)) return Enumerable.Empty<GamblersWorldAction>();

            var maxStake = Math.Min(state.DollarsInHand, DollarsToWin - state.DollarsInHand);

            return Enumerable.Range(0, maxStake + 1).Select(i => new GamblersWorldAction(i));
        }

        public IEnumerable<(GamblersWorldState, double)>
            PossibleStates(GamblersWorldState state, GamblersWorldAction action)
        {
            yield return (
                new GamblersWorldState(state.DollarsInHand + action.Stake),
                _probabilityOfHeads
            );

            yield return (
                new GamblersWorldState(state.DollarsInHand - action.Stake),
                1.0 - _probabilityOfHeads
            );
        }
    }

    internal readonly struct GamblersWorldState
    {
        public int DollarsInHand { get; }

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
        double PAction(GamblersWorldState state, GamblersWorldAction action);
    }

    internal class UniformRandomGamblersPolicy : IGamblersPolicy, IPolicy<GamblersWorldState, GamblersWorldAction>
    {
        public double PAction(GamblersWorldState state, GamblersWorldAction action)
        {
            if (action.Stake > state.DollarsInHand) return 0.0;

            return 1.0 / (state.DollarsInHand + 1);
        }
    }

    internal class AlwaysStake1DollarPolicy : IGamblersPolicy
    {
        public double PAction(GamblersWorldState state, GamblersWorldAction action)
        {
            return action.Stake == 1 ? 1.0 : 0;
        }
    }

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
                Console.WriteLine($"{i}: {_actions[i]}");
            }
        }
    }

    internal interface IGamblersWorldRewarder
    {
        double Reward(
            GamblersWorldState oldState,
            GamblersWorldState newState,
            GamblersWorldAction action);
    }

    internal class GamblersWorldRewarder :
        IGamblersWorldRewarder,
        IGenericRewarder<GamblersWorldState, GamblersWorldAction>
    {
        private readonly GamblersWorld _world;

        public GamblersWorldRewarder(GamblersWorld world)
        {
            _world = world;
        }

        public double Reward(
            GamblersWorldState oldState,
            GamblersWorldState newState,
            GamblersWorldAction action)
        {
            if (_world.IsTerminal(oldState)) return 0.0;
            if (newState.DollarsInHand == _world.DollarsToWin) return 1;
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
            if (state.DollarsInHand == 0) return 0.0;
            if (state.DollarsInHand == _world.DollarsToWin) return 1.0;

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
