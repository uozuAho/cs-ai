using System;
using System.Collections.Generic;
using System.Linq;
using ailib.Utils;

namespace CliffWalking.Agent
{
    /// <summary>
    /// N-step Sarsa policy improver. On-policy.
    /// </summary>
    public class NStepSarsaCliffWalker : ICliffWalkingAgent
    {
        private readonly double _chanceOfRandomAction;
        private readonly double _learningRate;
        private readonly Random _random = new();
        private readonly StateActionValues _stateActionValues = new();
        private readonly int _numSteps;

        /// <summary>
        /// </summary>
        /// <param name="chanceOfRandomAction">Also known as epsilon</param>
        /// <param name="learningRate"></param>
        /// <param name="numSteps"></param>
        public NStepSarsaCliffWalker(
            double chanceOfRandomAction = 0.1, double learningRate = 0.1, int numSteps = 2)
        {
            _chanceOfRandomAction = chanceOfRandomAction;
            _learningRate = learningRate;
            _numSteps = numSteps;
        }

        public StateActionValues ImproveEstimates(
            CliffWalkingEnvironment env, out TrainingDiagnostics diagnostics, int numEpisodes=10000)
        {
            var currentEpisode = 0;
            diagnostics = new TrainingDiagnostics();

            for (; currentEpisode < numEpisodes; currentEpisode++)
            {
                var states = new List<Position>();
                var actions = new List<CliffWalkingAction>();
                var rewards = new List<double> { 0.0 };

                states.Add(env.Reset());
                var action = GetAction(env, states[0]);
                var rewardSum = 0.0;

                var episodeLength = int.MaxValue;
                var t = 0;
                var tau = 0;

                for (; tau < episodeLength - 1; t++)
                {
                    tau = t - _numSteps + 1;
                    if (t < episodeLength)
                    {
                        actions.Add(action);
                        var (nextState, reward, done) = env.Step(action);
                        states.Add(nextState);
                        rewards.Add(reward);
                        action = GetAction(env, nextState);
                        rewardSum += reward;

                        if (done) episodeLength = t + 1;
                    }
                    if (tau >= 0)
                    {
                        var G = rewards.Skip(tau + 1).Take(_numSteps).Sum();
                        if (tau + _numSteps < episodeLength)
                            G += Value(states[tau + _numSteps], action);

                        var currentValue = Value(states[tau], actions[tau]);
                        var updatedValue = currentValue + _learningRate * (G - currentValue);
                        SetValue(states[tau], actions[tau], updatedValue);
                    }
                }

                diagnostics.RewardSumPerEpisode.Add(rewardSum);
            }

            return _stateActionValues;
        }

        private CliffWalkingAction GetAction(CliffWalkingEnvironment env, Position state)
        {
            return ShouldDoExploratoryAction()
                ? RandomAction(env)
                : GreedyAction(env, state);
        }

        private CliffWalkingAction GreedyAction(CliffWalkingEnvironment env, Position currentPosition)
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
