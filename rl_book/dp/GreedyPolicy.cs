using System.Collections.Generic;
using System.Linq;

namespace dp
{
    internal class GreedyPolicy<TState, TAction>
        : IPolicy<TState, TAction>, IDeterminatePolicy<TState, TAction>
    {
        private readonly IProblem<TState, TAction> _problem;
        private readonly Dictionary<TState, TAction> _actions;

        private GreedyPolicy(IProblem<TState, TAction> problem)
        {
            _problem = problem;
            _actions = new Dictionary<TState, TAction>();
        }

        public static GreedyPolicy<TState, TAction> Create(
            IProblem<TState, TAction> problem,
            ValueTable<TState, TAction> valueTable,
            IRewarder<TState, TAction> rewarder)
        {
            var greedyPolicy = new GreedyPolicy<TState, TAction>(problem);

            foreach (var state in problem.AllStates())
            {
                var bestAction = FindBestAction(problem, state, valueTable, rewarder);
                greedyPolicy._actions[state] = bestAction;
            }

            return greedyPolicy;
        }

        public double PAction(TState state, TAction action)
        {
            return action.Equals(_actions[state]) ? 1 : 0;
        }

        public TAction Action(TState state)
        {
            const double approxOne = 1 - double.Epsilon;

            return _problem
                .AvailableActions(state)
                .FirstOrDefault(action => PAction(state, action) >= approxOne);
        }

        private static TAction FindBestAction(
            IProblem<TState, TAction> problem,
            TState state,
            ValueTable<TState, TAction> valueTable,
            IRewarder<TState, TAction> rewarder)
        {
            var maxActionValue = double.MinValue;
            var maxAction = default(TAction);

            // var actionValues = new List<(GamblersWorldAction, double)>();

            foreach (var action in problem.AvailableActions(state))
            {
                var actionValue = 0.0;

                foreach (var (nextState, pNextState) in problem.PossibleStates(state, action))
                {
                    var nextStateValue = valueTable.Value(nextState);
                    var reward = rewarder.Reward(state, nextState, action);

                    actionValue += pNextState * (reward + nextStateValue);
                }

                // actionValues.Add((action, actionValue));

                if (actionValue > maxActionValue)
                {
                    maxActionValue = actionValue;
                    maxAction = action;
                }
            }

            return maxAction;
        }
    }

    internal interface IDeterminatePolicy<TState, TAction>
    {
        TAction Action(TState state);
    }
}
