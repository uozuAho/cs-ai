using System;
using System.Collections.Generic;
using System.Linq;
using ailib.Utils;

namespace CliffWalking.Agent
{
    public class DynaQCliffWalker : ICliffWalkingAgent
    {
        private readonly double _chanceOfRandomAction;
        private readonly double _learningRate;
        private readonly int _numPlanningSteps;
        private readonly Random _random = new();
        private StateActionValuesFaster? _stateActionValues;
        private readonly CliffWalkingEnvironmentModel _model;

        public DynaQCliffWalker(
            double chanceOfRandomAction,
            double learningRate,
            int numPlanningSteps)
        {
            _chanceOfRandomAction = chanceOfRandomAction;
            _learningRate = learningRate;
            _numPlanningSteps = numPlanningSteps;
            _model = new CliffWalkingEnvironmentModel();
        }

        public IStateActionValues ImproveEstimates(
            CliffWalkingEnvironment env,
            out TrainingDiagnostics diagnostics,
            int numEpisodes)
        {
            var iterationCount = 0;
            diagnostics = new TrainingDiagnostics();

            var (x, y) = env.Dimensions;
            _stateActionValues = new StateActionValuesFaster(x, y);

            for (; iterationCount < numEpisodes; iterationCount++)
            {
                var state = env.Reset();
                var action = GetAction(env, state);
                var nextAction = action;
                var isDone = false;
                var rewardSum = 0.0;

                while (!isDone)
                {
                    var (nextState, reward, done) = env.Step(nextAction);
                    nextAction = GetAction(env, nextState);
                    isDone = done;
                    rewardSum += reward;

                    var updatedValue = QLearningEstimateValue(state, action, reward, nextState, env);
                    SetValue(state, action, updatedValue);

                    _model.Update(state, action, nextState, reward);
                    ImproveEstimatesWithModel(env);

                    state = nextState;
                    action = nextAction;
                }

                diagnostics.RewardSumPerEpisode.Add(rewardSum);
            }

            return _stateActionValues;
        }

        private void ImproveEstimatesWithModel(CliffWalkingEnvironment env)
        {
            for (var i = 0; i < _numPlanningSteps; i++)
            {
                var state = _random.Choice(_model.ObservedStates);
                var action = _random.Choice(_model.ActionsTakenAt(state));
                var (nextState, reward, _) = _model.DoAction(state, action);
                var updatedValue = QLearningEstimateValue(state, action, reward, nextState, env);
                SetValue(state, action, updatedValue);
            }
        }

        private double QLearningEstimateValue(
            Position state,
            CliffWalkingAction action,
            double reward,
            Position nextState,
            CliffWalkingEnvironment env)
        {
            // assume next action is best
            // Q(s,a) <-- Q(s,a) + alpha[reward + best(Q(s',a')) - Q(s,a)]
            var bestNextAction = BestAction(env, nextState);
            var tdError = reward + Value(nextState, bestNextAction) - Value(state, action);
            return Value(state, action) + _learningRate * tdError;
        }

        private CliffWalkingAction GetAction(CliffWalkingEnvironment env, Position state)
        {
            return ShouldDoExploratoryAction()
                ? RandomAction(env)
                : BestAction(env, state);
        }

        private CliffWalkingAction BestAction(CliffWalkingEnvironment env, Position currentPosition)
        {
            var bestAction = CliffWalkingAction.Down;
            var highestValue = double.MinValue;
            
            foreach (var action in env.ActionSpace())
            {
                var value = Value(currentPosition, action);
                if (value > highestValue)
                {
                    bestAction = action;
                    highestValue = value;
                }
            }

            return bestAction;
        }

        private double Value(Position position, CliffWalkingAction action)
        {
            if (_stateActionValues == null) throw new NullReferenceException();

            return _stateActionValues.Value(position, action);
        }

        private void SetValue(Position position, CliffWalkingAction action, double value)
        {
            if (_stateActionValues == null) throw new NullReferenceException();

            _stateActionValues.SetValue(position, action, value);
        }

        private CliffWalkingAction RandomAction(CliffWalkingEnvironment env)
        {
            return _random.Choice(env.ActionSpace());
        }

        private bool ShouldDoExploratoryAction()
        {
            return _random.NextDouble() < _chanceOfRandomAction;
        }
    }

    internal class CliffWalkingEnvironmentModel
    {
        private readonly HashSet<Position>
            _observedStates = new();
        private readonly Dictionary<Position, HashSet<CliffWalkingAction>>
            _actionsTakenAt = new();
        private readonly Dictionary<(Position, CliffWalkingAction), (Position, double)>
            _map = new();

        public IEnumerable<Position>
            ObservedStates => _observedStates;
        public IEnumerable<CliffWalkingAction>
            ActionsTakenAt(Position state) => _actionsTakenAt[state];

        public void Update(Position state, CliffWalkingAction action, Position nextState, double reward)
        {
            _observedStates.Add(state);
            if (_actionsTakenAt.ContainsKey(state))
            {
                _actionsTakenAt[state].Add(action);
            }
            else
            {
                _actionsTakenAt[state] = new HashSet<CliffWalkingAction> {action};
            }
            _map[(state, action)] = (nextState, reward);
        }

        public Step DoAction(Position state, CliffWalkingAction action)
        {
            var (nextState, reward) = _map[(state, action)];
            return new Step(nextState, reward, false);
        }
    }
}
