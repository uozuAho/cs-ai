using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using ailib.Utils;
using CliffWalking.Agent.DataStructures;

namespace CliffWalking.Agent
{
    /// <summary>
    /// </summary>
    public class ExpectedSarsaCliffWalker : ICliffWalkingAgent
    {
        private readonly double _chanceOfRandomAction;
        private readonly double _learningRate;
        private readonly Random _random = new();
        private readonly StateActionValues _stateActionValues = new();

        /// <summary>
        /// </summary>
        /// <param name="chanceOfRandomAction">Also known as epsilon</param>
        /// <param name="learningRate"></param>
        public ExpectedSarsaCliffWalker(double chanceOfRandomAction = 0.1, double learningRate = 0.1)
        {
            _chanceOfRandomAction = chanceOfRandomAction;
            _learningRate = learningRate;
        }

        public IStateActionValues ImproveEstimates(
            CliffWalkingEnvironment env, out TrainingDiagnostics diagnostics, int iterations=10000)
        {
            var iterationCount = 0;
            var stopwatch = Stopwatch.StartNew();
            diagnostics = new TrainingDiagnostics();

            for (; iterationCount < iterations; iterationCount++)
            {
                var state = env.Reset();
                var action = GetAction(env, state);
                var nextAction = action;
                var isDone = false;
                var rewardSum = 0.0;

                while (!isDone)
                {
                    var (nextState, reward, done) = env.Step(nextAction);
                    rewardSum += reward;
                    // next action is e-greedy
                    nextAction = GetAction(env, nextState);
                    isDone = done;

                    var updatedValue = ExpectedSarsaEstimateValue(state, action, reward, nextState, env);
                    SetValue(state, action, updatedValue);

                    state = nextState;
                    action = nextAction;
                }

                diagnostics.RewardSumPerEpisode.Add(rewardSum);
            }

            return _stateActionValues;
        }

        private double ExpectedSarsaEstimateValue(
            Position state,
            CliffWalkingAction action,
            double reward,
            Position nextState,
            CliffWalkingEnvironment env)
        {
            var expectedNextValue = ExpectedNextValue(nextState, env);
            var tdError = reward + expectedNextValue - Value(state, action);
            return Value(state, action) + _learningRate * tdError;
        }

        private double ExpectedNextValue(Position nextState, CliffWalkingEnvironment env)
        {
            var availableActions = env.ActionSpace().ToList();

            if (availableActions.Count == 1)
            {
                return Value(nextState, availableActions.Single());
            }

            var bestAction = BestAction(env, nextState);
            var probabilityOfBestAction = 1 - _chanceOfRandomAction;
            var probabilityOfEachRandomAction = _chanceOfRandomAction / (availableActions.Count - 1);

            double ProbabilityOfAction(CliffWalkingAction action)
            {
                if (availableActions.Count == 1) return 1;

                return action == bestAction ? probabilityOfBestAction : probabilityOfEachRandomAction;
            }

            return env.ActionSpace().Sum(a => ProbabilityOfAction(a) * Value(nextState, a));
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
}
