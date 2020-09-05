using System;
using System.Collections.Generic;
using System.Linq;

namespace dp
{
    internal class ValueTableTester
    {
        public static void Test()
        {
            const double probabilityOfHeads = 0.4;
            const int dollarsToWin = 100;

            var gamblersWorld = new GamblersWorld(probabilityOfHeads, dollarsToWin);
            var rewarder = new GamblersWorldRewarder(gamblersWorld);
            var policy = new UniformRandomGamblersPolicy();

            var gamblersValues = new GamblersValueTable(gamblersWorld);
            var genericValues = new ValueTable<GamblersWorldState, GamblersWorldAction>(gamblersWorld);

            gamblersValues.Evaluate(policy, rewarder);
            genericValues.Evaluate(policy, rewarder);

            foreach (var state in gamblersWorld.AllStates())
            {
                var genericValue = genericValues.Value(state);
                var gamblerValue = genericValues.Value(state);

                if (Math.Abs(genericValue - genericValue) > double.Epsilon)
                {
                    throw new Exception($"values not equal for state {state}. " +
                                        $"generic: {genericValue}, gambler: {gamblerValue}");
                }
            }

            Console.WriteLine("PASS: All values match!");
        }
        //
        // public static void Test2()
        // {
        //     var gridWorld = new GridWorld();
        //     var rewarder = new NegativeAtNonTerminalStatesGridWorldRewarder();
        //     var policy = new UniformRandomGridWorldPolicy();
        //
        //     var gridValues = new GridWorldValueTable(gridWorld);
        //     var genericValues = new ValueTable<GridWorldState, GridWorldAction>(gridWorld);
        //
        //     gridValues.Evaluate(policy, rewarder);
        //     genericValues.Evaluate(policy, rewarder);
        //
        //     foreach (var state in gridWorld.AllStates())
        //     {
        //         var genericValue = genericValues.Value(state);
        //         var gamblerValue = genericValues.Value(state);
        //
        //         if (Math.Abs(genericValue - genericValue) > double.Epsilon)
        //         {
        //             throw new Exception($"values not equal for state {state}. " +
        //                                 $"generic: {genericValue}, gambler: {gamblerValue}");
        //         }
        //     }
        //
        //     Console.WriteLine("PASS: All values match!");
        // }
    }

    internal class ValueTable<TState, TAction>
    {
        private readonly IProblem<TState, TAction> _problem;

        private readonly Dictionary<TState, double> _values;

        public ValueTable(IProblem<TState, TAction> problem)
        {
            _problem = problem;
            _values = problem.AllStates().ToDictionary(s => s, s => 0.0);
        }

        public void Evaluate(
            IPolicy<TState, TAction> policy,
            IGenericRewarder<TState, TAction> rewarder,
            int sweepLimit = -1)
        {
            var numSweeps = 0;
            var largestValueChange = 0.0;

            do
            {
                largestValueChange = 0.0;

                foreach (var state in _problem.AllStates())
                {
                    var originalValue = Value(state);
                    var newValue = CalculateValue(state, policy, rewarder);

                    _values[state] = newValue;

                    var valueChange = Math.Abs(originalValue - newValue);
                    if (valueChange > largestValueChange) largestValueChange = valueChange;
                }

                if (sweepLimit > 0 && ++numSweeps == sweepLimit) break;

            } while (largestValueChange > 0.000001);
        }

        private double CalculateValue(
            TState state,
            IPolicy<TState, TAction> policy,
            IGenericRewarder<TState, TAction> rewarder)
        {
            var newValue = 0.0;

            foreach (var action in _problem.AvailableActions(state))
            {
                foreach (var (nextState, pNextState) in _problem.PossibleStates(state, action))
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

        public double Value(TState state)
        {
            return _values[state];
        }
    }

    internal interface IProblem<TState, TAction>
    {
        IEnumerable<TState> AllStates();
        IEnumerable<TAction> AvailableActions(TState state);

        /// <summary>
        /// Returns all possible states and their probability from the given state and action
        /// </summary>
        IEnumerable<(TState, double)> PossibleStates(TState state, TAction action);
    }

    internal interface IPolicy<in TState, in TAction>
    {
        double PAction(TState state, TAction action);
    }

    internal interface IGenericRewarder<in TState, in TAction>
    {
        double Reward(TState state, TState nextState, TAction action);
    }
}
