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
    public class Td0CliffWalker : ICliffWalkingAgent
    {
        private readonly double _chanceOfRandomAction;
        private readonly double _learningRate;
        private readonly Random _random = new();
        private readonly StateActionValues _stateActionValues = new();

        /// <summary>
        /// </summary>
        /// <param name="chanceOfRandomAction">Also known as epsilon</param>
        /// <param name="learningRate"></param>
        public Td0CliffWalker(double chanceOfRandomAction, double learningRate)
        {
            _chanceOfRandomAction = chanceOfRandomAction;
            _learningRate = learningRate;
        }

        public StateActionValues ImproveEstimates(
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

                    var tdError = reward + Value(nextState, nextAction) - Value(state, action);
                    var updatedValue = Value(state, action) + _learningRate * tdError;
                    SetValue(state, action, updatedValue);

                    state = nextState;
                    action = nextAction;
                }

                diagnostics.RewardSumPerEpisode.Add(rewardSum);
            }

            Console.WriteLine($"Ran {iterationCount} iterations in {stopwatch.ElapsedMilliseconds} ms");

            return _stateActionValues;
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
