using System;
using System.Collections.Generic;
using System.Linq;

namespace dp
{
    public class ValueTable<TState, TAction>
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

    public interface IProblem<TState, TAction>
    {
        IEnumerable<TState> AllStates();
        IEnumerable<TAction> AvailableActions(TState state);

        /// <summary>
        /// Returns all possible states and their probability from the given state and action
        /// </summary>
        IEnumerable<(TState, double)> PossibleStates(TState state, TAction action);
    }

    public interface IPolicy<in TState, in TAction>
    {
        double PAction(TState state, TAction action);
    }

    public interface IGenericRewarder<in TState, in TAction>
    {
        double Reward(TState state, TState nextState, TAction action);
    }
}
