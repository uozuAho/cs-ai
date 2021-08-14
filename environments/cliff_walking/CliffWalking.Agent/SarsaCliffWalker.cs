using System;
using System.Diagnostics;
using System.Linq;
using ailib.Utils;

namespace CliffWalking.Agent
{
    /// <summary>
    /// 1-step temporal difference policy improver
    /// - SARSA
    /// - on policy
    /// </summary>
    public class SarsaCliffWalker : ICliffWalkingAgent
    {
        private readonly double _chanceOfRandomAction;
        private readonly double _learningRate;
        private readonly Random _random = new();
        private readonly StateActionValues _stateActionValues = new();

        /// <summary>
        /// </summary>
        /// <param name="chanceOfRandomAction">Also known as epsilon</param>
        /// <param name="learningRate"></param>
        public SarsaCliffWalker(double chanceOfRandomAction = 0.1, double learningRate = 0.1)
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
                    nextAction = GetAction(env, nextState);
                    isDone = done;
                    rewardSum += reward;

                    var updatedValue = SarsaEstimateValue(state, action, reward, nextState, nextAction);
                    SetValue(state, action, updatedValue);

                    state = nextState;
                    action = nextAction;
                }

                diagnostics.RewardSumPerEpisode.Add(rewardSum);
            }

            return _stateActionValues;
        }

        private double SarsaEstimateValue(
            Position state,
            CliffWalkingAction action,
            double reward,
            Position nextState,
            CliffWalkingAction nextAction)
        {
            // Q(s,a) <-- Q(s,a) + alpha[reward + Q(s', a') - Q(s, a)]
            var tdError = reward + Value(nextState, nextAction) - Value(state, action);
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
}
