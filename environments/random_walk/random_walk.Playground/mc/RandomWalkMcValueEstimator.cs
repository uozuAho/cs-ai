using System.Collections.Generic;
using System.Linq;

namespace random_walk.Playground.mc
{
    class RandomWalkMcValueEstimator
    {
        private double[] _values;
        private StateReturns _returns;

        public double[] Estimate(RandomWalkEnvironment environment, int? episodeLimit = null)
        {
            _values = new double[environment.NumPositions + 1];
            _returns = new StateReturns(environment.NumPositions + 1);

            var maxEpisodes = episodeLimit ?? 10000;

            for (var i = 0; i < maxEpisodes; i++)
            {
                ImproveEstimates();
            }

            return _values;
        }

        private void ImproveEstimates()
        {
            var rewardSum = 0.0;
            var episode = Episode.Generate(new RandomWalkEnvironment(5));

            foreach (var t in Enumerable.Range(0, episode.Length - 1).Reverse())
            {
                var state = episode.Steps[t].State;
                rewardSum += episode.Steps[t + 1].Reward;

                if (episode.TimeOfFirstVisit(state) == t)
                {
                    _returns.Add(state, rewardSum);
                    _values[state] = _returns.AverageReturnFrom(state);
                }
            }
        }
    }

    internal class StateReturns
    {
        private readonly List<double>[] _returns;

        public StateReturns(int numStates)
        {
            _returns = Enumerable.Range(0, numStates + 1)
                .Select(_ => new List<double>()).ToArray();
        }

        public void Add(int state, double value)
        {
            _returns[state].Add(value);
        }

        public double AverageReturnFrom(int state)
        {
            return _returns[state].Average();
        }
    }

    internal class Episode
    {
        public int Length => Steps.Count;
        public List<EpisodeStep> Steps { get; set; } = new();

        public static Episode Generate(RandomWalkEnvironment env)
        {
            var episode = new Episode();
            env.Reset();
            RandomWalkStepResult step;

            do
            {
                step = env.Step();
                episode.Steps.Add(new EpisodeStep(step.State, step.Reward));
            } while (!step.IsDone);

            return episode;
        }

        public int TimeOfFirstVisit(int state)
        {
            return Steps.FindIndex(step => step.State == state);
        }
    }

    internal record EpisodeStep(int State, double Reward);
}