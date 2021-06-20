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
        private readonly StateActionValues _stateActionValues = new();
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

        public StateActionValues ImproveEstimates(
            CliffWalkingEnvironment env,
            out TrainingDiagnostics diagnostics,
            int numEpisodes)
        {
            var iterationCount = 0;
            diagnostics = new TrainingDiagnostics();

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

                    state = nextState;
                    action = nextAction;

                    _model.Update(state, action, nextState, reward);
                    ImproveEstimatesWithModel(env);
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
            return env
                .ActionSpace()
                .Select(a => new
                {
                    action = a,
                    value = Value(currentPosition, a)
                })
                .OrderByDescending(av => av.value)
                .First().action;
        }

        private double Value(Position position, CliffWalkingAction action)
        {
            return _stateActionValues.Value(position, action);
        }

        private void SetValue(Position position, CliffWalkingAction action, double value)
        {
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
        public IEnumerable<Position> ObservedStates => Enumerable.Empty<Position>();

        public void Update(Position state, CliffWalkingAction action, Position nextState, double reward)
        {
        }

        public IEnumerable<CliffWalkingAction> ActionsTakenAt(Position state)
        {
            return Enumerable.Empty<CliffWalkingAction>();
        }

        public Step DoAction(Position state, CliffWalkingAction action)
        {
            return new Step(state, 0.0, false);
        }
    }
}
