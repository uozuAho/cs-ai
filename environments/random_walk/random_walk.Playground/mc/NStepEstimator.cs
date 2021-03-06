﻿using System.Collections.Generic;
using System.Linq;

namespace random_walk.Playground.mc
{
    internal class NStepEstimator
    {
        private double[] _values;
        private readonly double _learningRate;
        private readonly int _numSteps;

        public NStepEstimator(double learningRate, int numSteps)
        {
            _learningRate = learningRate;
            _numSteps = numSteps;
        }

        public double[] Estimate(RandomWalkEnvironment environment, int? episodeLimit = null)
        {
            _values = Enumerable.Range(0, environment.NumPositions).Select(_ => 0.0).ToArray();

            var maxEpisodes = episodeLimit ?? 10000;

            for (var i = 0; i < maxEpisodes; i++)
            {
                var states = new List<int> {environment.Reset()};
                var rewards = new List<double> { 0.0 };

                var episodeLength = int.MaxValue;
                var t = 0;
                var tau = 0;

                for (; tau < episodeLength - 1; t++)
                {
                    tau = t - _numSteps + 1;
                    if (t < episodeLength)
                    {
                        var (nextState, reward, done) = environment.Step();
                        states.Add(nextState);
                        rewards.Add(reward);

                        if (done) episodeLength = t + 1;
                    }
                    if (tau >= 0)
                    {
                        var G = rewards.Skip(tau + 1).Take(_numSteps).Sum();
                        if (tau + _numSteps < episodeLength)
                            G += Value(states[tau + _numSteps]);

                        var currentValue = Value(states[tau]);
                        var updatedValue = currentValue + _learningRate * (G - currentValue);
                        _values[states[tau]] = updatedValue;
                    }
                }
            }

            return _values;
        }

        private double Value(int state)
        {
            if (state < 0 || state >= _values.Length) return 0;

            return _values[state];
        }
    }
}
